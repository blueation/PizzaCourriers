using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaCourriers
{
    
    class Bezorger
    {
        public int ID;
        
        public List<Node> route = new List<Node>();
        public Node firstNode, lastNode;
        public int routeLength = 0;

        public Bezorger(int ID)
        {
            this.ID = ID;
        }

        public Node getRandomNode
        {
            get
            {
                if (route.Count == 0) return null;
                return route[Program.random.Next(route.Count)];
            }
        }

        public void AddNodetoStart(Node added)
        {
            if (firstNode == null)
            {   //no first node? no nodes at all.
                route.Add(added);
                added.onRoute = this;
                firstNode = added;
                lastNode = added;
                routeLength += 2 * Help.dist(added, null);
            }
            else
            {
                routeLength -= Help.dist(firstNode, null);
                route.Add(added);
                added.onRoute = this;
                added.next = firstNode;
                firstNode.previous = added;
                firstNode = added;
                routeLength += Help.dist(added, null);
                routeLength += Help.dist(added, added.next);
            }
        }

        public void AddNodetoEnd(Node added)
        {
            if (lastNode == null)
            {   //no last node? no nodes at all.
                route.Add(added);
                firstNode = added;
                lastNode = added;
                routeLength += 2 * Help.dist(added, null);
            }
            else
            {
                routeLength -= Help.dist(lastNode, null);
                route.Add(added);
                added.previous = lastNode;
                lastNode.next = added;
                lastNode = added;
                routeLength += Help.dist(added, null);
                routeLength += Help.dist(added, added.previous);
            }
        }

        public string StringSolution()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID + ": ");
            Node N = firstNode;
            sb.Append("({0},{1})", N.x, N.y);
            N = N.next;
            while (N != null)
            {
                sb.Append(", ({0},{1})", N.x, N.y);
                N = N.next;
            }
            sb.Append('\n');
            return sb.ToString();
        }
    }

    class Node
    {
        public int ID;
        public int x, y;
        public Node next, previous;
        public Bezorger onRoute;

        public Node(int ID, int x, int y)
        {
            this.ID = ID; this.x = x; this.y = y;
        }
    }
}
