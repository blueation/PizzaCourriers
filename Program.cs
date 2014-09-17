﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PizzaCourriers
{
    partial class Program
    {
        public static Random random = new Random(9987);

        public static List<Node> nodelist = new List<Node>();
        public static Bezorger[] bezorgers = new Bezorger[4];
        public static int resX, resY;

        static void Main(string[] args)
        {
            //initialize uninitialized data
            

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
            

        }


    }


}
