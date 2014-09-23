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
        public static Random random = new Random(9987);
        public static Stopwatch stopwatch = new Stopwatch();

        public static List<Node> nodelist = new List<Node>();
        public static Bezorger[] bezorgers = new Bezorger[4];
        public static int resX, resY;

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
            //initialize uninitialized data, no clue wether these values are correct or not.
            cooldown = 0.95;
            temperature = 20000.0;
            limit = 100.0;
            changepercooldown = 10;
            imax = 100000;

            //load map into nodelist, restaurant into resX+resY
            InputFromFile("Simple.txt");

            //initialize map-dependant data
            for (int num = 0; num < bezorgers.Length; num++)
                bezorgers[num] = new Bezorger(num);

            //create initial solution, optimality is not important, at all at this point
            for (int i = 0; i < nodelist.Count; i++)
            {
                bezorgers[i % 4].AddNodetoEnd(nodelist[i]);
            }
            foreach (Bezorger B in bezorgers)
                BestSolutionCost += B.routeLength;
            CurrentCost = BestSolutionCost;
            BestSolutionOutput = StringSolution();
            Console.WriteLine(BestSolutionOutput);
            
            //optimalize
            SimulatedAnealing();

            //output
        }


    }


}
