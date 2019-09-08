using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator Animator;
    public Renderer ShipRender;
    public Material EngineOff;
    public Material EngineSlow;
    public Material EngineFast;
    private int Speed, Strafe;

    // Start is called before the first frame update
    void Start()
    {
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }

        Instantiate(EngineFast);
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
            if (!Input.GetKey(KeyCode.W))
            {
                Speed = 1;
            }
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Strafe = 2;
            if (!Input.GetKey(KeyCode.W))
            {
                Speed = 1;
            }
        }

        else
        {
            Strafe = 0;
        }

        if (Speed == 2)
        {
            Material[] mat = ShipRender.materials;
            mat[4] = EngineFast;
            ShipRender.materials = mat;
        }

        else if (Speed == 1)
        {
            Material[] mat = ShipRender.materials;
            mat[4] = EngineSlow;
            ShipRender.materials = mat;
        }

        else if (Speed == 0)
        {
            Material[] mat = ShipRender.materials;
            mat[4] = EngineOff;
            ShipRender.materials = mat;
        }

        Animator.SetInteger("Speed", Speed);
        Animator.SetInteger("Strafe", Strafe);
    }
}
