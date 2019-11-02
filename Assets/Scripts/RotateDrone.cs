using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDrone : MonoBehaviour
{
    public Transform playerPoint;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = playerPoint.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = playerPoint.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}
