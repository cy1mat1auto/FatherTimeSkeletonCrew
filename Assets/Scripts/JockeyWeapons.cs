using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JockeyWeapons : MonoBehaviour
{

	private RaycastHit vision;
	private float rayLength;
    public static bool onObject;
    public static Vector3 laserEnd;

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
        }
        else
        {
            laserEnd = transform.forward * 100000;
        }
    }
}
