using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSize : MonoBehaviour
{
    private Vector3 newSize;

    // Start is called before the first frame update
    void Start()
    {
        newSize = new Vector3(Random.Range(2, 5), Random.Range(2, 5), Random.Range(2, 5));
        transform.localScale = newSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
