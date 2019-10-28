using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWPMngr : MonoBehaviour
{
    private Dictionary<GameObject, ConAndD> ComputedWaypoints;
    private List<GameObject> waypoints = new List<GameObject>();
    private float Dist;
    private GameObject[] Connections;

    private void OnDrawGizmos()
    {
        //First, iterate through the list of waypoints
        for (int i = 0; i < waypoints.Count; i++)
        {
            //Actually, there is no reason the following nested for statements should be in this script rather than on individual waypoints:

            //Each waypoint in the list has a GizmoLine script attached, with an array of connections called "Adjacent":
            Connections = waypoints[i].GetComponent<GizmoLine>().Adjacent;

            //Let's iterate through "Adjacent" waypoints:
            for (int j = 0; j < Connections.Length; j++)
            {
                //Because this array can have a null value when a path is blocked by terrain, we need to check if each connection is valid:
                if (Connections[j] != null)
                {
                    Dist = Vector3.Distance(Connections[j].transform.position, waypoints[i].transform.position);

                    //When all criteria are met, we can store these values in our sub-dictionary ConAndD:
                    if(waypoints[i].GetComponent<ConAndD>() == null)
                    {
                        waypoints[i].AddComponent<ConAndD>();
                        waypoints[i].GetComponent<ConAndD>().ConnectAndDistance.Add(Connections[j], Dist);
                    }
                    
                    else
                    {
                        waypoints[i].GetComponent<ConAndD>().ConnectAndDistance.Add(Connections[j], Dist);
                    }
                }
            }

            //After a dictionary of all "Adjacents" and their corresponding distances to the original waypoint is created,
            //key this into the main dictionary:
            ComputedWaypoints.Add(waypoints[i], waypoints[i].GetComponent<ConAndD>());
        }
            
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class ConAndD: MonoBehaviour
{
    public Dictionary<GameObject, float> ConnectAndDistance;
}
