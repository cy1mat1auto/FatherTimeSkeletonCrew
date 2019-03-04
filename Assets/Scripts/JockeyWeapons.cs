using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JockeyWeapons : MonoBehaviour
{
    public GameObject OneLaser;
	private RaycastHit vision;
	private float rayLength;
    public static bool onObject;
    public static Vector3 laserEnd;
    public static Transform missileTarget;

    // Start is called before the first frame update
    void Start()
    {
		rayLength = 200;
        onObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red, 8.5f);
        onObject = Physics.Raycast(transform.position, transform.forward * rayLength, out vision, rayLength);
        if (onObject)
        {
            laserEnd = vision.collider.transform.position;
            missileTarget = vision.collider.transform;
            if (vision.transform.tag == "Enemy" && OneLaser.GetComponent<laserScript>().pressed)
            {
                vision.transform.gameObject.GetComponent<EnemyHealth>().CurrentHealth -= 1;
            }

            //for hitting explosive barrels;
            else if (vision.transform.tag == "Explosive" && OneLaser.GetComponent<laserScript>().pressed)
            {
                vision.transform.gameObject.GetComponent<ExploBarrel2>().health -= 1;
            }
        }
        else
        {
            laserEnd = transform.forward * 100000;
        }
    }
}
