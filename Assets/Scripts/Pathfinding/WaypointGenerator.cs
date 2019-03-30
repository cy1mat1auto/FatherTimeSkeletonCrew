using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    GameObject waypoint;
    Waypoint[,,] waypointList;
    int[] waypointIndexes;  // The proxy for the tree
    int numBranches = 0;

    // Dimensions of the box. Temporarily public, can and should be manipulated through Init()
    public float xLength = 1;
    public float yLength = 1;
    public float zLength = 1;

    int xNodes;
    int yNodes;
    int zNodes;

    // Space between the waypoints. Number of waypoints is defined as dimensions / density for each dimension
    public float density = 1;  // Depending on density, the amount of waypoints might not fit the dimensions.

    Vector3 centre = Vector3.zero;

    public void Init(float x, float y, float z, float density, Vector3 centre)
    {
        // Precondition: x, y, z, >= 0, density > 0. Otherwise, just Mathf.Abs() and density > 0 in the future
        xLength = x;
        yLength = y;
        zLength = z;

        this.density = density;
        this.centre = centre;
    }

    // Start is called before the first frame update
    void Start()
    {
        waypoint = (GameObject)Resources.Load("Waypoint");

        xNodes = Mathf.CeilToInt(xLength / density);
        yNodes = Mathf.CeilToInt(yLength / density);
        zNodes = Mathf.CeilToInt(zLength / density);

        waypointList = new Waypoint[xNodes, yNodes, zNodes];
        numBranches = Mathf.CeilToInt(Mathf.Log(xNodes * yNodes * zNodes / 10, 2)); // Collect the required number of branches containing set of 10 waypoints per leaf
        waypointIndexes = new int[Mathf.CeilToInt(Mathf.Pow(2, numBranches))];

        // Create the entire grid first. No connections made
        for (int i = 0; i < xNodes; i++)
        {
            for(int j = 0; j < yNodes; j++)
            {
                for(int k = 0; k < zNodes; k++)
                {
                    GameObject waypointClone = Instantiate(waypoint, new Vector3(centre.x - density * ((xNodes - 1) / 2 - i) - density / 2 * (xNodes % 2), centre.y - density * ((yNodes - 1) / 2 - j) - density / 2 * (yNodes % 2), centre.z - density * ((zNodes - 1) / 2 - k) - density / 2 * (zNodes % 2)), transform.rotation);
                    waypointList[i, j, k] = waypointClone.GetComponent<Waypoint>();
                    waypointList[i, j, k].num = i * yNodes * zNodes + j * zNodes + k;
                }
            }
        }
        // Revisit the created grid and add the links. An elegant solution may arise one day
        for (int i = 0; i < xNodes; i++)
        {
            for (int j = 0; j < yNodes; j++)
            {
                for (int k = 0; k < zNodes; k++)
                {
                    if (i + 1 < xNodes)
                    {
                        waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j, k]); // RIGHT
                        waypointList[i + 1, j, k].AddNodeConnection(waypointList[i, j, k]); // LEFT

                        if(j + 1 < yNodes)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j + 1, k]); // UP RIGHT
                            waypointList[i + 1, j + 1, k].AddNodeConnection(waypointList[i, j, k]); // DOWN LEFT

                            if (k + 1 < zNodes)
                            {
                                waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j + 1, k + 1]); // RIGHT UP FORWARD
                                waypointList[i + 1, j + 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // LEFT DOWN BACKWARD
                            }
                        }

                        if (k + 1 < zNodes)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j, k + 1]); // RIGHT FORWARD
                            waypointList[i + 1, j, k + 1].AddNodeConnection(waypointList[i, j, k]); // LEFT BACKWARD
                        }
                    }
                    if (j + 1 < yNodes)
                    {
                        waypointList[i, j, k].AddNodeConnection(waypointList[i, j + 1, k]); // UP
                        waypointList[i, j + 1, k].AddNodeConnection(waypointList[i, j, k]); // DOWN

                        if (k + 1 < zNodes)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i, j + 1, k + 1]); // UP FORWARD
                            waypointList[i, j + 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // DOWN BACKWARD
                        }

                        if(i - 1 >= 0)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i - 1, j + 1, k]); // UP LEFT
                            waypointList[i - 1, j + 1, k].AddNodeConnection(waypointList[i, j, k]); // DOWN RIGHT
                        }
                    }
                    if (k + 1 < zNodes)
                    {
                        waypointList[i, j, k].AddNodeConnection(waypointList[i, j, k + 1]); // FORWARD
                        waypointList[i, j, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD

                        if ((i + 1 < xNodes) && (j - 1 >= 0))
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j - 1, k + 1]); // FORWARD DOWN RIGHT
                            waypointList[i + 1, j - 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD UP LEFT
                        }
                        if (i - 1 >= 0)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i - 1, j, k + 1]); // FORWARD LEFT
                            waypointList[i - 1, j, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD RIGHT

                            if (j + 1 < yNodes)
                            {
                                waypointList[i, j, k].AddNodeConnection(waypointList[i - 1, j + 1, k + 1]); // FORWARD UP LEFT
                                waypointList[i - 1, j + 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD DOWN RIGHT
                            }
                            if(j - 1 >= 0)
                            {
                                waypointList[i, j, k].AddNodeConnection(waypointList[i - 1, j - 1, k + 1]); // FORWARD DOWN LEFT
                                waypointList[i - 1, j - 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD UP RIGHT
                            }
                        }
                        if (j - 1 >= 0)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i, j - 1, k + 1]); // FORWARD DOWN
                            waypointList[i, j - 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD UP
                        }

                    }
                }
            }
        }
    }

    /*
    // Modified version of Kd-Tree, adapted for Skeleton Crew
    void BuildTree(int count, int[] minLength, int[] maxLength)
    {

        count = # of nodes, required for base case. Branch determined as n / 2
        minLength = lower bound of partition
        maxLength = upper bound of partition

        int[] min = minLength;
        int[] max = maxLength;

        if(count > waypointIndexes.Length)
            return;

        waypointIndexes[count] = (minLength[count % 3] + maxLength[count % 3]) / 2; // Assign the median

        min[Mathf.CeilToInt(count / 2) % 3] = 1;
        max[Mathf.CeilToInt(count / 2) % 3] = 1;

        BuildTree(2 * count + 1);   // Left
        BuildTree(2 * count + 2);   // Right
    }
*/

    // Binary search tree; Adapted Kd-tree for Skeleton Crew. Currently doesn't check for the actual closest nodes under all cases
    public Waypoint SearchTree(Transform position)
    {
        int count = 0;

        int minX = 0;
        int maxX = xNodes - 1;
        int minY = 0;
        int maxY = yNodes - 1;
        int minZ = 0;
        int maxZ = zNodes - 1;

        int pivotX = xNodes - 1;
        int pivotY = yNodes - 1;
        int pivotZ = zNodes - 1;

        Waypoint closestNode = waypointList[pivotX,pivotY,pivotZ];

        int counter = 0;

        while (count < numBranches)
        {
            counter++;
            counter++;
            switch (count % 3)
            {
                case 0:
                    pivotX = (maxX + minX) / 2;
                    if (position.position.x <= waypointList[pivotX, pivotY, pivotZ].transform.position.x)
                        maxX = (maxX + minX) / 2;
                    else if ((maxX + minX) / 2 + 1 < xNodes)
                        minX = (maxX + minX) / 2 + 1;
                    break;
                case 1:
                    pivotY = (maxY + minY) / 2;
                    if (position.position.y <= waypointList[pivotX, pivotY, pivotZ].transform.position.y)
                        maxY = (maxY + minY) / 2;
                    else if ((maxY + minY) / 2 + 1 < yNodes)
                        minY = (maxY + minY) / 2 + 1;
                    break;
                case 2:
                    pivotZ = (maxZ + minZ) / 2;
                    if (position.position.z <= waypointList[pivotX, pivotY, pivotZ].transform.position.z)
                        maxZ = (maxZ + minZ) / 2;
                    else if((maxZ + minZ) / 2 + 1 < zNodes)
                        minZ = (maxZ + minZ) / 2 + 1;
                    break;
                default:
                    print("Error");
                    return null;
            }
            if(Mathf.Pow(position.position.x - waypointList[pivotX, pivotY, pivotZ].transform.position.x, 2) + Mathf.Pow(position.position.y - waypointList[pivotX, pivotY, pivotZ].transform.position.y, 2) + Mathf.Pow(position.position.z - waypointList[pivotX, pivotY, pivotZ].transform.position.z, 2) < Mathf.Pow(position.position.x - closestNode.transform.position.x, 2) + Mathf.Pow(position.position.y - closestNode.transform.position.y, 2) + Mathf.Pow(position.position.z - closestNode.transform.position.z, 2))
                closestNode = waypointList[pivotX, pivotY, pivotZ];
            counter++;
            count++;
        }

        // Goes through the remaining nodes. Not n^3
        for(int i = minX; i < maxX + 1; i++)
        {
            for(int j = minY; j < maxY + 1; j++)
            {
                for(int k = minZ; k < maxZ + 1; k++)
                {
                    if (Mathf.Pow(position.position.x - waypointList[i, j, k].transform.position.x, 2) + Mathf.Pow(position.position.y - waypointList[i, j, k].transform.position.y, 2) + Mathf.Pow(position.position.z - waypointList[i, j, k].transform.position.z, 2) < Mathf.Pow(position.position.x - closestNode.transform.position.x, 2) + Mathf.Pow(position.position.y - closestNode.transform.position.y, 2) + Mathf.Pow(position.position.z - closestNode.transform.position.z, 2))
                        closestNode = waypointList[i, j, k];
                    counter++;
                }
            }
        }
        counter += (maxX - minX + 1) * (maxY - minY + 1) * (minZ - maxZ + 1);
     //   print("Operations: " + counter);
        return closestNode;
    }
}
