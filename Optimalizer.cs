using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PizzaCourriers
{
    partial class Program
    {
        public static void SimulatedAnealing()
        {
            int i = 0;
            //stopwatch.Start();
            while (/*temperature > limit &&*/ i < imax)
            {
                int r = random.Next(opt2chance + opt2halfchance);
                
                if (r < opt2chance)
                {
                    //Console.WriteLine("opt2");

                    Bezorger B = getRandomBezorgerNonEmpty;
                    Node n1 = B.getRandomNode;
                    Node n2 = B.getRandomNode;

                    Node ni = n1.next;
                    while (ni != null && ni != n2) ni = ni.next;
                    if (ni != n2)
                    {
                        ni = n2;
                        n2 = n1;
                        n1 = ni;
                    }

                    int before = QualityCost;
                    B.FlipOrder(n1, n2);
                    int after = CalculateQuality();
                    //int opt2cost = B.costsFlipOrder(n1, n2);
                    if (ShouldChange(after - before, temperature))
                    {
                        //B.FlipOrder(n1, n2);
                        //CurrentCost += opt2cost;
                        QualityCost = after;
                    }
                    else
                        B.FlipOrder(n2, n1);
                }
                else //(r < opt2chance + opt2halfchance)
                {
                    //Console.WriteLine("opt2half");

                    //int opt2halfcost = 0;
                    int before = QualityCost;
                    Bezorger B1 = getRandomBezorgerNonEmpty;
                    Node n = B1.getRandomNode;
                    //to make this easier to reason about, actually temporarily changing B1's route
                    //  -prevents B2 from returning n as location
                    //  -prevents B2's change cost to possibly be wrong.
                    Node reinsertlocationb1 = n.previous;
                    //opt2halfcost += B1.costsRemoveNode(n);
                    B1.RemoveNode(n);
                    
                    Bezorger B2 = bezorgers[random.Next(bezorgers.Length)];
                    Node l = B2.getRandomNodeOrNull;
                    //opt2halfcost += B2.costsAddAfter(n, l);
                    B2.AddAfter(n, l);
                    int after = CalculateQuality();

                    if (ShouldChange(after - before, temperature))
                    {
                        //B2.AddAfter(n, l);
                        //CurrentCost += opt2halfcost;
                        QualityCost = after;
                    }
                    else
                    {
                        B2.RemoveNode(n);
                        B1.AddAfter(n, reinsertlocationb1); //put back n where it belongs, should not change
                    }
                }

                //if (CurrentCost < BestSolutionCost)
                if (QualityCost < BestSolutionQuality)
                {
                    //BestSolutionCost = CurrentCost;
                    BestSolutionQuality = QualityCost;
                    BestSolutionOutput = StringSolution();
                    foreach (Bezorger b in Program.bezorgers)
                        b.GetLength();
                }

                //Console.WriteLine("----");
                i++;
                if (i % changepercooldown == 0)
                {
                    switch(schedule)
                    {
                        case "constant":
                            break;
                        case "linear":
                            temperature -= changepercooldown;
                            break;
                        case "exponential":
                            temperature *= cooldown;
                            break;
                        case "logarithmic":
                            temperature = EnergyBarrier / Math.Log(i + 1);
                            break;
                        case "speed":
                            temperature = -V_s * temperature / Epsilon / Math.Sqrt(Capacity);
                            break;
                    }
                }
            }
            //stopwatch.Stop();

            if (temperature <= limit)
                Console.WriteLine("temperature limit reached");
            if (i >= imax)
                Console.WriteLine("iteration limit reached");

        }

        public static bool ShouldChange(int delta, double temperature)
        {
            if (delta <= 0)
                return true;
            double kans = Math.Pow(Math.E, -delta / temperature);
            kans = kans * 1000;
            return random.Next(0, 1000) < kans;
        }

        public static Bezorger getRandomBezorgerNonEmpty
        {
            get
            {
                Bezorger[] temp = bezorgers.Where(item => item.route.Count > 0).ToArray();
                return temp[random.Next(temp.Count())];
            }
        }

        public static int CalculateQuality()
        {
            int totalQuality = 0;
            foreach (Bezorger B in bezorgers)
            {
                int elapsedtime = 0;
                Node prevClient = null;
                Node currClient = B.firstNode;
                while (currClient != null)
                {
                    elapsedtime += Help.dist(prevClient, currClient);
                    totalQuality += elapsedtime;
                    prevClient = currClient;
                    currClient = currClient.next;
                }
            }

            return totalQuality;
        }
    }
}
