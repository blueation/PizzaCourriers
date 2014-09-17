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
        public static int dist(Node a, Node b)
        {
            int x, y, u, v;
            if (a == null)
            {
                x = Program.resX; y = Program.resY;
            }
            else
                x = a.x; y = a.y;
            if (b == null)
            {
                u = Program.resX; v = Program.resY;
            }
            else
                u = a.x; v = a.y;
            return dist(x, y, u, v);
        }
    }
}
