﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds information about all the waypoints, as well as initial setup.
public class WaypointManager : MonoBehaviour
{
    /*static*/ List<Waypoint> waypoints = new List<Waypoint>(); // Keeps track of all the waypoints
    int code;

    // Start is called before the first frame update
    public void Start()
    {
        code = Random.Range(0, 1000);
    }
    public void Init()
    {
        GameObject[] collectWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        for (int i = 0; i < collectWaypoints.Length; i++)
            waypoints.Add(collectWaypoints[i].GetComponent<Waypoint>());
    }

    public Waypoint FindClosestWaypoint(Transform currentPosition)
    {
        GameObject findGenerator = GameObject.FindGameObjectWithTag("Generator");

      //  if (findGenerator != null)
     //       return findGenerator.GetComponent<WaypointGenerator>().SearchTree(currentPosition);
     //   else
    //    {
            // Find the closest distance
            float distance;
            float closestDistance = Mathf.Infinity;
            Waypoint closestWaypoint = null;

            for (int i = 0; i < waypoints.Count; i++)
            {
              if (waypoints[i].target == -1 || (waypoints[i].target == code))
              {
                  distance = Mathf.Pow(waypoints[i].transform.position.x - currentPosition.position.x, 2) + Mathf.Pow(waypoints[i].transform.position.y - currentPosition.position.y, 2) + Mathf.Pow(waypoints[i].transform.position.z - currentPosition.position.z, 2);
                  if (distance < closestDistance)
                  {
                      closestDistance = distance;
                      closestWaypoint = waypoints[i];
                  }
              }
            }
            return closestWaypoint;
    //    }
    }

    float CalcDistance(Vector3 start, Vector3 end)
    {
        float dx = Mathf.Abs(start.x - end.x);
        float dy = Mathf.Abs(start.y - end.y);
        float dz = Mathf.Abs(start.z - end.z);
        float calc = dx + dy + dz;

        if (dx == 0)
            return calc + (Mathf.Sqrt(2) - 2) * Mathf.Min(dy, dz);
        else if (dy == 0)
            return calc + (Mathf.Sqrt(2) - 2) * Mathf.Min(dx, dz);
        else if (dz == 0)
            return calc + (Mathf.Sqrt(2) - 2) * Mathf.Min(dx, dy);
        else
            return calc + (Mathf.Sqrt(3) - 3) * Mathf.Min(dx, dy, dz);
    }

    // Pathfinding. Either function can be called and will return a list containing the path
    public List<Waypoint> FindPath(Waypoint start, Transform goal)
    {
        return FindPath(start, FindClosestWaypoint(goal));
    }
    // Making use of A*
    public List<Waypoint> FindPath(Waypoint start, Waypoint end)
    {
        Dictionary<int, Waypoint> closedList = new Dictionary<int, Waypoint>(); // Create a dict containing nodes already seen (start is default)
        List<Waypoint> openList = new List<Waypoint>() { start };   // Create a list containing nodes seen but not assessed yet. Ordered as a priority queue

        List<Waypoint> path = new List<Waypoint>(); // List containing all the nodes in the shortest path

        List<Waypoint> currentLinks = new List<Waypoint>(); // Contains the neighbouring nodes to be assessed from the current node
        Waypoint currentWaypoint = start;

        // Init of the starting node
        //currentWaypoint.SetHeuristic(Mathf.Pow(end.gameObject.transform.position.x, 2) + Mathf.Pow(end.gameObject.transform.position.y, 2) + Mathf.Pow(end.gameObject.transform.position.z, 2));
        currentWaypoint.SetHeuristic(Mathf.Abs(currentWaypoint.gameObject.transform.position.x - end.gameObject.transform.position.x) + Mathf.Abs(currentWaypoint.gameObject.transform.position.y - end.gameObject.transform.position.y) + Mathf.Abs(currentWaypoint.gameObject.transform.position.z - end.gameObject.transform.position.z));
        currentWaypoint.AssignNodeValue(0);
        currentWaypoint.AssignChild(null);

        bool foundEnd = false;

        while ((openList.Count > 0) && (!foundEnd))
        {
            currentWaypoint = RemoveHeap(openList);
            closedList[currentWaypoint.num] = currentWaypoint;
            currentLinks = currentWaypoint.GetConnectedNodes(); // Take all the nodes that the current node is connected to

            for(int i = 0; i < currentLinks.Count; i++)
            {
                // Make sure it's not already in the closed list. Ignore it otherwise
                if (!closedList.ContainsKey(currentLinks[i].num))
                {
                    //               if (currentLinks[i].IsBlocked())
                    //                closedList.Add(i, currentLinks[i]);
                    //          else
                    //       {
                    float heuristic = Mathf.Pow(end.gameObject.transform.position.x - currentLinks[i].gameObject.transform.position.x, 2) + Mathf.Pow(end.gameObject.transform.position.y - currentLinks[i].gameObject.transform.position.y, 2) + Mathf.Pow(end.gameObject.transform.position.z - currentLinks[i].transform.position.z, 2);
                    //float heuristic = Mathf.Abs(end.gameObject.transform.position.x - currentLinks[i].gameObject.transform.position.x) + Mathf.Abs(end.gameObject.transform.position.y - currentLinks[i].gameObject.transform.position.y) + Mathf.Abs(end.gameObject.transform.position.z - currentLinks[i].transform.position.z);
                    //float heuristic = CalcDistance(end.gameObject.transform.position, currentLinks[i].gameObject.transform.position);
                        float gValue = Mathf.Pow(currentLinks[i].gameObject.transform.position.x - currentWaypoint.gameObject.transform.position.x, 2) + Mathf.Pow(currentLinks[i].gameObject.transform.position.y - currentWaypoint.gameObject.transform.position.y, 2) + Mathf.Pow(currentLinks[i].gameObject.transform.position.z - currentWaypoint.transform.position.z, 2) + currentWaypoint.GetGVal();
                        //float gValue = Mathf.Abs(currentLinks[i].gameObject.transform.position.x - currentWaypoint.gameObject.transform.position.x) + Mathf.Abs(currentLinks[i].gameObject.transform.position.y - currentWaypoint.gameObject.transform.position.y) + Mathf.Abs(currentLinks[i].gameObject.transform.position.z - currentWaypoint.transform.position.z) + currentWaypoint.GetGVal();
                        //heuristic = 0f; // Turn this into Dijkstra's
                        // If this item has not been seen OR if it has, that it has a higher cost value
                        if ((!openList.Contains(currentLinks[i]) || (openList.Contains(currentLinks[i]) && (currentLinks[i].GetNodeValue() > gValue + heuristic))))
                        {
                            currentLinks[i].AssignChild(currentWaypoint);

                            // Assign the distance to each node
                            currentLinks[i].SetHeuristic(heuristic);//Mathf.Sqrt(heuristic));
                            currentLinks[i].AssignNodeValue(gValue);

                            AddHeap(openList, currentLinks[i]);
            //            }
                    }
                }
                // If the end is found, exit
                if (currentLinks[i] == end)
                {
                    currentWaypoint = end;
                    end.target = code;
                    i = currentLinks.Count;
                    foundEnd = true;
                }
            }
        }
        // If a path was found, otherwise the path list will return empty
        if(foundEnd)
        {
            while(currentWaypoint != null)
            {
                path.Add(currentWaypoint);
                currentWaypoint = currentWaypoint.GetChild();
            }
            path.Reverse(); // Or can implement as a stack
        }
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
