using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeap : MonoBehaviour
{
    public float FireRate = 0.7f;
    public GameObject Player;
    private float PreviousShot;
    //A script to manage all the weapons attached to an enemy ship

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireMissile()
    {
        if (Time.time - PreviousShot >= FireRate)
        {
            Vector3 Source = transform.position - 3*transform.up;
            Quaternion missilerot = Quaternion.LookRotation((Player.transform.position - Source).normalized);
            GameObject Missile = Instantiate(Resources.Load<GameObject>("EnemyRocket01"), Source, missilerot);
            //Debug.DrawLine(Source, Player.GetComponent<CapsuleCollider>().bounds.center, Color.yellow, 1f);
            //Missile.transform.LookAt(Player.GetComponent<CapsuleCollider>().bounds.center);
            PreviousShot = Time.time;
            //Debug.Log(Player.GetComponent<CapsuleCollider>().bounds.center);
        }
        
    }
}
