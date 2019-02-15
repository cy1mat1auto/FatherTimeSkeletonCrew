using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidRotation : MonoBehaviour
{
    private Quaternion rot;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rot = Quaternion.Euler(new Vector3 (Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MoveRotation(rb.rotation * rot);
    }
}
