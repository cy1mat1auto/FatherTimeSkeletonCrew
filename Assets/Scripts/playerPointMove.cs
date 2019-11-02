using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

public class playerPointMove : MonoBehaviour
{
    public Transform Jockey;
    public Transform droneEnd;

    private bool value;
    public int timer1;
    private int pause1;
    private int increment1;
    private float relocate;
    public GameObject DroneShip;

    private void Start()
    {
        DroneShip = GameObject.Find("DroneShip02");
        timer1 = DroneShip.GetComponent<EnemyLaser>().timer;
        pause1 = DroneShip.GetComponent<EnemyLaser>().dLtimer + DroneShip.GetComponent<EnemyLaser>().timer;
        increment1 = pause1;
        relocate = pause1;
    }

    void Update()
    {

        if (Time.time >= timer1)
        {
            value = true;
            timer1 += increment1;
        }

        else if (Time.time >= pause1)
        {
            value = false;
            pause1 += increment1;
        }

        if (value == true)
        {
            Jockey = null;
        }

        else if (value == false)
        {
            Jockey = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = Vector3.MoveTowards(transform.position, Jockey.position, 1f);

            if (Time.time >= relocate)
            {
                transform.position = Jockey.position;
                relocate += increment1;
            }
        }

    }

}
