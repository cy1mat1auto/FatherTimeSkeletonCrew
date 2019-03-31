using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Waypoint class to attach to each waypoint
public class Waypoint : MonoBehaviour
{
    // Hold the list of all connections this waypoint has. Turn this private
    public List<Waypoint> waypoints = new List<Waypoint>();

    public int num = 0;
    float gVal = 0f;    // Value of cumulative distance
    float hVal = 0f;    // Heuristic value
    float fVal = 0f;    // Total value (g + h)

    Waypoint child = null;    // Used like a linked-list to keep track of the shortest path

    public bool blockedNode = false; // If this node is blocked by another object (sets this via onTriggerEnter)
    public int target = -1;

    // Used by obstacles
    public void BlockNode(bool block)
    {
        blockedNode = block;
    }
    public bool IsBlocked()
    {
        return blockedNode;
    }

    public void SetHeuristic(float hVal)
    {
        this.hVal = hVal;
    }
    public float GetHeuristic()
    {
        return hVal;
    }
    public float AssignNodeValue(float gVal)
    {
        // Assign the g for the waypoint and return f, where f = h + g
        this.gVal = gVal;
        fVal = gVal + hVal;

        return fVal;
    }
    public float GetGVal()
    {
        return gVal;
    }
    public float GetNodeValue()
    {
        return fVal;
    }
    public void AssignChild(Waypoint assignChild)
    {
        child = assignChild;
    }
    public Waypoint GetChild()
    {
        return child;
    }

    public void AddNodeConnection(Waypoint newNodeConnection)
    {
        waypoints.Add(newNodeConnection);
    }
    public List<Waypoint> GetConnectedNodes()
    {
        return waypoints;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player"))
        {
            BlockNode(true);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (CalculateDistance(collision.GetComponent<Ship>().transform.position, transform.position) <= 100f)
            {
                BlockNode(true);
            }
        }
        else if (!collision.CompareTag("Player"))
        {
            BlockNode(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
            target = -1;
        if (!collision.CompareTag("Player"))
        {
            BlockNode(false);
        }
    }

    private float CalculateDistance(Vector3 o1, Vector3 o2)
    {
        float total = Mathf.Pow(o1.x - o2.x, 2) + Mathf.Pow(o1.y - o2.y, 2) + Mathf.Pow(o1.z - o2.z, 2);
        return (Mathf.Sqrt(total));
    }
}
