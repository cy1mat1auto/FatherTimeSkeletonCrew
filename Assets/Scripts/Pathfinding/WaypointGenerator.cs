using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    GameObject waypoint;

    // Dimensions of the box. Temporarily public, can and should be manipulated through Init()
    public float xLength = 1;
    public float yLength = 1;
    public float zLength = 1;

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

        int xNodes = Mathf.CeilToInt(xLength / density);
        int yNodes = Mathf.CeilToInt(yLength / density);
        int zNodes = Mathf.CeilToInt(zLength / density);

        Waypoint[,,] waypointList = new Waypoint[xNodes, yNodes, zNodes];
 
        // Create the entire grid first. No connections made
        for (int i = 0; i < xNodes; i++)
        {
            for(int j = 0; j < yNodes; j++)
            {
                for(int k = 0; k < zNodes; k++)
                {
                    GameObject waypointClone = Instantiate(waypoint, new Vector3(centre.x - density * ((xNodes - 1) / 2 - i) - density / 2 * (xNodes % 2), centre.y - density * ((yNodes - 1) / 2 - j) - density / 2 * (yNodes % 2), centre.z - density * ((zNodes - 1) / 2 - k) - density / 2 * (zNodes % 2)), transform.rotation);
                    waypointList[i, j, k] = waypointClone.GetComponent<Waypoint>();
                    waypointList[i, j, k].num = i + j + k;
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

                            if(k + 1 < zNodes)
                            {
                                waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j, k + 1]); // RIGHT FORWARD
                                waypointList[i + 1, j, k + 1].AddNodeConnection(waypointList[i, j, k]); // LEFT BACKWARD

                                waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j + 1, k + 1]); // RIGHT UP FORWARD
                                waypointList[i + 1, j + 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // LEFT DOWN BACKWARD
                            }
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

                        if((i + 1 < xNodes) && (j - 1 >= 0))
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i + 1, j - 1, k + 1]); // FORWARD DOWN RIGHT
                            waypointList[i + 1, j - 1, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD UP LEFT
                        }
                        if (i - 1 >= 0)
                        {
                            waypointList[i, j, k].AddNodeConnection(waypointList[i - 1, j, k + 1]); // FORWARD LEFT
                            waypointList[i - 1, j, k + 1].AddNodeConnection(waypointList[i, j, k]); // BACKWARD RIGHT

                            if(j + 1 < yNodes)
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
}
