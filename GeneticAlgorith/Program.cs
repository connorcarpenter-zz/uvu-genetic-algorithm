using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    internal class Program
    {
        public static Random MainRandom = new Random();

        private static void Main(string[] args)
        {
            const string targetString = "A quick brown fox jumps over the lazy dog!";
            const int timesToTest = 50;
            var comparer = new HypothesisComparer();

            //testable variables
            const int populationSize = 12;
            const double percentToCrossover = 0.3;
            const double percentToMutate = 0.3;
            const double mutationIntensity = 0.5; // 0 is no mutation, 1 is infinite mutation (don't do this) 
            const int childPerPair = 2;
            IEvaluator evaluator = new OneToOneCharEvaluator(targetString);
            ICrossoverMethod crossoverMethod = new DoubleSplitCrossoverMethod();
            ICrossoverHandler crossoverHandler = new TournamentCrossoverHandler(crossoverMethod);
            ///////

            var tests = 0;
            var generationTotal = 0;
            var timeTotal = 0;
            while (tests < timesToTest)
            {
                var time = DateTime.Now;
                tests++;
                var hypothesisList = new List<Hypothesis>();
                for (var i = 0; i < populationSize; i++)
                {
                    hypothesisList.Add(Hypothesis.CreateRandomHypothesis());
                }

                var generations = 0;
                while (true)
                {
                    SortAndCull(comparer, populationSize, evaluator, hypothesisList);
                    var bestHypothesis = hypothesisList.FirstOrDefault();
                    if (bestHypothesis != null)
                    {
                        var bestString = bestHypothesis.String;
                        //Console.WriteLine(bestString);
                        //Console.ReadLine();
                        if (bestString.Equals(targetString)) break;
                    }
                    var newHypothesisList = crossoverHandler.CrossoverPopulation(hypothesisList, percentToCrossover, childPerPair);
                    hypothesisList.AddRange(newHypothesisList);
                    MutatePopulation(hypothesisList, percentToMutate, mutationIntensity);
                    
                    generations++;
                }

                var timeElapsed = (DateTime.Now - time).Milliseconds;
                generationTotal += generations;
                timeTotal += timeElapsed;
                Console.WriteLine("Test "+tests+" Found in: " + generations + " generations, "+timeElapsed+" ms");
            }

            Console.WriteLine("Average to find the goal over "+timesToTest+" trials: "+(generationTotal/timesToTest)+" generations, "+(timeTotal/timesToTest)+" ms");
            Console.ReadLine();
        }

        private static void SortAndCull(HypothesisComparer comparer, int populationSize, IEvaluator evaluator, List<Hypothesis> hypothesisList)
        {
            foreach (var h in hypothesisList)
            {
                h.Fitness = evaluator.EvaluateHypothesis(h);
            }
            hypothesisList.Sort(comparer);
            CullPopulation(hypothesisList, populationSize);
        }

        private static void CullPopulation(List<Hypothesis> population, int populationSize)
        {
            while (population.Count > populationSize)
            {
                population.RemoveAt(population.Count-1);
            }
        }

        private static void MutatePopulation(List<Hypothesis> hypothesisList, double percentToMutate, double mutationIntensity)
        {
            var populationToMutate = Math.Max(1,Convert.ToInt32(hypothesisList.Count*percentToMutate));
            for (var i = 0; i < populationToMutate; i++)
            {
                var index = MainRandom.Next(0, hypothesisList.Count);
                var hypothesisToMutate = hypothesisList[index];
                hypothesisList.RemoveAt(index);
                hypothesisList.Add(Hypothesis.CreateMutatedHypothesis(hypothesisToMutate, 1-mutationIntensity));
            }
        }
    }
}
