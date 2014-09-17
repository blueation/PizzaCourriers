using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaCourriers
{
    
    class Bezorger
    {
        public int ID;
        public int resX, resY;  //quicker reference point for routeLength calculations

        public List<Node> route = new List<Node>();
        public Node firstNode, lastNode;
        public int routeLength = 0;

        public Bezorger(int ID, int resX, int resY)
        {
            this.ID = ID; this.resX = resX; this.resY = resY;
        }

        public void AddtoStart(Node added)
        {
            if (firstNode == null)
            {   //no first node? no nodes at all.
                route.Add(added);
                firstNode = added;
                lastNode = added;
                routeLength += 2 * Help.dist(resX, resY, added.x, added.y);
            }
            else
            {
                routeLength -= Help.dist(resX, resY, firstNode.x, firstNode.y);
                route.Add(added);
                added.next = firstNode;
                firstNode.previous = added;
                firstNode = added;
                routeLength += Help.dist(resX, resY, added.x, added.y);
                routeLength += Help.dist(added.x, added.y, added.next.x, added.next.y);
            }
        }

        public void AddtoEnd(Node added)
        {
            if (lastNode == null)
            {   //no last node? no nodes at all.
                route.Add(added);
                firstNode = added;
                lastNode = added;
            }
            else
            {
                route.Add(added);
                added.previous = lastNode;
                lastNode.next = added;
                lastNode = added;
            }
        }
    }

    class Node
    {
        public int ID;
        public int x, y;
        public Node next, previous;

        public Node(int ID, int x, int y)
        {
            this.ID = ID; this.x = x; this.y = y;
        }
    }
}
