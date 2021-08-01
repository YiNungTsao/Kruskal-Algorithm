using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _0853337_曹議濃_HW6
{
    class Arc
    {
        public double tail { get; set; }
        public double head { get; set; }
        public double cost { get; set; }
    }

    class Forest
    {
        public List<double> tree = new List<double>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            // read graph
            string line = string.Empty;
            double[] arr_line;
            Arc arc = new Arc();
            List<Arc> graph = new List<Arc>();
            System.IO.StreamReader sr = new System.IO.StreamReader("input.csv");

            while ((line = sr.ReadLine()) != null)
            {
                try
                {
                    arr_line = Array.ConvertAll(line.Split(','), double.Parse);
                    arc.tail = arr_line[0];
                    arc.head = arr_line[1];
                    arc.cost = arr_line[2];

                    graph.Add(arc);
                    arc = new Arc();
                }
                catch (System.Exception e) { continue; }
            }

            // sort by cost
            graph = graph.OrderBy(value => value.cost).ToList();
            double max_node = Math.Max(graph.Max(value => value.tail), graph.Max(value => value.head));

            // initial forest
            Forest tree = new Forest();
            List<Forest> forest = new List<Forest>();
            for (int i = 1; i <= max_node; i++)
            {
                tree.tree.Add(i);
                forest.Add(tree);
                tree = new Forest();
            }

            // main procedure of Kruskal algorithm
            int iterations = 0;
            double total_cost = 0;
            List<Arc> results = new List<Arc>();
            while (forest.Count != 1)
            { 
                

                // pick the least cost from graph
                arc = graph.Find(value1 => value1.cost == graph.Min(value2 => value2.cost));

                // check forest
                int tail_id = -1, head_id = -1;
                for (int i = 0; i < forest.Count; i++)
                {
                    int temp1 = forest[i].tree.FindIndex(value2 => value2 == arc.tail);
                    int temp2 = forest[i].tree.FindIndex(value2 => value2 == arc.head);
                    if (temp1 != -1) tail_id = i;
                    if (temp2 != -1) head_id = i;
                }

                if (tail_id != -1 && head_id != -1 && tail_id != head_id) 
                {
                    iterations++;

                    // update forest
                    for (int i = 0; i < forest[tail_id].tree.Count; i++)
                        forest[head_id].tree.Add(forest[tail_id].tree[i]);
                    forest.RemoveAt(tail_id);

                    // update results
                    results.Add(arc);
                    total_cost += arc.cost;

                    // show current iteration information
                    Console.WriteLine("==============================================================");
                    Console.WriteLine("iteration {0}", iterations);
                    Console.WriteLine("selected arc: tail = {0}, head = {1}, cost = {2}", arc.tail, arc.head, arc.cost);
                    Console.WriteLine("Forest");
                    for (int i = 0; i < forest.Count; i++)
                    {
                        Console.Write("No.{0} tree = ", i);
                        for (int j = 0; j < forest[i].tree.Count; j++) Console.Write("{0}, ", forest[i].tree[j]);
                        Console.WriteLine();
                    }
                }
                else // remove arc from the graph
                    graph.Remove(arc);
            }

            // final information
            Console.WriteLine("==============================================================");
            Console.WriteLine("Final result, total cost = {0}", total_cost);
            results.ForEach(each_arc => 
            {
                Console.WriteLine("arc({0},{1}) = {2}", each_arc.tail, each_arc.head, each_arc.cost);
            });
            Console.WriteLine("\nfinish");
            Console.ReadLine();
        }
    }
}
