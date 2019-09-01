using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Animator Doors;
    public Collider DoorSensor;
    // Start is called before the first frame update
    void Start()
    {
        if (Doors == null)
        {
            Doors = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Doors.SetBool("New Bool", true);
        }
    }
}
