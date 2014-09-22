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

        public Node getRandomNode //call this if you are working on specific nodes (like deleting nodes)
        {
            get
            {
                if (route.Count == 0) return null;
                return route[Program.random.Next(route.Count)];
            }
        }

        public Node getRandomNodeOrNull //call this in case you are working between the nodes (like adding new ones)
        {
            get
            {
                int r = Program.random.Next(route.Count + 1);
                if (route.Count == r)
                    return null;
                return route[r];
            }
        }

        //KAN IEMAND DEZE FUNCTIES EVEN CONTROLEREN?
        //***
        public void AddAfter(Node added, Node location) //if location is null, adds to start of route
        {
            if (location == null)
                AddNodetoStart(added);
            else
            {
                routeLength += costsAddAfter(added, location);
                route.Add(added);
                added.previous = location;
                added.next = location.next;
                location.next = added;
                added.next.previous = added;
            }
        }
        public int costsAddAfter(Node added, Node location)
        {
            if (location == null)
                return costsAddNodetoStart(added);
            return Help.dist(added.next, added) + Help.dist(added, location) - Help.dist(added.next, location);
        }

        public void AddBefore(Node added, Node location) //if location is null, adds to end of route
        {
            if (location == null)
                AddNodetoEnd(added);
            else
            {
                routeLength += costsAddBefore(added, location);
                route.Add(added);
                added.next = location;
                added.previous = location.previous;
                location.previous = added;
                added.previous.next = added;
            }
        }
        public int costsAddBefore(Node added, Node location)
        {
            if (location == null)
                return costsAddNodetoEnd(added);
            return Help.dist(added.previous, added) + Help.dist(added, location) - Help.dist(added.previous, location);
        }

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
            return Help.dist(added, null) + Help.dist(added, lastNode) - Help.dist(lastNode, null);
        }
        //***

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
