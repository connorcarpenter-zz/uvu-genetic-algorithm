using System;

namespace GeneticAlgorithm
{
    internal class OneToOneCharEvaluator : IEvaluator
    {
        private readonly string _targetString;

        public OneToOneCharEvaluator(string target)
        {
            _targetString = target;
        }

        public int EvaluateHypothesis(Hypothesis h)
        {
            if (h.Evaluated) return h.Fitness;
            var fitness = 0;
            var minLength = Math.Min(_targetString.Length, h.String.Length);
            for (var i = 0; i < minLength; i++)
            {
                var charA = h.String[i];
                var charB = _targetString[i];
                var result = 100 - Math.Abs(charA - charB);
                if (result < 0)
                {
                    var a = true;
                }
                fitness += result;
            }
            fitness -= Math.Abs(_targetString.Length - h.String.Length) * 20;
            h.Evaluated = true;
            return fitness;
        }
    }

    internal class ScanDistanceEvaluator : IEvaluator
    {
        private readonly string _targetString;

        public ScanDistanceEvaluator(string target)
        {
            _targetString = target;
        }

        public int EvaluateHypothesis(Hypothesis h)
        {
            if (h.Evaluated) return h.Fitness;
            var fitness = 10000;
            var hIndex = 0;
            var endScan = false;
            if (h.String.Length > 0)
            {
                foreach (var charToFind in _targetString)
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
                    fitness += 100;
                }
            }
            if (h.String.Length < _targetString.Length)
            {
                fitness -= (_targetString.Length - h.String.Length) * 10;
            }
            h.Evaluated = true;
            return fitness;
        }
    }

    internal interface IEvaluator
    {
        int EvaluateHypothesis(Hypothesis h);
    }
}
