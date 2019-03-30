using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds information about all the waypoints, as well as initial setup.
public class WaypointManager : MonoBehaviour
{
    /*static*/ List<Waypoint> waypoints = new List<Waypoint>(); // Keeps track of all the waypoints

    // Start is called before the first frame update
    public void Init()
    {
        GameObject[] collectWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        for (int i = 0; i < collectWaypoints.Length; i++)
            waypoints.Add(collectWaypoints[i].GetComponent<Waypoint>());
    }

    // Use a Kd-Tree. To be implemented later
    public Waypoint FindClosestWaypoint(Transform currentPosition)
    {
        GameObject findGenerator = GameObject.FindGameObjectWithTag("Generator");

        if (findGenerator != null)
            return findGenerator.GetComponent<WaypointGenerator>().SearchTree(currentPosition);
        else
        {
            // Find the closest distance
            float distance;
            float closestDistance = Mathf.Infinity;
            Waypoint closestWaypoint = null;

            for (int i = 0; i < waypoints.Count; i++)
            {
                distance = Mathf.Pow(waypoints[i].transform.position.x - currentPosition.position.x, 2) + Mathf.Pow(waypoints[i].transform.position.y - currentPosition.position.y, 2) + Mathf.Pow(waypoints[i].transform.position.z - currentPosition.position.z, 2);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestWaypoint = waypoints[i];
                }
            }
            return closestWaypoint;
        }
    }


    // Pathfinding. Either function can be called and will return a list containing the path
    public List<Waypoint> FindPath(Waypoint start, Transform goal)
    {
        return FindPath(start, FindClosestWaypoint(goal));
    }
    // Making use of A*
    public List<Waypoint> FindPath(Waypoint start, Waypoint end)
    {
        Dictionary<int, Waypoint> closedList = new Dictionary<int, Waypoint>(); // Create a list containing nodes already seen (start is default)
        List<Waypoint> openList = new List<Waypoint>() { start };   // Create a list containing nodes seen but not assessed yet. Ordered as a priority queue

        List<Waypoint> path = new List<Waypoint>(); // List containing all the nodes in the shortest path

        List<Waypoint> currentLinks = new List<Waypoint>(); // Contains the neighbouring nodes to be assessed from the current node
        Waypoint currentWaypoint = start;

        // Init of the starting node
        currentWaypoint.SetHeuristic(Mathf.Pow(end.gameObject.transform.position.x, 2) + Mathf.Pow(end.gameObject.transform.position.y, 2) + Mathf.Pow(end.gameObject.transform.position.z, 2));
        currentWaypoint.AssignNodeValue(0);
        currentWaypoint.AssignChild(null);

        bool foundEnd = false;

        int counter = 0;

        while ((openList.Count > 0) && (!foundEnd))
        {
            currentWaypoint = RemoveHeap(openList);
            closedList[currentWaypoint.num] = currentWaypoint;
            currentLinks = currentWaypoint.GetConnectedNodes(); // Take all the nodes that the current node is connected to

            counter += 6;

            for(int i = 0; i < currentLinks.Count; i++)
            {
                counter += 2;
                // Make sure it's not already in the closed list. Ignore it otherwise
                if (!closedList.ContainsKey(currentLinks[i].num))//!closedList.Contains(currentLinks[i]))
                {
                 //   if (currentLinks[i].IsBlocked())
                   //     closedList.Add(currentLinks[i]);
                   // else
                   // {
                        // Using Euclidean distance as heuristic function since waypoints are set at diff angles
                        float heuristic = Mathf.Pow(end.gameObject.transform.position.x - currentLinks[i].gameObject.transform.position.x, 2) + Mathf.Pow(end.gameObject.transform.position.y - currentLinks[i].gameObject.transform.position.y, 2) + Mathf.Pow(end.gameObject.transform.position.z - currentLinks[i].transform.position.z, 2);
                        float gValue = Mathf.Pow(currentLinks[i].gameObject.transform.position.x - currentWaypoint.gameObject.transform.position.x, 2) + Mathf.Pow(currentLinks[i].gameObject.transform.position.y - currentWaypoint.gameObject.transform.position.y, 2) + Mathf.Pow(currentLinks[i].gameObject.transform.position.z - currentWaypoint.transform.position.z, 2) + currentWaypoint.GetGVal();
                    //heuristic = 0f; // Turn this into Dijkstra's
                    counter += 5;
                        // If this item has not been seen OR if it has, that it has a higher cost value
                        if ((!openList.Contains(currentLinks[i]) || (openList.Contains(currentLinks[i]) && (currentLinks[i].GetNodeValue() > gValue + heuristic))))
                        {
                            currentLinks[i].AssignChild(currentWaypoint);

                            // Assign the distance to each node
                            currentLinks[i].SetHeuristic(Mathf.Sqrt(heuristic));
                            currentLinks[i].AssignNodeValue(gValue);

                        counter += 3;
                            AddHeap(openList, currentLinks[i]);
                     //   }
                    }
                }
                counter++;
                // If the end is found, exit
                if (currentLinks[i] == end)
                {
                    currentWaypoint = end;
                    i = currentLinks.Count;
                    foundEnd = true;

                    counter += 3;
                }
            }
        }

        counter++;
        // If a path was found, otherwise the path list will return empty
        if(foundEnd)
        {
            while(currentWaypoint != null)
            {
                counter++;
                path.Add(currentWaypoint);
                currentWaypoint = currentWaypoint.GetChild();
            }
            path.Reverse(); // Or can implement as a stack

            counter += path.Count;
        }
        //print("Total calc: " + counter);

        return path;
    }

    // Create the priority queue
    void AddHeap(List<Waypoint> heap, Waypoint add)
    {
        Waypoint temp = null;
        heap.Add(add);

        int i = heap.Count - 1;

        while(i > 0)
        {
            if (heap[i].GetNodeValue() < heap[i / 2].GetNodeValue())
            {
                temp = heap[i];
                heap[i] = heap[i / 2];
                heap[i / 2] = temp;

                i = i / 2;
            }
            else
                i = 0;
        }
    }
    Waypoint RemoveHeap(List<Waypoint> heap)
    {
        Waypoint nextWaypoint = heap[0];
        Waypoint temp = null;
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        if(heap.Count > 0)
        {
            temp = heap[0];

            int i = 0;
            int nextInd = 0;
            while (i < heap.Count - 1)
            {
                nextInd = i;
                if ((i * 2 + 1 < heap.Count) && (heap[i].GetNodeValue() > heap[i * 2 + 1].GetNodeValue()))
                    nextInd = i * 2 + 1;
                if ((i * 2 + 2 < heap.Count) && (heap[i * 2 + 2].GetNodeValue() < heap[nextInd].GetNodeValue()))
                    nextInd = i * 2 + 2;

                if (nextInd == i)
                    i = heap.Count;
                else
                {
                    temp = heap[i];
                    heap[i] = heap[nextInd];
                    heap[nextInd] = temp;
                    i = nextInd;
                }
            }
        }
        return nextWaypoint;
    }


    // FOR TESTING PURPOSES ONLY (Ship.cs)
    public Waypoint GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Count)];
    }
}
