using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaCourriers
{
    class Help
    {
        public static int dist(int x, int y, int u, int v)
        {
            return Math.Abs(x - u) + Math.Abs(y - v);
        }
    }
}
