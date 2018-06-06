using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;

namespace EternalCalculator
{
    public class EternalCalculator
    {
        public CardCollection MasterCardCollection;
        public CardCollection CurrentCardCollection;
        public int NumTrials;
        Dictionary<Set, int[]> PackCounts;

        public EternalCalculator(int numTrials = 10)
        {
            NumTrials = numTrials;
            MasterCardCollection = new CardCollection();
            PackCounts = new Dictionary<Set, int[]>();
            foreach(Set set in MasterCardCollection.Sets.Keys)
            {
                PackCounts[set] = new int[numTrials];
            }
        }

        public void Initialize()
        {
            foreach (SetList setList in MasterCardCollection.Sets.Values)
            {
                foreach (RarityGroup rarityGroup in setList.RarityGroups.Values)
                {
                    rarityGroup.Initialize();
                }
            }
            this.CurrentCardCollection = MasterCardCollection.Clone();
        }

        public void ConductTrials(bool lazy = true, bool clearShiftStone = true)
        {
            for (int i = 0; i < NumTrials; i++)
            {
                Console.WriteLine(@"Working on trial {0}...", i);
                ConductTrial(i, clearShiftStone);
                CurrentCardCollection.Reset();
            }
        }

        private void ConductTrial(int i, bool lazy = true, bool clearShiftStone = true)
        {
            PackFactory packer = new PackFactory(MasterCardCollection);
            Pack pack;
            foreach (Set set in CurrentCardCollection.Sets.Keys)
            {
                while (!CurrentCardCollection.CanCraftRemainingCards(set))
                {
                    pack = packer.FillPack(set);
                    PackCounts[set][i]++;
                    CurrentCardCollection.AddPack(pack);
                    CurrentCardCollection.DestroyExcessCards(lazy);
                }
                CurrentCardCollection.CraftRemaingCards(set);
                if (clearShiftStone)
                {
                    CurrentCardCollection.ShiftStoneTotal = 0;
                }
            }
        }

        public void AnalyzeResults()
        {
            double[] quantiles = { 0.01, 0.05, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.95, 0.99 };
            foreach(Set set in PackCounts.Keys)
            {
                Console.WriteLine(Enum.GetName(typeof(Set), set));
                Console.WriteLine(@"    Quantile    Num Packs   Cost");
                foreach (double p in quantiles)
                {
                    IEnumerable<double> data = PackCounts[set].Select(i => (double)i);
                    double numPacks = Statistics.Quantile(data, p);
                    double cost = numPacks * Pack.GEMS_PER_PACK / Pack.GEMS_PER_DOLLAR;
                    Console.WriteLine(@"    {0}     {1}     {2}", p, numPacks, cost);
                }
            }
            
        }

        static int Main(string[] args)
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

            Console.WriteLine("Initializing master card collection...");
            calc.Initialize();

            Console.WriteLine("Staring experiment...");
            calc.ConductTrials();
            Console.WriteLine("Experiment finished.");

            Console.WriteLine("Results:");
            calc.AnalyzeResults();
            Console.WriteLine();

            Console.WriteLine("Press any key when finished.");
            Console.ReadKey();
            return 0;
        }
    }
}
