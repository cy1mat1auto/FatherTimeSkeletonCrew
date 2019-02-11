using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidRotation : MonoBehaviour
{
    private Vector3 rot;

    // Start is called before the first frame update
    void Start()
    {
        rot = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rot);
    }
}
