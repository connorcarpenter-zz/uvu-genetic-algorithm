using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeneticAlgorithm
{
    internal class Program
    {
        public static Random MainRandom = new Random();
        public static HypothesisComparer Comparer = new HypothesisComparer();

        private static void Main(string[] args)
        {
            var ExperimentSets = InitExperimentSets();

            while (true)
            {
                var allDone = true;
                foreach (var experimentSet in ExperimentSets)
                {
                    experimentSet.RunExperiment();
                    if (!experimentSet.Done)
                        allDone = false;
                }
                if (allDone)
                    break;
            }
        }

        private static List<ExperimentSet> InitExperimentSets()
        {
            var experimentSets = new List<ExperimentSet>();

            //Population Size
            var newSet = new ExperimentSet();
            experimentSets.Add(newSet);

            AddPopulationSizeExperiment(newSet, 10);
            AddPopulationSizeExperiment(newSet, 20);
            AddPopulationSizeExperiment(newSet, 30);
            AddPopulationSizeExperiment(newSet, 40);
            AddPopulationSizeExperiment(newSet, 50);
            AddPopulationSizeExperiment(newSet, 60);
            AddPopulationSizeExperiment(newSet, 70);
            AddPopulationSizeExperiment(newSet, 80);
            AddPopulationSizeExperiment(newSet, 90);
            AddPopulationSizeExperiment(newSet, 100);

            //Crossover Percent
            newSet = new ExperimentSet();
            experimentSets.Add(newSet);

            AddCrossoverPercentExperiment(newSet, 0.1);
            AddCrossoverPercentExperiment(newSet, 0.2);
            AddCrossoverPercentExperiment(newSet, 0.3);
            AddCrossoverPercentExperiment(newSet, 0.4);
            AddCrossoverPercentExperiment(newSet, 0.5);
            AddCrossoverPercentExperiment(newSet, 0.6);
            AddCrossoverPercentExperiment(newSet, 0.7);
            AddCrossoverPercentExperiment(newSet, 0.8);
            AddCrossoverPercentExperiment(newSet, 0.9);

            //Mutation Percent
            newSet = new ExperimentSet();
            experimentSets.Add(newSet);

            AddMutationPercentExperiment(newSet, 0.1);
            AddMutationPercentExperiment(newSet, 0.2);
            AddMutationPercentExperiment(newSet, 0.3);
            AddMutationPercentExperiment(newSet, 0.4);
            AddMutationPercentExperiment(newSet, 0.5);
            AddMutationPercentExperiment(newSet, 0.6);
            AddMutationPercentExperiment(newSet, 0.7);
            AddMutationPercentExperiment(newSet, 0.8);
            AddMutationPercentExperiment(newSet, 0.9);

            //Crossover Method
            newSet = new ExperimentSet();
            experimentSets.Add(newSet);

            AddCrossoverMethodExperiment(newSet, new UniformCrossoverMethod(), "Uniform");
            AddCrossoverMethodExperiment(newSet, new SingleSplitCrossoverMethod(), "Single Split");
            AddCrossoverMethodExperiment(newSet, new DoubleSplitCrossoverMethod(), "Double Split");

            //Crossover Handler
            newSet = new ExperimentSet();
            experimentSets.Add(newSet);

            AddCrossoverHandlerExperiment(newSet, new AlphaBetaCrossoverHandler(), "Alpha-Beta");
            AddCrossoverHandlerExperiment(newSet, new FitnessPercentageCrossoverHandler(), "Fitness Percentage");
            AddCrossoverHandlerExperiment(newSet, new FitnessRankCrossoverHandler(), "Fitness Rank");
            AddCrossoverHandlerExperiment(newSet, new TournamentCrossoverHandler(), "Tournament");

            return experimentSets;
        }

        private static void AddPopulationSizeExperiment(ExperimentSet newSet, int populationSize)
        {
            var newExperiment = new Experiment();
            newSet.Experiments.Add(newExperiment);
            newExperiment.PopulationSize = populationSize;
            newExperiment.Label = "Population Size: " + populationSize;
        }

        private static void AddCrossoverPercentExperiment(ExperimentSet newSet, double crossoverPercent)
        {
            var newExperiment = new Experiment();
            newSet.Experiments.Add(newExperiment);
            newExperiment.PercentToCrossover = crossoverPercent;
            newExperiment.Label = "Crossover Percent: " + crossoverPercent;
        }

        private static void AddMutationPercentExperiment(ExperimentSet newSet, double mutationPercent)
        {
            var newExperiment = new Experiment();
            newSet.Experiments.Add(newExperiment);
            newExperiment.PercentToMutate = mutationPercent;
            newExperiment.Label = "Mutation Percent: " + mutationPercent;
        }

        private static void AddCrossoverMethodExperiment(ExperimentSet newSet, ICrossoverMethod crossoverMethod, string title)
        {
            var newExperiment = new Experiment();
            newSet.Experiments.Add(newExperiment);
            newExperiment.CrossoverMethod = crossoverMethod;
            newExperiment.Label = "Crossover Method: " + title;
        }

        private static void AddCrossoverHandlerExperiment(ExperimentSet newSet, ICrossoverHandler crossoverHandler, string title)
        {
            var newExperiment = new Experiment();
            newSet.Experiments.Add(newExperiment);
            newExperiment.CrossoverHandler = crossoverHandler;
            newExperiment.Label = "Selection Process: " + title;
        }
    }

    class Experiment
    {
        public int PopulationSize { get; set; }
        public double PercentToCrossover { get; set; }
        public double PercentToMutate { get; set; }
        public double MutationIntensity { get; set; } // 0 is no mutation, 1 is infinite mutation (don't do this) 
        public int ChildPerPair { get; set; }
        public string Label { get; set; }

        private IEvaluator _evaluator;
        public IEvaluator Evaluator
        {
            get { return _evaluator; }
            set
            {
                var tempString = _evaluator.TargetString;
                _evaluator = value;
                _evaluator.TargetString = tempString;
            }
        }
        private ICrossoverHandler _crossoverHandler;
        public ICrossoverHandler CrossoverHandler
        {
            get { return _crossoverHandler; }
            set
            {
                var tempMethod = _crossoverHandler.CrossoverMethod;
                _crossoverHandler = value;
                _crossoverHandler.CrossoverMethod = tempMethod;
            }
        }
        public ICrossoverMethod CrossoverMethod
        {
            get { return CrossoverHandler.CrossoverMethod; }
            set { CrossoverHandler.CrossoverMethod = value; }
        }
        public string TargetString
        {
            get { return Evaluator.TargetString; }
            set { Evaluator.TargetString = value; }
        }

        public Experiment()
        {
            Evaluator = new OneToOneCharEvaluator();
            CrossoverHandler = new AlphaBetaCrossoverHandler();
            PopulationSize = 50;
            PercentToCrossover = 0.5;
            PercentToMutate = 0.5;
            MutationIntensity = 0.5; // 0 is no mutation, 1 is infinite mutation (don't do this) 
            ChildPerPair = 5;
            TargetString = "A quick brown fox jumps over the lazy dog!";
            CrossoverMethod = new DoubleSplitCrossoverMethod();
        }
    }

    class ExperimentSet
    {
        public string FileName { get; set; }
        public List<Experiment> Experiments = new List<Experiment>();
        public List<string> Results = new List<string>();
        public bool Done = false;
        
        public void RunExperiment()
        {
            if (Done) return;
            var nextExperiment = Experiments.FirstOrDefault();
            Experiments.RemoveAt(0);
            var result = ExecuteExperiment(nextExperiment);
            Results.Add(nextExperiment.Label + "," + result);

            if (Experiments.Count == 0)
            {
                File.WriteAllLines(@FileName, Results.ToArray());
                Done = true;
            }
        }

        private static int ExecuteExperiment(Experiment experiment)
        {
            //returns milliseconds to get target string
            const int timesToTest = 50;

            var tests = 0;
            //var generationTotal = 0;
            var timeTotal = 0;
            while (tests < timesToTest)
            {
                var time = DateTime.Now;
                tests++;
                var hypothesisList = new List<Hypothesis>();
                for (var i = 0; i < experiment.PopulationSize; i++)
                {
                    hypothesisList.Add(Hypothesis.CreateRandomHypothesis());
                }

                var generations = 0;
                while (true)
                {
                    SortAndCull(Program.Comparer, experiment.PopulationSize, experiment.Evaluator, hypothesisList);
                    var bestHypothesis = hypothesisList.FirstOrDefault();
                    if (bestHypothesis != null)
                    {
                        var bestString = bestHypothesis.String;
                        if (bestString.Equals(experiment.TargetString)) break;
                    }
                    var newHypothesisList = experiment.CrossoverHandler.CrossoverPopulation(hypothesisList, experiment.PercentToCrossover, experiment.ChildPerPair);
                    MutatePopulation(newHypothesisList, experiment.PercentToMutate, experiment.MutationIntensity);
                    hypothesisList.AddRange(newHypothesisList);

                    generations++;
                }

                var timeElapsed = (DateTime.Now - time).Milliseconds;
                //generationTotal += generations;
                timeTotal += timeElapsed;
                //Console.WriteLine("Test "+tests+" Found in: " + generations + " generations, "+timeElapsed+" ms");
            }

            //Console.WriteLine("Average to find the goal over "+timesToTest+" trials: "+(generationTotal/timesToTest)+" generations, "+(timeTotal/timesToTest)+" ms");
            //Console.ReadLine();
            return timeTotal / timesToTest;
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
                population.RemoveAt(population.Count - 1);
            }
        }

        private static void MutatePopulation(List<Hypothesis> hypothesisList, double percentToMutate, double mutationIntensity)
        {
            var populationToMutate = Math.Max(1, Convert.ToInt32(hypothesisList.Count * percentToMutate));
            for (var i = 0; i < populationToMutate; i++)
            {
                var index = Program.MainRandom.Next(0, hypothesisList.Count);
                var hypothesisToMutate = hypothesisList[index];
                hypothesisList.RemoveAt(index);
                hypothesisList.Add(Hypothesis.CreateMutatedHypothesis(hypothesisToMutate, 1 - mutationIntensity));
            }
        }
    }
}
