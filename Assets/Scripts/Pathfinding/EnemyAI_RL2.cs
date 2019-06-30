﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_RL2 : MonoBehaviour
{

    // Decision-tree based AI by Richard Liu, version 2
    public GameObject Ship, Player;
    public Rigidbody rb;
    public float Range = 150f;
    public bool Spotted, Avoiding;
    private RaycastHit view, front;

    // Start is called before the first frame update
    void Start()
    {
        if (Ship == null)
        {
            Ship = gameObject;
        }

        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //Enemy will only detect player if within Range, and within a cone of vision
        if (Vector3.Distance(transform.position, Player.transform.position) <= Range && Vector3.Angle(transform.forward, Player.transform.position - transform.position) <= 45f)
        {
            Spotted = true;
            Debug.Log(Spotted);
        }

        else if (!Avoiding)
        {
            Spotted = false;
        }

        if (! Spotted)
        {
            rb.AddRelativeForce(new Vector3(0, 0, 150f), ForceMode.Force);
            rb.rotation *= Quaternion.Euler(new Vector3(0, 0.4f, 0));
            CancelInvoke("Seeking");
        }

        else
        {
            //Tying raycast to InvokeRepeating limits frequency of raycasts.
            InvokeRepeating("Seeking", 0f, 0.1f);

            if (view.collider && view.collider == front.collider)
            {
                if (view.collider.CompareTag("Player"))
                {
                    Avoiding = false;
                    Debug.Log("Ready to Fire");
                }

                else if (view.collider.CompareTag("Enemy"))
                { 
                    Avoiding = true;
                }
                

                else if (view.collider.CompareTag("Terrain"))
                {

                }
            }

            else if (view.collider && view.collider != front.collider && Avoiding)
            {
                if (view.collider.CompareTag("Player"))
                {
                    Avoiding = false;
                    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position, transform.up), Time.deltaTime * 2f);
                }
            }

            else if (!Avoiding)
            {
                rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position, transform.up), Time.deltaTime * 2f);
                //rb.MoveRotation(Quaternion.LookRotation(Player.transform.position - transform.position, transform.up));
            }

            if (Avoiding)
            {
                //first, try to fly under the obstacle:
                Vector3 ImmediateGoal = view.transform.position + view.collider.bounds.size.y * -transform.up;
                if (transform.position != ImmediateGoal)
                {
                    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ImmediateGoal - transform.position, transform.up), Time.deltaTime * 2f);
                    rb.AddRelativeForce(new Vector3(0, 0, 150f), ForceMode.Force);
                }


            }
            
        }
    }

    void Seeking()
    {
        Physics.Raycast(transform.position, Player.transform.position - transform.position, out view, Range);
        Physics.Raycast(transform.position, transform.forward, out front, Range);
    }
}
