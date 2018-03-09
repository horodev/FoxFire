using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxFire
{
    internal class GraphNode<T>
    {
        public GraphNode(T value)
        {
            Value = value;
            Neighbors = new List<GraphNode<T>>();
        }

        public List<GraphNode<T>> Neighbors { get; }
        public T Value { get; set; }
    }

    public class Graph<T>
    {
        private List<GraphNode<T>> Nodes;

        public Graph()
        {
            Nodes = new List<GraphNode<T>>();
        }

        public void AddNode(T value)
        {
            GraphNode<T> n = new GraphNode<T>(value);
            if (Nodes.Contains(n))
                return;

            Nodes.Add(n);
        }

        public void AddEdge(T origin, T neighbor)
        {
            GraphNode<T> originNode = Nodes.First(n => n.Value.Equals(origin));
            GraphNode<T> neighborNode = Nodes.First(n => n.Value.Equals(neighbor));

            if (originNode == null || neighborNode == null)
                return;

            originNode.Neighbors.Add(neighborNode);
            neighborNode.Neighbors.Add(originNode);
        }
    }
}
