using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    //Controls the guide arrow; an experimental navigation aid
    private bool Navigating = false;
    private float SwitchDelay = 0.5f;
    private float LastSwitch = 0f;
    public GameObject Destination;

    // Start is called before the first frame update
    void Start()
    {
        Navigating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Destination != null)
        {
           transform.LookAt(Destination.transform);

            if (Input.GetKeyDown(KeyCode.C) && Navigating == false && Time.time - LastSwitch > SwitchDelay)
            {
                LastSwitch = Time.time;
                Navigating = true;
                //Debug.Log("Navigating");
            }

            if (Input.GetKeyUp(KeyCode.C) && Navigating == true && Time.time - LastSwitch > SwitchDelay)
            {
                LastSwitch = Time.time;
                Navigating = false;
                //Debug.Log("Not Navigating");
            }
        }

       else
        {
            Navigating = false;
        }

        if (Navigating == true)
        {
            GetComponentInChildren<Renderer>().enabled = true;
        }

        else
        {
            GetComponentInChildren<Renderer>().enabled = false;
        }
    }
}
