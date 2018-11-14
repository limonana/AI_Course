using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.Observers;
using AIBasic.Algorithms;
using QuickGraph.Algorithms.ShortestPath;

namespace World
{    
    public class TravelGraph : UndirectedGraph<int, TravelEdge>, ICloneable
    {
        Dictionary<TravelEdge, double> _Costs = new Dictionary<TravelEdge, double>();

        public double GetCost(TravelEdge edge)
        {
            return _Costs[edge];
        }

        protected override void OnEdgeRemoved(TravelEdge args)
        {
            base.OnEdgeRemoved(args);
            _Costs.Remove(args);            
        }

        public void AddEdge(TravelEdge edge,double cost)
        {
            AddEdge(edge);
            _Costs[edge] = cost;
        }

        

        public IEnumerable<TravelPath> findChepestPaths(int startLocation, params int[] targets)
        {
            Func<TravelEdge, double> distanceFunc = new Func<TravelEdge, double>(edge => _Costs[edge]);
            var resFunc = this.ShortestPathsDijkstra(distanceFunc, startLocation);

            var paths = targets.Select(node =>
            {
                IEnumerable<TravelEdge> path;
                resFunc.Invoke(node, out path);
                return path;
            }
            ).Where(path=>path!=null);

            if (paths.Count() == 0)
                return new List<TravelPath>();

            
            var minValue = paths.Min(path => path.Sum(edge => _Costs[edge]));
            var minPaths = paths.Where(path => path.Sum(edge => _Costs[edge]) == minValue);
            return minPaths.Select(path => ConvertToPath(path,startLocation));            
        }

        private  TravelPath ConvertToPath(IEnumerable<TravelEdge> path,int start)
        {
            var enumerator = path.GetEnumerator();
            enumerator.MoveNext();
            int current = start;
            TravelPath p = new TravelPath();
            int prev;
            do
            {
                prev = current;
                current = enumerator.Current.getAnotherPlace(current);                                
                p.Add(current,GetCost(current,prev));
            }
            while (enumerator.MoveNext());
            return p;
        }

        public  double GetCost(int p1, int p2)
        {
            return GetCost(getWay(p1, p2));
        }

        public object Clone()
        {
            var newGraph = new TravelGraph();
            newGraph.AddVertexRange(this.Vertices);
            newGraph.AddEdgeRange(this.Edges);
            newGraph._Costs = new Dictionary<TravelEdge,double>(_Costs,_Costs.Comparer);
            return newGraph;
        }

        public TravelPath ShortestPath(int sourcePlace,int targetPlace)
        {
            var algo = new UndirectedBreadthFirstSearchAlgorithm<int, TravelEdge>(this);
            algo.SetRootVertex(sourcePlace);
            var predecessors = new UndirectedVertexPredecessorRecorderObserver<int, TravelEdge>();
            predecessors.Attach(algo);

            algo.Compute();

            List<TravelEdge> path = new List<TravelEdge>();
            var place = targetPlace;
            //there is no way to get to the target
            if (!predecessors.VertexPredecessors.ContainsKey(place))
                return null;
            
            while (place != sourcePlace)
            {
                var edge =  predecessors.VertexPredecessors[place];
                path.Insert(0,edge);
                place = edge.getAnotherPlace(place);
            }            
            return ConvertToPath(path,sourcePlace);
        }

        public TravelEdge getWay(int fromPlace, int toPlace)
        {
            return AdjacentEdges(fromPlace).
                Single(edge => edge.getAnotherPlace(fromPlace) == toPlace);
        }

        FloydWarshallAllShortestPathAlgorithm<int, TravelEdge> _fwAlgo = null;
        public void CalcCheapestPaths()
        {                        
            Func<TravelEdge, double> distanceFunc = new Func<TravelEdge, double>(edge => GetCost(edge.Source,edge.Target));            
            _fwAlgo = new  FloydWarshallAllShortestPathAlgorithm<int, TravelEdge>(this.ToDirectedGrpah(), distanceFunc);
            _fwAlgo.Compute();                     
        }

        public TravelPath GetPath(int start, int dest)
        {
            IEnumerable<TravelEdge> path = new List<TravelEdge>();
            if (_fwAlgo.TryGetPath(start, dest, out path))
                return ConvertToPath(path, start);
            else
                return null;
        }
    }

    static class GraphExtension
    {
        public static IVertexAndEdgeListGraph<int,TravelEdge> ToDirectedGrpah(this UndirectedGraph<int,TravelEdge> g)
        {
            var directed = new QuickGraph.AdjacencyGraph<int, TravelEdge>();
            directed.AddVerticesAndEdgeRange(g.Edges);
            foreach (var edge in g.Edges)
            {
                directed.AddEdge(new TravelEdge(edge.Target, edge.Source));
            }
            return directed;
        }
    }
}
