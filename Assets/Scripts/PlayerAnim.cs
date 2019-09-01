using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator Animator;
    private int Speed, Strafe;

    // Start is called before the first frame update
    void Start()
    {
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKey)
        {
            Speed = 0;
            Strafe = 0;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 2;
        }

        else if (Input.GetKey(KeyCode.W))
        {
            Speed = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Strafe = 1;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Strafe = 2;
        }

        else
        {
            Strafe = 0;
        }

        Animator.SetInteger("Speed", Speed);
        Animator.SetInteger("Strafe", Strafe);
    }
}
