using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    class HypothesisComparer : IComparer<Hypothesis>
    {
        public int Compare(Hypothesis x, Hypothesis y)
        {
            return y.Fitness - x.Fitness;
        }
    }

    class Hypothesis
    {
        public string String { get; set; }
        public int Fitness { get; set; }
        public bool Evaluated { get; set; }

        public Hypothesis()
        {
            Evaluated = false;
        }

        public static Hypothesis CreateRandomHypothesis()
        {
            var newHypotheis = new Hypothesis();

            var length = Convert.ToInt32(Program.MainRandom.NextDouble() * (Math.Pow(Program.MainRandom.NextDouble() * 10, (Program.MainRandom.NextDouble() * 5))));
            newHypotheis.String = CreateRandomString(length);
            return newHypotheis;
        }

        public static string CreateRandomString(int length)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                sb.Append(GetRandomChar());
            }
            return sb.ToString();
        }

        public static Hypothesis CreateMutatedHypothesis(Hypothesis parent, double mutationIntensity)
        {
            var newHypothesis = new Hypothesis();
            var newString = new string(parent.String.ToCharArray());
            while (Program.MainRandom.NextDouble() > mutationIntensity)
            {
                var index = Program.MainRandom.Next(0, newString.Length);
                var type = Program.MainRandom.Next(0, 3);

                if (type != 0 && newString.Length > 0)
                    newString = newString.Remove(index, 1);
                if (type != 1)
                    newString = newString.Insert(index, GetRandomChar());
            }
            newHypothesis.String = newString;
            return newHypothesis;
        }

        public static string GetRandomChar()
        {
            return "" + Convert.ToChar(Program.MainRandom.Next(32, 127));
        }
    }
}
