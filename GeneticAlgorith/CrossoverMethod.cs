using System;
using System.Text;

namespace GeneticAlgorithm
{
    internal interface ICrossoverMethod
    {
        Hypothesis Crossover(Hypothesis hypothesisA, Hypothesis hypothesisB);
    }

    internal class DoubleSplitCrossoverMethod : ICrossoverMethod
    {
        public Hypothesis Crossover(Hypothesis hypothesisA, Hypothesis hypothesisB)
        {
            var minLength = Math.Min(hypothesisA.String.Length, hypothesisB.String.Length);
            if (minLength == 0)
            {
                return hypothesisA.String.Length > hypothesisB.String.Length ? hypothesisA : hypothesisB;
            }
            var indexA = Program.MainRandom.Next(0, minLength);
            var indexB = Program.MainRandom.Next(0, minLength);
            var minIndex = Math.Min(indexA, indexB);
            var maxIndex = Math.Max(indexA, indexB);
            Hypothesis hypA;
            Hypothesis hypB;
            if (Program.MainRandom.NextDouble() > 0.5)
            {
                hypA = hypothesisA;
                hypB = hypothesisB;
            }
            else
            {
                hypA = hypothesisB;
                hypB = hypothesisA;
            }

            var firstPart = hypA.String.Substring(0, minIndex);
            var secondPart = hypB.String.Substring(minIndex, maxIndex - minIndex);
            var thirdPart = hypA.String.Substring(maxIndex, hypA.String.Length - maxIndex);
            var newString = "" + firstPart + secondPart + thirdPart;
            var newHypothesis = new Hypothesis { String = newString };
            return newHypothesis;
        }
    }

    internal class SingleSplitCrossoverMethod : ICrossoverMethod
    {
        public Hypothesis Crossover(Hypothesis hypothesisA, Hypothesis hypothesisB)
        {
            var minLength = Math.Min(hypothesisA.String.Length, hypothesisB.String.Length);
            if (minLength == 0)
            {
                return hypothesisA.String.Length > hypothesisB.String.Length ? hypothesisA : hypothesisB;
            }
            var index = Program.MainRandom.Next(0, minLength);
            Hypothesis hypA;
            Hypothesis hypB;
            if (Program.MainRandom.NextDouble() > 0.5)
            {
                hypA = hypothesisA;
                hypB = hypothesisB;
            }
            else
            {
                hypA = hypothesisB;
                hypB = hypothesisA;
            }

            var firstPart = hypA.String.Substring(0, index);
            var secondPart = hypB.String.Substring(index, hypB.String.Length - index);
            var newString = "" + firstPart + secondPart;
            var newHypothesis = new Hypothesis { String = newString };
            return newHypothesis;
        }
    }

    internal class UniformCrossoverMethod : ICrossoverMethod
    {
        public Hypothesis Crossover(Hypothesis hypothesisA, Hypothesis hypothesisB)
        {
            var sb = new StringBuilder();
            var maxLength = Math.Max(hypothesisA.String.Length, hypothesisB.String.Length);

            for (var i = 0; i < maxLength; i++)
            {
                if (i >= hypothesisA.String.Length)
                {
                    sb.Append("" + hypothesisB.String[i]);
                    continue;
                }
                if (i >= hypothesisB.String.Length)
                {
                    sb.Append("" + hypothesisA.String[i]);
                    continue;
                }
                if (Program.MainRandom.NextDouble() > 0.5)
                {
                    sb.Append("" + hypothesisA.String[i]);
                }
                else
                {
                    sb.Append("" + hypothesisB.String[i]);
                }
            }

            var newHypothesis = new Hypothesis();
            newHypothesis.String = sb.ToString();
            return newHypothesis;
        }
    }
}
