using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTLOverdrive.Client.Gamestate;

namespace FTLOverdrive.Client.Map
{
    public class SectorMap
    {
        public class Node
        {
            public Sector Sector { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public List<Node> NextNodes { get; set; }

            public Node(int x, int y, Sector sector)
            {
                X = x;
                Y = y;
                Sector = sector;
                NextNodes = new List<Node>();
            }
        }

        private Node currentNode;
        public Node CurrentNode {
            get { return (currentNode == null) ? Nodes[0] : currentNode; }
            set { currentNode = value; }
        }

        public List<Node> Nodes { get; set; }

        public Node AddNode(Node n)
        {
            Nodes.Add(n);
            return n;
        }

        public Node AddNode(int x, int y, Sector sector)
        {
            Node n = new Node(x, y, sector);
            Nodes.Add(n);
            return n;
        }

        public SectorMap()
        {
            Nodes = new List<Node>();
        }
    }
}
