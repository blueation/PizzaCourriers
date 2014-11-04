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

		public static int nrTestRuns = 100;
		public static int testBestCost = int.MaxValue;
		public static int testMeanCost = 0;

        public static int CurrentCost = 0;
        public static int BestSolutionCost = 0;
        public static string BestSolutionOutput;

        public static double cooldown;          //only used with linear and exponential
        public static double temperature;       //always used
        public static double limit;             //always used
        public static int changepercooldown;    //always used
        public static int imax;                 //always used
        public static int OptimalCost;          //only used with logarithmic and is the cost of the optimal solution. This would be an approximation in practice
        public static int EnergyBarrier;        //only used with logarithmic and is calculated at the start from OptimalCost and the starting solution
        public static double V_s;               //only used with thermodynamic speed
        public static double Epsilon;           //only used with thermodynamic speed
        public static int Capacity;             //only used with thermodynamic speed

        public static int opt2chance = 50;
        public static int opt2halfchance = 50;

        //public static string schedule = "constant";
        //public static string schedule = "linear";
        //public static string schedule = "exponential";
        public static string schedule = "logarithmic";
        //public static string schedule = "speed";

        static void Main(string[] args)
        {
			for (int y = 0; y < nrTestRuns; y++) {
				//initialize uninitialized data, no clue wether these values are correct or not.
                switch (schedule)
                {
                    case "constant":
                        temperature = 10.0;
                        limit = 1.0;
                        changepercooldown = 800;
                        imax = 1000000;
                        break;
                    case "linear":
                        cooldown = 1.0;
                        temperature = 10.0;
                        limit = 1.0;
                        changepercooldown = 800;
                        imax = 1000000;
                        break;
                    case "exponential":
                        cooldown = 0.98;
                        temperature = 10.0;
                        limit = 1.0;
                        changepercooldown = 800;
                        imax = 1000000;
                        break;
                    case "logarithmic":
                        OptimalCost = 100;
                        //temperature is set later
                        limit = 9;
                        changepercooldown = 80;
                        imax = int.MaxValue;
                        break;
                    case "speed":
                        //V_s = ;
                        //Epsilon = ;
                        //Capacity = ;
                        temperature = 10.0;
                        limit = 1.0;
                        changepercooldown = 800;
                        imax = 1000000;
                        break;

                }

				nodelist = new List<Node>();
				bezorgers = new Bezorger[1];
				CurrentCost = 0;
				BestSolutionCost = 0;

				//load map into nodelist, restaurant into resX+resY
				InputFromFile ("GeoSquare2.txt");

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

                if (schedule == "logarithmic")
                {
                    EnergyBarrier = CurrentCost - OptimalCost;
                    temperature = EnergyBarrier / Math.Log(1 + 1);
                }

				if(y == 0)
					Console.WriteLine (BestSolutionCost);
				//Console.WriteLine(BestSolutionOutput);
            
				//optimalize
				SimulatedAnealing ();

				//Gather information multistart
				if (testBestCost > BestSolutionCost)
					testBestCost = BestSolutionCost;
				testMeanCost += BestSolutionCost;

				//output
				//BestSolutionOutput = StringSolution(); -> The solution the program has in the last state, is not the bestsolution
				//Console.WriteLine(BestSolutionCost);
				//Console.WriteLine(BestSolutionOutput);
				//foreach (Bezorger b in bezorgers)
				//Console.WriteLine(b.GetLength());
			}
			Console.WriteLine("Best value in " + nrTestRuns + " runs: " + testBestCost);
			Console.WriteLine("Mean cost over " + nrTestRuns + " of runs: " + (testMeanCost / nrTestRuns));
            Console.ReadLine();
        }


    }


}
