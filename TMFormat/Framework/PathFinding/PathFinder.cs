using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMFormat.Enums;
using TMFormat.Framework.Maps;

namespace TMFormat.Framework.PathFinding
{
    public class PathFinder
    {
        SearchNode[,] searchNodes;
        int levelWidth;
        int levelHeight;
        int levelX;
        int levelY;
        int floor;

        List<SearchNode> openList = new List<SearchNode>();
        List<SearchNode> closedList = new List<SearchNode>();
        MapManager map;

        public PathFinder(MapManager map)
        {
            this.map = map;
        }
        float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }

        bool isTileWalk(int x, int y, int z)
        {
            var tile = map.MapBase.Floors[z][x, y];


            if (tile.isCreature)
            {
                return false;
            }

            if (tile.item == null)
            {
                return false;
            }

            if (tile.item.Id <= 0)
            {
                return false;
            }

            if (tile.item.Block)
            {
                return false;
            }

            if (tile.items != null)
            {
                var _item = tile.items.FirstOrDefault();

                if (_item != null)
                {
                    if (_item.Block || _item.Type == (int)TypeItem.Stair && !_item.Use || _item.Type == (int)TypeItem.Field)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        void InitializeSearchNodes(Point startPoint, Point endPoint)
        {
            searchNodes = new SearchNode[levelWidth, levelHeight];

            //For each of the tiles in our map, we
            // will create a search node for it.
            for (int x = levelX; x < levelWidth; x++)
            {
                for (int y = levelY; y < levelHeight; y++)
                {
                    //Create a search node to represent this tile.
                    SearchNode node = new SearchNode();
                    node.Position = new Point(x, y);

                    // Our enemies can only walk on grass tiles.
                    node.Walkable = isTileWalk(x, y, floor);

                    if (x == startPoint.X && y == startPoint.Y)
                    {
                        node.Walkable = true;
                    }

                    if (x == endPoint.X && y == endPoint.Y)
                    {
                        node.Walkable = true;
                    }

                    // We only want to store nodes
                    // that can be walked on.
                    if (node.Walkable)
                    {
                        node.Neighbors = new SearchNode[4];
                        searchNodes[x, y] = node;
                    }
                }
            }

            // Now for each of the search nodes, we will
            // connect it to each of its neighbours.
            for (int x = levelX; x < levelWidth; x++)
            {
                for (int y = levelY; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    // We only want to look at the nodes that 
                    // our enemies can walk on.
                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    // An array of all of the possible neighbors this 
                    // node could have. (We will ignore diagonals for now.)
                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1), // The node above the current node
                        new Point (x, y + 1), // The node below the current node.
                        new Point (x - 1, y), // The node left of the current node.
                        new Point (x + 1, y), // The node right of the current node
                    };

                    // We loop through each of the possible neighbors
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];

                        // We need to make sure this neighbour is part of the level.
                        if (position.X < 0 || position.X > levelWidth - 1 || position.Y < 0 || position.Y > levelHeight - 1)
                        {
                            continue;
                        }

                        SearchNode neighbor = searchNodes[position.X, position.Y];

                        // We will only bother keeping a reference 
                        // to the nodes that can be walked on.
                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }

                        // Store a reference to the neighbor.
                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }

        void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = levelX; x < levelWidth; x++)
            {
                for (int y = levelY; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        public List<Vector2> FindPath(Point startPoint, Point endPoint, int floor)
        {
            levelX = startPoint.X - 12;
            levelY = startPoint.Y - 9;
            this.floor = floor;

            if (levelX < 0)
            {
                levelX = 0;
            }

            if (levelY < 0)
            {
                levelY = 0;
            }

            levelWidth = startPoint.X + 12;
            levelHeight = startPoint.Y + 9;

            InitializeSearchNodes(startPoint, endPoint);

            if (endPoint.X > levelWidth || endPoint.Y > levelHeight)
            {
                return new List<Vector2>();
            }

            // Only try to find a path if the start and end points are different.
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            if (startPoint.X >= levelWidth || startPoint.Y >= levelHeight)
            {
                return new List<Vector2>();
            }

            SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];

            if (endPoint.X >= levelWidth || endPoint.Y >= levelHeight)
            {
                return new List<Vector2>();
            }

            SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];

            if (startNode == null || endNode == null)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Setp 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                SearchNode currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                {
                    break;
                }

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    SearchNode neighbor = currentNode.Neighbors[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    // An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbor.Position, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value we just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + the estimated 
                        //     distance between the neighbouring node and goal node.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active Node.
                        neighbor.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }

        List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path.
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].Position.X, closedList[i].Position.Y));
            }

            return finalPath;
        }

        SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal.
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }
    }

}
