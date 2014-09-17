using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PizzaCourriers
{
    partial class Program
    {
        public static void InputFromFile(string filename)
        {
            using (TextReader sr = new StreamReader(filename))
            {
                int id = 0;
                char[] coorchar = new char[] { '(', ',', ')' };
                string line = sr.ReadLine();
                string[] splitline = line.Split(coorchar);
                resX = int.Parse(splitline[1]); resY = int.Parse(splitline[2]);
                while ((line = sr.ReadLine()) != null)
                {
                    splitline = line.Split(coorchar);
                    nodelist.Add(new Node(id++, int.Parse(splitline[1]), int.Parse(splitline[2])));
                }
            }
        }

        public static string StringSolution()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Bezorger b in bezorgers)
                sb.Append(b.StringSolution());
            return sb.ToString();
        }
    }
}
