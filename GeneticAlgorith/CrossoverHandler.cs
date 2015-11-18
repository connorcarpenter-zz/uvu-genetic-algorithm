using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    interface ICrossoverHandler
    {
        List<Hypothesis> CrossoverPopulation(List<Hypothesis> inputList, double crossoverRate, int childPerPair);
        ICrossoverMethod CrossoverMethod { get; set; }
    }

    class AlphaBetaCrossoverHandler : ICrossoverHandler
    {
        public List<Hypothesis> CrossoverPopulation(List<Hypothesis> population, double crossoverRate, int childPerPair)
        {
            var newPopulation = new List<Hypothesis>();
            var popToCrossover = Math.Max(1, population.Count * crossoverRate);

            for (var j = 0; j < popToCrossover; j++)
            {
                var candidateA = population[0];
                var candidateB = population[1];
                if (candidateA == null || candidateB == null) continue;
                for (var i = 0; i < childPerPair; i++)
                    newPopulation.Add(CrossoverMethod.Crossover(candidateA, candidateB));
            }
            return newPopulation;
        }

        public ICrossoverMethod CrossoverMethod { get; set; }
    }

    class TournamentCrossoverHandler : ICrossoverHandler
    {
        public List<Hypothesis> CrossoverPopulation(List<Hypothesis> population, double crossoverRate, int childPerPair)
        {
            var newPopulation = new List<Hypothesis>();
            var popToCrossover = Math.Max(1, population.Count * crossoverRate);

            for (var j = 0; j < popToCrossover; j++)
            {
                var candidateA = FindCrossOverCandidate(population);
                var candidateB = FindCrossOverCandidate(population);
                if (candidateA == null || candidateB == null) continue;
                for (var i = 0; i < childPerPair; i++)
                    newPopulation.Add(CrossoverMethod.Crossover(candidateA, candidateB));
            }
            return newPopulation;
        }

        public ICrossoverMethod CrossoverMethod { get; set; }

        private static Hypothesis FindCrossOverCandidate(List<Hypothesis> population)
        {
            var candidateA = population[Program.MainRandom.Next(0, population.Count)];
            var candidateB = population[Program.MainRandom.Next(0, population.Count)];
            if (candidateA.Fitness < candidateB.Fitness)
                return candidateB;
            return candidateA;
        }
    }

    class FitnessRankCrossoverHandler : ICrossoverHandler
    {
        public List<Hypothesis> CrossoverPopulation(List<Hypothesis> population, double crossoverRate, int childPerPair)
        {
            var newPopulation = new List<Hypothesis>();
            var popToCrossover = Math.Max(1, population.Count * crossoverRate);
            var totalRank = population.Select((t, i) => population.Count - i).Sum();

            for (var j = 0; j < popToCrossover; j++)
            {
                var candidateA = FindCrossOverCandidate(population, totalRank);
                var candidateB = FindCrossOverCandidate(population, totalRank);
                if (candidateA == null || candidateB == null) continue;
                for (var i = 0; i < childPerPair; i++)
                    newPopulation.Add(CrossoverMethod.Crossover(candidateA, candidateB));
            }
            return newPopulation;
        }

        public ICrossoverMethod CrossoverMethod { get; set; }

        private static Hypothesis FindCrossOverCandidate(List<Hypothesis> population, int totalRank)
        {
            var index = Program.MainRandom.Next(0, Math.Max(0, totalRank - 1));

            for (var i=0;i<population.Count;i++)
            {
                var rank = population.Count - i;
                if (index <= rank)
                    return population[i];
                index -= rank;
            }

            return null;
        }
    }

    class FitnessPercentageCrossoverHandler : ICrossoverHandler
    {
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
                        newPopulation.Add(CrossoverMethod.Crossover(candidateA, candidateB));
                }
            }
            return newPopulation;
        }

        public ICrossoverMethod CrossoverMethod { get; set; }

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
