using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    /*static*/
    List<Waypoint> waypoints = new List<Waypoint>(); // Keeps track of all the waypoints

    public void Init()
    {
        GameObject[] collectWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        for (int i = 0; i < collectWaypoints.Length; i++)
            waypoints.Add(collectWaypoints[i].GetComponent<Waypoint>());
    }

    public Waypoint FindClosestWaypoint(Transform currentPosition)
    {
        GameObject findGenerator = GameObject.FindGameObjectWithTag("Generator");

        float distance;
        float closestDistance = Mathf.Infinity;
        Waypoint closestWaypoint = null;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i].target == -1)
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
    public List<Waypoint> FindPath(Waypoint start, Transform goal, List<Waypoint> cachedPath)
    {
        return FindPath(start, FindClosestWaypoint(goal), cachedPath);
    }

    // D*Lite
    public List<Waypoint> FindPath(Waypoint start, Waypoint end, List<Waypoint> cachedPath)
    {
        // Store the old path in a dict to be referenced while creating a new path.
        Dictionary<Waypoint, int> cache = new Dictionary<Waypoint, int>();
        for (int i = 0; i < cachedPath.Count - 1; i++)
        {
            cache[cachedPath[i]] = i;
        }

        int consecutiveNode = -2;

        Dictionary<int, Waypoint> closedList = new Dictionary<int, Waypoint>(); // Create a dict containing nodes already seen (start is default)
        List<Waypoint> openList = new List<Waypoint>() { end };   // Create a list containing nodes seen but not assessed yet. Ordered as a priority queue

        List<Waypoint> path = new List<Waypoint>(); // List containing all the nodes in the shortest path

        List<Waypoint> currentLinks = new List<Waypoint>(); // Contains the neighbouring nodes to be assessed from the current node
        Waypoint currentWaypoint = end;

        // Init of the starting node
        currentWaypoint.SetHeuristic(Mathf.Abs(currentWaypoint.gameObject.transform.position.x - start.gameObject.transform.position.x) + Mathf.Abs(currentWaypoint.gameObject.transform.position.y - start.gameObject.transform.position.y) + Mathf.Abs(currentWaypoint.gameObject.transform.position.z - start.gameObject.transform.position.z));
        currentWaypoint.AssignNodeValue(0);
        currentWaypoint.AssignChild(null);

        bool foundStart = false;

        while ((openList.Count > 0) && (!foundStart))
        {
            currentWaypoint = RemoveHeap(openList);

            // First check to see if this node was present in the old path
            if (cache.ContainsKey(currentWaypoint))
            {
                // Check to see if the edge is the same as the old path
                if (consecutiveNode - 1 == cache[currentWaypoint])
                    foundStart = true;
                else
                    consecutiveNode = cache[currentWaypoint];
            }
            if (!foundStart)
            {
                closedList[currentWaypoint.num] = currentWaypoint;
                currentLinks = currentWaypoint.GetConnectedNodes(); // Take all the nodes that the current node is connected to
                for (int i = 0; i < currentLinks.Count; i++)
                {
                    // Make sure it's not already in the closed list. Ignore it otherwise
                    if (!closedList.ContainsKey(currentLinks[i].num))
                    {
                        float heuristic = Mathf.Pow(start.gameObject.transform.position.x - currentLinks[i].gameObject.transform.position.x, 2) + Mathf.Pow(start.gameObject.transform.position.y - currentLinks[i].gameObject.transform.position.y, 2) + Mathf.Pow(start.gameObject.transform.position.z - currentLinks[i].transform.position.z, 2);
                        float gValue = Mathf.Pow(currentLinks[i].gameObject.transform.position.x - currentWaypoint.gameObject.transform.position.x, 2) + Mathf.Pow(currentLinks[i].gameObject.transform.position.y - currentWaypoint.gameObject.transform.position.y, 2) + Mathf.Pow(currentLinks[i].gameObject.transform.position.z - currentWaypoint.transform.position.z, 2) + currentWaypoint.GetGVal();

                        // If this item has not been seen OR if it has, that it has a higher cost value
                        if ((!openList.Contains(currentLinks[i]) || (openList.Contains(currentLinks[i]) && (currentLinks[i].GetNodeValue() > gValue + heuristic))))
                        {
                            currentLinks[i].AssignChild(currentWaypoint);

                            // Assign the distance to each node
                            currentLinks[i].SetHeuristic(heuristic);
                            currentLinks[i].AssignNodeValue(gValue);

                            AddHeap(openList, currentLinks[i]);
                        }
                    }
                    // If the end is found, exit
                    if (currentLinks[i] == start)
                    {
                        currentWaypoint = start;
                        i = currentLinks.Count;
                        foundStart = true;
                    }
                }
            }
        }
        // If a path was found, otherwise the path list will return empty
        if (foundStart)
        {
            if (consecutiveNode >= 0)
                path = cachedPath.GetRange(0, consecutiveNode);

            while (currentWaypoint != null)
            {
                path.Add(currentWaypoint);
                currentWaypoint = currentWaypoint.GetChild();
            }
        }
        for (int i = 0; i < cachedPath.Count; i++)
        {
            cachedPath[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        for(int i = 0; i < path.Count; i++)
        {
            path[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        return path;
    }

    // Create the priority queue
    void AddHeap(List<Waypoint> heap, Waypoint add)
    {
        Waypoint temp = null;
        heap.Add(add);

        int i = heap.Count - 1;

        while (i > 0)
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

        if (heap.Count > 0)
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








