using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    internal interface ICrossoverHandler
    {
        List<Hypothesis> CrossoverPopulation(List<Hypothesis> inputList, double crossoverRate, int childPerPair);
    }

    internal class FitnessPercentageCrossoverHandler : ICrossoverHandler
    {
        private readonly ICrossoverMethod _crossoverMethod;

        public FitnessPercentageCrossoverHandler(ICrossoverMethod crossoverMethod)
        {
            _crossoverMethod = crossoverMethod;
        }

        public List<Hypothesis> CrossoverPopulation(List<Hypothesis> population, double crossoverRate, int childPerPair)
        {
            var newPopulation = new List<Hypothesis>();
            var popToCrossover = Math.Max(1, population.Count * crossoverRate);
            var totalFitness = population.Sum(h => h.Fitness);
            var minimumFitness = population.Select(h => h.Fitness).Concat(new[] { 0 }).Min();
            if (minimumFitness < 0)
            {
                totalFitness = population.Sum(h => h.Fitness - minimumFitness);
            }
            for (var j = 0; j < popToCrossover; j++)
            {
                var candidateA = FindCrossOverCandidate(population, totalFitness, -minimumFitness);
                var candidateB = FindCrossOverCandidate(population, totalFitness, -minimumFitness);
                if (candidateA != null && candidateB != null)
                {
                    for (var i = 0; i < childPerPair; i++)
                        newPopulation.Add(_crossoverMethod.Crossover(candidateA, candidateB));
                }
            }
            return newPopulation;
        }

        private static Hypothesis FindCrossOverCandidate(List<Hypothesis> population, int totalFitness, int minFitness)
        {
            var index = Program.MainRandom.Next(0, Math.Max(0, totalFitness - 1));

            foreach (var h in population)
            {
                if (index <= h.Fitness + minFitness)
                    return h;
                index -= h.Fitness + minFitness;
            }

            return null;
        }
    }
}
