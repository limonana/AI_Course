using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.Observers;

namespace World
{    
    public class TravelWorld:ICloneable
    {
        TravelGraph _graph = new TravelGraph();

        HashSet<int> _waterPlaces = new HashSet<int>();        
        HashSet<TravelEdge> _fireWays = new HashSet<TravelEdge>();        

        public IEnumerable<int> GetWaterPlaces() { return _waterPlaces; }
        public IEnumerable<TravelEdge> GetFireWays() { return _fireWays; }

        public TravelWorld()
        {
        }

        public TravelWorld(TravelGraph travelGraph)
        {            
            _graph = travelGraph;
        }

        public TravelWorld(TravelWorld travelWorld)
        {
            this._waterPlaces = new HashSet<int>(travelWorld._waterPlaces);
            this._fireWays = new HashSet<TravelEdge>(travelWorld._fireWays);
            this._graph = travelWorld._graph;            
        }

        public void StopFire(int fromPlace, int toPlace)
        {
            _fireWays.Remove(_graph.getWay(fromPlace, toPlace));
        }

        public double getCostWay(int fromPlace, int toPlace)
        {            
            return _graph.GetCost(fromPlace,toPlace);          
        }       

        public bool isClear(int place, int adjacentPlace)
        {
            var way = _graph.getWay(place, adjacentPlace);
            return isClear(way);
        }

        public bool HaveWater(int place)
        {
            return _waterPlaces.Contains(place);
        }

        public bool TakeWater(int place)
        {
            bool haveWater = _waterPlaces.Contains(place);

            if (haveWater)
            {
                _waterPlaces.Remove(place);
                return true;
            }
            else
                return false;
        }

        public double PickupCost { get; set; }

        public double EpsilonCost { get { return Double.Epsilon; } }

        public void SetFire(int fromPlace, int toPlace)
        {
            _fireWays.Add(_graph.getWay(fromPlace, toPlace));
        }

        public IEnumerable<TravelPath> findCheapestWaterPaths(int startLocation)
        {
            var g2 = createWorldWithClearWays();
            return g2.findChepestPaths(startLocation, _waterPlaces.ToArray());
        }

        private TravelGraph createWorldWithClearWays()
        {
            var newWorld = (TravelGraph)(_graph.Clone());
            newWorld.RemoveEdgeIf(edge => _fireWays.Contains(edge));
            return newWorld;
        }



        public IEnumerable<TravelPath> findCheapestFirePaths(int startLocation)
        {
            //reduction: want to find chepest path that include  edge with fire
            // insert node "inside" the fire edges and calculate the distance to it
            //in order to get the right the path insert "opposite" edge inside the fire edge
            //with cost infinity.            
            //edges between real node and virtual node will be with original cost                        
            TravelGraph reductionGraph = createWorldWithClearWays();
            List<int> fireNodes = new List<int>();
            int generatePlace = reductionGraph.Vertices.Max();
            Dictionary<int, int> virtualToRealPlace = new Dictionary<int, int>();
            foreach (var edge in _fireWays)
            {
                var newNodeTarget = ++generatePlace;
                reductionGraph.AddVertex(newNodeTarget);
                
                var newNodeSource = ++generatePlace;
                reductionGraph.AddVertex(newNodeSource);                

                reductionGraph.AddEdge(new TravelEdge(edge.Source, newNodeTarget), _graph.GetCost(edge));
                reductionGraph.AddEdge(new TravelEdge(newNodeSource, edge.Target), _graph.GetCost(edge));

                //opposite edge
                virtualToRealPlace.Add(newNodeTarget, edge.Target);
                virtualToRealPlace.Add(newNodeSource, edge.Source);
                reductionGraph.AddEdge(new TravelEdge(newNodeSource, newNodeTarget), double.MaxValue);

                fireNodes.Add(newNodeSource);
                fireNodes.Add(newNodeTarget);
            }

            var paths =  reductionGraph.findChepestPaths(startLocation, fireNodes.ToArray()).ToArray();
            foreach (var path in paths)
            {
                foreach (var virt in virtualToRealPlace.Keys)
	            {                    
                    path.Replace(virt, virtualToRealPlace[virt]);
	            }
                          
            }
            return paths;
        }

        public IEnumerable<TravelEdge> GetClearWays(int startLocation)
        {
            return GetWays(startLocation).
                Where(edge => isClear(edge));
        }

        public IEnumerable<TravelEdge> GetWays(int place)
        {
            return _graph.AdjacentEdges(place);
        }

        public bool isClear(TravelEdge way)
        {
            return !_fireWays.Contains(way);
        }


        public TravelPath ShortestClearPath(int sourcePlace, int targetPlace)
        {
            var g = createWorldWithClearWays();
            return g.ShortestPath(sourcePlace,targetPlace);            
        }

        public bool isPathClear(TravelPath path)
        {            
            var enumartor = path.GetEnumerator();
            enumartor.MoveNext();
            int prev =enumartor.Current;
            while(enumartor.MoveNext())
            {                
                if (!isClear(prev,enumartor.Current))
                    return false;
                prev = enumartor.Current;
            }

            return true;
        }

        public void AddPlaces(params int[] places)
        {
            foreach (var place in places)
            {
                AddPlace(place);
            }
        }

        public void AddPlace(int place)
        {
            _graph.AddVertex(place);
        }

        public void AddWay(int p1, int p2, double cost, bool clear)
        {
            var edge = new TravelEdge(p1, p2);
            _graph.AddEdge(edge, cost);
            if (!clear)
                _fireWays.Add(edge);
        }

        public void PutWater(params int[] places)
        {
            foreach (var p in places)
            {
                _waterPlaces.Add(p);
            }

        }           

        public void AddWay(int p1, int p2, double cost)
        {
            _graph.AddEdge(new TravelEdge(p1, p2), cost);
        }

        public double getCostWay(TravelEdge way)
        {
            return _graph.GetCost(way);  
        }

        public TravelGraph GetGraph()
        {
            return _graph;
        }

        public TravelPath findCheapestCleartPath(int startLocation, int goal)
        {
            var g2 = createWorldWithClearWays();
            var res = g2.findChepestPaths(startLocation, goal);
            if (res.Count() == 1)
                return res.First();
            else
                return null;
        }

        public TravelPath findCheapestPath(int startLocation, int goal)
        {
            var res = _graph.findChepestPaths(startLocation, goal);
            if (res.Count() == 1)
                return res.First();
            else
                return null;
        }

        TravelGraph _clearGraph = null;        
        public void CalcCheapestCleanPaths()
        {
            _clearGraph = createWorldWithClearWays();
            _clearGraph.CalcCheapestPaths();
        }

        public TravelPath getClearPath(int start, int dest)
        {
            if (start == dest)
                return new TravelPath();
            
            return _clearGraph.GetPath(start, dest);
        }

        public TravelPath getClearPath(int start, TravelEdge fire)
        {
            var path = getClearPath(start, fire.Source);
            int beforeLst = path.ElementAt(path.Count - 2);
            if (beforeLst == fire.Target)
                return path;
            else
                return getClearPath(start, fire.Target);
        }

        public object Clone()
        {
            TravelWorld newWorld = new TravelWorld(this);
            return newWorld;
        }
    }


}