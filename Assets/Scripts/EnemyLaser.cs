using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float laserRange = 50f;
    public Transform droneEnd;
    public Transform playerPoint;
    public Transform drone;
    public GameObject TrackingLaser;
    public GameObject DeathLaser;
    public float damage = 10f;
    //public Color white = new Color(255f, 255f, 255f);
    public Color pink = new Color(255f, 150f, 150f);

    private LineRenderer laserLine;
    private LineRenderer deathLaser;
    public int timer = 5; //How long Trackinglaser lasts
    public int dLtimer = 5; //How long deathLaser lasts
    private int pause;
    private int increment;
    private int cSpeed;
    private GameObject Jockey;
    private float colorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        TrackingLaser = GameObject.Find("EnemyLaser");
        laserLine = TrackingLaser.GetComponent<LineRenderer>();

        DeathLaser = GameObject.Find("deathLaser");
        deathLaser = DeathLaser.GetComponent<LineRenderer>();

        laserLine.enabled = true;
        deathLaser.enabled = false;

        pause = timer + dLtimer;
        increment = pause;
        cSpeed = timer - 1;
    }

    // Update is called once per frame
    void Update()
    {
        colorSpeed += Time.deltaTime/cSpeed;
        //laserLine.SetPosition(0, droneEnd.position);
        //laserLine.SetPosition(1, playerPoint.position);
        RaycastHit hit;

        laserLine.SetPosition(0, droneEnd.position);
        deathLaser.SetPosition(0, droneEnd.position);

        if (Physics.Raycast(droneEnd.position, drone.transform.forward, out hit, laserRange))
        {
            laserLine.SetPosition(1, hit.point);
            deathLaser.SetPosition(1, hit.point);

            if (deathLaser.enabled == true && hit.rigidbody != null)
            {
                Jockey = hit.rigidbody.gameObject;
                if (Jockey.tag == "Player")
                {
                    Jockey.GetComponent<PlayerHealth>().CurrentHealth -= Mathf.Round(damage * Time.deltaTime);
                }
            }
        }
        else
        {
            laserLine.SetPosition(1, droneEnd.position + (drone.transform.forward * laserRange));
            deathLaser.SetPosition(1, droneEnd.position + (drone.transform.forward * laserRange));
        }

        if (Time.time >= timer) //When deathLaser shoots
        {
            laserLine.enabled = false;
            deathLaser.enabled = true;
            timer += increment;
        }

        if (Time.time >= pause) //When trackingLaser shoots
        {
            laserLine.enabled = true;
            deathLaser.enabled = false;
            pause += increment;
        }

        if (laserLine.enabled == true)
        {
            laserLine.material.color = Color.Lerp(pink, Color.yellow, colorSpeed);

        }
        else
        {
            laserLine.material.color = pink;
            colorSpeed = 0;
        }
    }

}