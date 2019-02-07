using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class containing all the gun modules, behaviour and pathfinding (needs to be added as prefabs for visual effect)
public class Ship : MonoBehaviour
{
    protected TestPathfinding searchTarget;
    protected Turret[] equippedTurrets;
    protected int numTurrets = 1;
    //protected float range = 10f;    // May or may not need
    protected bool targetInRange = false;

    // Need to review these stats
    protected int level = 1;
    protected float power = 1f;
    protected int maxHp = 10;
    protected int currentHp;
    protected float baseSpeed = 1f;
    protected float attackRadius = 20f;


    protected void Init(int numTurrets = 1, int maxHp = 10, float baseSpeed = 1f, float power = 1f, float radius = 10f, int level = 1)
    {
        this.numTurrets = Mathf.Abs(numTurrets);
        this.baseSpeed = baseSpeed;
        this.maxHp = maxHp;
        this.power = power;
        this.level = level;
        attackRadius = radius;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHp = maxHp;
        searchTarget = gameObject.AddComponent<TestPathfinding>();
        equippedTurrets = new Turret[numTurrets];

        // Temporary
        for(int i = 0; i < numTurrets; i++)
        {
            equippedTurrets[i] = gameObject.AddComponent<Turret>();
            equippedTurrets[i].Init(false, 1f, 10f, 100f, 3, 0.3f);
        }

        if(gameObject.GetComponent<SphereCollider>() != null)
            gameObject.GetComponent<SphereCollider>().radius = attackRadius;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentHp <= 0)
        {
            for(int i = 0; i < numTurrets; i++)
            {
                Destroy(equippedTurrets[i]);
            }
            Destroy(gameObject);
        }

        if (targetInRange)
        {
            for (int i = 0; i < numTurrets; i++)
            {
                if (equippedTurrets[i] != null)
                    equippedTurrets[i].Activate();
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")//Player")
            targetInRange = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Finish")
            targetInRange = false;
    }
}
