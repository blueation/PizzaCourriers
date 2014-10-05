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

        #region Properties to get Random Nodes
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
        #endregion

        #region Functions to Add Nodes
        public void AddAfter(Node added, Node location) //if location is null, adds to start of route
        {
            if (location == null)
                AddNodetoStart(added);
            else
            {
                routeLength += costsAddAfter(added, location);
                route.Add(added);
                added.onRoute = this;
                added.previous = location;
                added.next = location.next;
                location.next = added;
                if (added.next != null)
                    added.next.previous = added;
            }
        }
        public int costsAddAfter(Node added, Node location)
        {
            if (location == null)
                return costsAddNodetoStart(added);
            return Help.dist(location.next, added) + Help.dist(added, location) - Help.dist(location.next, location);
        }

        public void AddBefore(Node added, Node location) //if location is null, adds to end of route
        {
            if (location == null)
                AddNodetoEnd(added);
            else
            {
                routeLength += costsAddBefore(added, location);
                route.Add(added);
                added.onRoute = this;
                added.next = location;
                added.previous = location.previous;
                location.previous = added;
                if (added.previous != null)
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
            added.onRoute = this;

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
        #endregion

        #region Functions to Remove Nodes
        public void RemoveNode(Node removed)
        {
            routeLength += costsRemoveNode(removed);
            if (removed.previous != null)
                removed.previous.next = removed.next;
            else
                removed.onRoute.firstNode = removed.next;
            if (removed.next != null)
                removed.next.previous = removed.previous;
            else
                removed.onRoute.lastNode = removed.previous;
            removed.previous = null;
            removed.next = null;
            removed.onRoute = null;
            route.Remove(removed);
        }
        public int costsRemoveNode(Node removed)
        {
            return Help.dist(removed.previous, removed.next) - Help.dist(removed.previous, removed) - Help.dist(removed, removed.next);
        }
        #endregion

        #region Functions with Other Effects on Nodes
        public void FlipOrder(Node first, Node last) //2-opt
        {
            routeLength += costsFlipOrder(first, last);

            //changes the cross-directional references between first, first.previous, last and last.next
            Node helper = first.previous;
            if (first.previous != null)
                first.previous.next = last;
            else 
                first.onRoute.firstNode = last;
            first.previous = last.next;
            if (last.next != null)
                last.next.previous = first;
            else
                last.onRoute.lastNode = first;
            last.next = helper;

            //flips direction of route between the first and last nodes
            Node IterationNode = first;
            Node EndOnNode = last.next;
            while (IterationNode != EndOnNode)
            {
                Node NextIterationNode = IterationNode.next;
                IterationNode.next = IterationNode.previous;
                IterationNode.previous = NextIterationNode;
                IterationNode = NextIterationNode;
            }
        }
        public int costsFlipOrder(Node first, Node last)
        {
            return Help.dist(first.previous, last) + Help.dist(first, last.next) - Help.dist(first.previous, first) - Help.dist(last, last.next);
        }
        #endregion

        public string StringSolution()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID + ": ");
            Node N = firstNode;
            while (N != null)
            {
                sb.Append("(" + N.x + ", " + N.y + ")");
                N = N.next;
            }
            sb.Append('\n');
            return sb.ToString();
        }

        public int GetLength()
        {
            Node n = firstNode;
            if (n == null)
                return 0;
            int tel = 0;
            while (n != null)
            {
                tel++;
                n = n.next;
            }
            return tel;
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
