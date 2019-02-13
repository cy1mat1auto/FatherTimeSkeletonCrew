using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class containing all the gun modules, behaviour and pathfinding (needs to be added as prefabs for visual effect)
public class Ship : MonoBehaviour
{
    protected TestPathfinding searchTarget;
    protected Turret[] equippedTurrets;
    protected int numTurrets = 2;
    //protected float range = 10f;    // May or may not need
    protected bool targetInRange = false;

    // Need to review these stats
    protected int level = 1;
    protected float power = 1f;
    protected int maxHp = 10;
    protected int currentHp;
    protected float baseSpeed = 1f;
    protected float attackRadius = 30f;


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
            equippedTurrets[i].Init(false, 0.8f, 10f, 300f, 3, 0.2f);
            equippedTurrets[i].SetTurretOffset(new Vector3(30 * (2 * i - 1), 0f, 0f));  // Testing
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
            bool hitObject = Physics.Raycast(transform.position, -transform.up, out RaycastHit lineOfSight, attackRadius * 100);

            for (int i = 0; i < numTurrets; i++)
            {
                if ((equippedTurrets[i] != null) && (hitObject) && (lineOfSight.transform.tag == "Player"))
                    equippedTurrets[i].Activate(-gameObject.transform.up);  // Hard-coded offset
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//Player")
            targetInRange = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            targetInRange = false;
    }
}
