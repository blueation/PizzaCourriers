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

        public Node getRandomNodeNull //call this in case you are working between values 
        {
            get
            {
                int r = Program.random.Next(route.Count + 1);
                if (route.Count == r)
                    return null;
                return route[r];
            }
        }

        public void AddBefore(Node added, Node location) //if location is null, adds to end of route
        {
            if (location == null)
                AddNodetoEnd(added);
            else
            {
                route.Add(added);
                added.next = location;
                added.previous = location.previous;
                location.previous = added;
                added.previous.next = added;
                {
                    routeLength -= Help.dist(added.previous, location);
                    routeLength += Help.dist(added.previous, added);
                    routeLength += Help.dist(added, location);
                }
            }
        }

        //public int costsAddBefore(Node added, Node location)
        //{
            //if (location == null)
                
            
        //}

        public void AddNodetoStart(Node added)
        {
            //update costs
            routeLength += costsAddNodetoStart(added);
            route.Add(added);
            added.onRoute = this;

            if (firstNode == null)
            {   //no first node? no nodes at all
                firstNode = added;
                lastNode = added;
            }
            else
            {
                added.next = firstNode;
                firstNode.previous = added;
                firstNode = added;
            }
        }
        public int costsAddNodetoStart(Node added)
        {
            if (firstNode == null)
                return 2 * Help.dist(added, null);
            else
            {
                return Help.dist(added, null) + Help.dist(added, firstNode) - Help.dist(firstNode, null);
            }
        }

        public void AddNodetoEnd(Node added)
        {
            routeLength += costsAddNodetoEnd(added);
            route.Add(added);

            if (lastNode == null)
            {   //no last node? no nodes at all.
                firstNode = added;
                lastNode = added;
            }
            else
            {
                added.previous = lastNode;
                lastNode.next = added;
                lastNode = added;
            }
        }
        public int costsAddNodetoEnd(Node added)
        {
            if (lastNode == null)
                return 2 * Help.dist(added, null);
            else
            {
                return Help.dist(added, null) + Help.dist(added, lastNode) - Help.dist(lastNode, null);
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
