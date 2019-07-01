using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLOrientation : MonoBehaviour
{

    //This script will readjust orientation of an AI-controlled ship to match its Goal's orientation when it gets close enough.
    public Rigidbody rb;
    public GameObject Goal;
    //Currently, ship reorients at distance Close2, and cuts engine at Close1: 
    public float Range = 150f, Close1 = 20f, Close2 = 40f;
    private RaycastHit view, front;
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, Goal.transform.position) >= Close1)
        {
            rb.AddRelativeForce(new Vector3(0, 0, 120f), ForceMode.Force);
        }

        if (Vector3.Distance(transform.position, Goal.transform.position) >= Close2)
        {
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Goal.transform.position - transform.position, transform.up), Time.deltaTime * 4f);
        }

        else
        {
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Goal.transform.position - transform.position, Goal.transform.up), Time.deltaTime * 2f);
        }
              
    }
}
