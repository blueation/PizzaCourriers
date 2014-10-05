using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PizzaCourriers
{
    partial class Program
    {
        public static Random random = new Random();
        //public static Stopwatch stopwatch = new Stopwatch();

        public static List<Node> nodelist = new List<Node>();
        public static Bezorger[] bezorgers = new Bezorger[1];
        public static int resX, resY;

		public static int nrTestRuns = 10;
		public static int testBestCost = int.MaxValue;

        public static int CurrentCost = 0;
        public static int BestSolutionCost = 0;
        public static string BestSolutionOutput;

        public static double cooldown;
        public static double temperature;
        public static double limit;
        public static int changepercooldown;
        public static int imax;

        public static int opt2chance = 50;
        public static int opt2halfchance = 50;

        static void Main(string[] args)
        {
			for (int y = 0; y < nrTestRuns; y++) {
				//initialize uninitialized data, no clue wether these values are correct or not.
				cooldown = 0.98;
				temperature = 10.0;
				limit = 1.0;
				changepercooldown = 800;
				imax = 1000000;

				nodelist = new List<Node>();
				bezorgers = new Bezorger[1];
				CurrentCost = 0;
				BestSolutionCost = 0;

				//load map into nodelist, restaurant into resX+resY
				InputFromFile ("GeoSquare1.txt");

				//initialize map-dependant data
				for (int num = 0; num < bezorgers.Length; num++)
					bezorgers [num] = new Bezorger (num);

				//create initial solution, optimality is not important, at all at this point
				for (int i = 0; i < nodelist.Count; i++) {
					bezorgers [i % bezorgers.Length].AddNodetoEnd (nodelist [i]);
				}
				foreach (Bezorger B in bezorgers)
					BestSolutionCost += B.routeLength;
				CurrentCost = BestSolutionCost;
				BestSolutionOutput = StringSolution ();

				if(y == 0)
					Console.WriteLine (BestSolutionCost);
				//Console.WriteLine(BestSolutionOutput);
            
				//optimalize
				SimulatedAnealing ();

				if (testBestCost > BestSolutionCost)
					testBestCost = BestSolutionCost;

				//output
				//BestSolutionOutput = StringSolution(); -> The solution the program has in the last state, is not the bestsolution
				Console.WriteLine(BestSolutionCost);
				//Console.WriteLine(BestSolutionOutput);
				//foreach (Bezorger b in bezorgers)
				//Console.WriteLine(b.GetLength());
			}
			Console.WriteLine("Best value in " + nrTestRuns + " runs: " + testBestCost);
            Console.ReadLine();
        }


    }


}
