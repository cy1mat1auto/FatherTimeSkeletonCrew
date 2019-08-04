using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeap : MonoBehaviour
{
    //A script to manage all the weapons attached to an enemy ship

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireMissile()
    {
        Instantiate(Resources.Load<GameObject>("EnemyRocket01"), transform.position, transform.rotation);
    }
}
