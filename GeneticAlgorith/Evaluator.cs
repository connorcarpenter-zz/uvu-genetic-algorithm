using System;

namespace GeneticAlgorithm
{
    class OccurenceEvaluator : IEvaluator
    {
        public int EvaluateHypothesis(Hypothesis h)
        {
            if (h.Evaluated) return h.Fitness;
            var fitness = 0;
            var minLength = Math.Min(TargetString.Length, h.String.Length);
            for (var i = 0; i < minLength; i++)
            {
                if (h.String[i] == TargetString[i])
                {
                    fitness += 20;
                }
                else
                {
                    for (var j = 1; j < 3; j++)
                    {
                        if (h.String[Math.Min(h.String.Length-1, i + j)] == TargetString[i])
                        {
                            fitness += 10 - j;
                            break;
                        }
                        if (h.String[Math.Max(0, i - j)] != TargetString[i]) continue;
                        fitness += 10 - j;
                        break;
                    }
                }
            }
            fitness -= Math.Abs(TargetString.Length - h.String.Length)*2;
            h.Evaluated = true;
            return fitness;
        }

        public string TargetString { get; set; }
    }

    class OneToOneCharEvaluator : IEvaluator
    {
        public int EvaluateHypothesis(Hypothesis h)
        {
            if (h.Evaluated) return h.Fitness;
            var fitness = 0;
            var minLength = Math.Min(TargetString.Length, h.String.Length);
            for (var i = 0; i < minLength; i++)
            {
                if(h.String[i] == TargetString[i])
                    fitness += 5;
            }
            fitness -= Math.Abs(TargetString.Length - h.String.Length);
            h.Evaluated = true;
            return fitness;
        }

        public string TargetString { get; set; }
    }

    class ScanDistanceEvaluator : IEvaluator
    {
        public int EvaluateHypothesis(Hypothesis h)
        {
            if (h.Evaluated) return h.Fitness;
            var fitness = 10000;
            var hIndex = 0;
            var endScan = false;
            if (h.String.Length > 0)
            {
                foreach (var charToFind in TargetString)
                {
                    while (h.String[hIndex] != charToFind)
                    {
                        hIndex++;
                        fitness -= 1;
                        if (hIndex < h.String.Length) continue;
                        endScan = true;
                        break;
                    }
                    if (endScan)
                        break;
                    fitness += 20;
                }
            }
            fitness -= Math.Abs(TargetString.Length - h.String.Length);
            h.Evaluated = true;
            return fitness;
        }

        public string TargetString { get; set; }
    }

    internal interface IEvaluator
    {
        int EvaluateHypothesis(Hypothesis h);
        string TargetString { get; set; }
    }
}
