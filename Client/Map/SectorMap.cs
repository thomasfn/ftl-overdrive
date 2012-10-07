using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTLOverdrive.Client.Gamestate;

namespace FTLOverdrive.Client.Map
{
    public class SectorMap
    {
        public enum SectorType { Civilian, Hostile, Nebula }

        public class Node
        {
            public SectorType Type { get; set; }
            public string Name { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public List<Node> NextNodes { get; set; }

            public Node(string name, int x, int y)
            {
                Name = name;
                X = x;
                Y = y;
                NextNodes = new List<Node>();
            }

            public Node(SectorType type, string name, int x, int y) : this (name, x, y)
            {
                Type = type;
            }

            public Node(string type, string name, int x, int y) : this(name, x, y)
            {
                Type = (SectorType)Enum.Parse(typeof(SectorType), type);
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

        public Node AddNode(SectorType type, string name, int x, int y)
        {
            Node n = new Node(type, name, x, y);
            Nodes.Add(n);
            return n;
        }

        public Node AddNode(string type, string name, int x, int y)
        {
            Node n = new Node(type, name, x, y);
            Nodes.Add(n);
            return n;
        }

        public SectorMap()
        {
            Nodes = new List<Node>();
        }
    }
}
