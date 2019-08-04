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
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireMissile()
    {
        if (Time.time - PreviousShot >= FireRate)
        {
            Quaternion missilerot = Quaternion.LookRotation(Player.GetComponent<CapsuleCollider>().transform.position - transform.position + 2*transform.up, Vector3.up);
            Instantiate(Resources.Load<GameObject>("EnemyRocket01"), transform.position + 2*transform.up, missilerot);
            PreviousShot = Time.time;
        }
        
    }
}
