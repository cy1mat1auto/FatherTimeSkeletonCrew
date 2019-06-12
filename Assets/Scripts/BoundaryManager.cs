using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    //Centralized Script for managing level boundaries
    public GameObject Top;
    public GameObject Bottom;
    public GameObject Left;
    public GameObject Right;
    public GameObject Front;
    public GameObject Rear;

    public float OOBLim;
    private float OOBEvent = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
