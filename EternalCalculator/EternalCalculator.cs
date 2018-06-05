﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EternalCalculator
{
    public class EternalCalculator
    {
        public CardCollection MasterCardCollection { get; }
        public CardCollection CurrentCardCollection { get; }
        public int NumTrials;
        public int[] PackCounts;

        public EternalCalculator(int numTrials = 10)
        {
            NumTrials = numTrials;
            MasterCardCollection = new CardCollection();
            PackCounts = new int[NumTrials];
        }

        public void Initialize()
        {
            foreach (Set set in MasterCardCollection.Sets.Keys)
            {
                foreach (Rarity rarity in MasterCardCollection.Sets[set].RarityGroups.Keys)
                {
                    MasterCardCollection.Sets[set].RarityGroups[rarity].Initialize();
                }
            }
        }

        public void ConductTrials()
        {
            // TODO
        }

        private void ConductTrial(int i)
        {
            // TODO
        }

        public void AnalyzeResults()
        {
            // TODO
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
