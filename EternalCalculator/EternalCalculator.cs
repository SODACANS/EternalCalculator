using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace EternalCalculator
{
    public class EternalCalculator
    {
        public int NumTrials;
        Dictionary<Set, int[]> PackCounts;

        public EternalCalculator(int numTrials = 10)
        {
            NumTrials = numTrials;
            PackCounts = new Dictionary<Set, int[]>();
            foreach(Set set in Enum.GetValues(typeof(Set)))
            {
                PackCounts[set] = new int[numTrials];
            }
        }

        public async Task ConductTrialsAsync()
        {
            var trials = new Task[NumTrials];
            for (int i = 0; i < NumTrials; i++)
            {
                var trial = Task.Factory.StartNew((index) => ConductTrial((int)index), i);
                trials[i] = trial;
            }
            await Task.WhenAll(trials);
        }

        private void ConductTrial(int i)
        {
            Console.WriteLine($"Working on trial {i}...");
            var packer = new PackFactory();
            var cardCollection = CardCollection.CreateCardCollection();
            foreach (Set set in Enum.GetValues(typeof(Set)))
            {
                while (!cardCollection.CanCraftRemainingCards(set))
                {
                    var pack = packer.FillPack(set);
                    PackCounts[set][i]++;
                    cardCollection.AddPack(pack);
                    cardCollection.DestroyExcessCards();
                }
                cardCollection.CraftRemainingCardsIfPossible(set);
                cardCollection.ResetShiftStoneTotal();
            }
        }

        public Dictionary<Set, List<(double Quantile, double NumPacks, decimal Cost)>> AnalyzeResults()
        {
            double[] quantiles = { 0.01, 0.05, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.95, 0.99 };
            var results = new Dictionary<Set, List<(double, double, decimal)>>();
            foreach(Set set in PackCounts.Keys)
            {
                var setResults = new List<(double, double, decimal)>();
                foreach (double p in quantiles)
                {
                    IEnumerable<double> data = PackCounts[set].Select(i => (double)i);
                    double numPacks = Statistics.Quantile(data, p);
                    decimal cost = (decimal)numPacks * Pack.GEMS_PER_PACK / Pack.GEMS_PER_DOLLAR;
                    setResults.Add((p, numPacks, cost));
                }
                results[set] = setResults;
            }
            return results;
        }

        public void PrintResults(Dictionary<Set, List<(double Quantile, double NumPacks, decimal Cost)>> results)
        {
            // See alignment value in: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated 
            string[] headers = { "Quantile", "Number of Packs", "Cost" };
            const int column1Width = 8;
            const int column2Width = 16;
            const int column3Width = 10;
            string formatString = $"{{0,{column1Width}:N2}} {{1,{column2Width}:N0}} {{2,{column3Width}:C}}";
            var stringBuilder = new StringBuilder();
            foreach (Set set in results.Keys)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"{set}:");
                stringBuilder.AppendLine(string.Format($"{{0,{column1Width}}} {{1,{column2Width}}} {{2,{column3Width}}}", headers));
                foreach (var (Quantile, NumPacks, Cost) in results[set])
                {
                    stringBuilder.AppendLine(string.Format(formatString, Quantile, NumPacks, Cost));
                }
            }
            Console.Write(stringBuilder.ToString());
        }

        // TODO: Use McMaster.Extensions.CommanlineUtils package (name might not be quite right) to set up
        // better command line parsing.
        static async Task<int> Main(string[] args)
        {
            EternalCalculator calc;
            if (args.Length < 1)
            {
                Console.WriteLine("No trial count given. Will conduct test with default trial count of 10.");
                calc = new EternalCalculator();
            }
            else
            {
                int numTrials = 0;
                if (Int32.TryParse(args[0],out numTrials))
                {
                    Console.WriteLine("Will conduct test with trial count of " + args[0]);
                    calc = new EternalCalculator(numTrials);
                }
                else
                {
                    Console.WriteLine("Failed to parse trial count. Usage: EternalCalculator 100");
                    return -1;
                }
            }

            Console.WriteLine("Conducting experiment...");
            await calc.ConductTrialsAsync();
            Console.WriteLine("Experiment finished.");

            Console.WriteLine("Results:");
            var results = calc.AnalyzeResults();
            calc.PrintResults(results);
            Console.WriteLine();

            Console.WriteLine("Press any key when finished.");
            Console.ReadKey();
            return 0;
        }
    }
}
