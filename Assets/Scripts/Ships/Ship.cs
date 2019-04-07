using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class containing all the gun modules, behaviour and pathfinding (needs to be added as prefabs for visual effect)
public class Ship : MonoBehaviour
{
    public bool testPathfind = true;
    protected TestPathfinding searchTarget;
    protected string[] targetTypes = {"Player"};
    protected Turret[] equippedTurrets;
    protected int numTurrets = 2;
    protected bool targetInRange = false;
    protected bool engaged = false;
    protected string shipName = "";

    // FLOCK BEHAVIOUR
    protected List<Ship> squad = new List<Ship>();
    protected int maxFlockCount = 10;

    //protected float patrolRadius = 50f;  // The RADIUS of player detection (may modify later such that the player must be in cone of detection)
    GameObject[] patrolPoints;    // Likely will not follow A* if on patrol. Depends on presence of obstacles

    // Need to review these stats
    protected int level = 1;
    protected float power = 1f;
    protected int maxHp = 10;
    protected int currentHp;
    protected float baseSpeed = 1f;
    protected float attackRadius = 30f;

    // Aggression levels (rudimentary behaviour for A.I.)
    protected float aggression = 0f;    // 0 being no aggression, 1 being max aggression
    protected float evasiveness = 0f;   // 0 being no defence, 1 being max defence
    protected float baseAggression = 0f;    // Starting aggressiveness value
    protected float baseEvasiveness = 0f;   // Starting evasiveness value
    protected float pursuitAggression = 0f; // The longer the pursuit of the player is, the more aggressive the ship becomes
    protected int hitCap = 0;  // How many times being hit will increase aggression and evasion
    protected float reactToHitAggression = 0f;  // How much aggression will increase/decrease after being hit (# of times capped by hitCap)
    protected float reactToHitEvasion = 0f; // How much evasion will increase/decrease after being hit (# of times capped by hitCap)

    // Aggression calculations
    protected float reactionTime = 8f;  // The time before evasion/aggression calculations must be recalculated
    protected bool bombRun = false; // If the ship is able to overtake the player, perform a "bomb-run", which resets the reactionTime

    protected bool init = false;

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
        searchTarget.SetTurnRadius(600);
        equippedTurrets = new Turret[numTurrets];

        // Temporary
        for(int i = 0; i < numTurrets; i++)
        {
            equippedTurrets[i] = gameObject.AddComponent<Turret>();
            equippedTurrets[i].Init(false, 0.8f, 10f, 900f, 3, 0.2f);
            equippedTurrets[i].SetTurretOffset(new Vector3(30 * (2 * i - 1), 0f, 0f));  // Testing
        }

        /*
        if(gameObject.GetComponent<SphereCollider>() != null)
            gameObject.GetComponent<SphereCollider>().radius = attackRadius;*/
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (!init)
        {
            init = true;
            // TEMPORARY!!!!
            if (gameObject.GetComponent<WaypointManager>() != null) // GENERATE RANDOM PATROL POINTS FOR EACH SHIP. TESTING
            {
                patrolPoints = new GameObject[Random.Range(2, 5)];

                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    patrolPoints[i] = gameObject.GetComponent<WaypointManager>().GetRandomWaypoint().gameObject;
                }
                SetPatrolPoints(patrolPoints);
            }
            else
            {
                engaged = true;
                searchTarget.SetEngage(engaged);
                searchTarget.SetGoal(GameObject.FindGameObjectWithTag("Player"));
            }
        }

        if (currentHp <= 0)
        {
            for(int i = 0; i < numTurrets; i++)
            {
                Destroy(equippedTurrets[i]);
            }
            Destroy(gameObject);
        }

        /*
        if (targetInRange)
        {
            bool hitObject = Physics.Raycast(transform.position, -transform.up, out RaycastHit lineOfSight, attackRadius * 100);

            for (int i = 0; i < numTurrets; i++)
            {
                if ((equippedTurrets[i] != null) && (hitObject) && (lineOfSight.transform.tag == "Player"))
                    equippedTurrets[i].Activate(-gameObject.transform.up);  // Hard-coded offset
            }
        }
        */
    }

    // Check if it has a flock attached to it, and inform each flock to disengage or find a new leader
    private void OnDestroy()
    {
        if (squad.Count == 1)
            squad[0].GetComponent<FlockBehaviour>().AssignLeader(null);
        else
        {
            for (int i = 0; i < squad.Count; i++)
            {
                if (squad[i] != null)
                {
                    for(int j = i; j < squad.Count; j++)
                    {
                        if(squad[j] != null)
                            squad[j].GetComponent<FlockBehaviour>().AssignLeader(squad[i]);
                    }
                    i = squad.Count;
                }
            }
        }
    }

    protected virtual void SetPatrolPoints(GameObject[] setPatrolPoints)
    {
        // POINTS WILL BE PATROLLED IN ORDER
        patrolPoints = setPatrolPoints;
        searchTarget.SetGoals(patrolPoints);
    }


    // DO NOT INHERIT THESE
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetInRange = true;

            if(!engaged)
            {
                engaged = true;
                searchTarget.SetEngage(engaged);
                searchTarget.SetGoal(other.gameObject);
            }

            /*
             * 
             * 
             * TEMPORARY!!!!
             * 
             * */

            if (Vector3.Angle(transform.position, other.transform.position) < 65f)
            {
                for (int i = 0; i < numTurrets; i++)
                {
                    if (equippedTurrets[i] != null)
                    {
                        //equippedTurrets[i].Activate(-gameObject.transform.up);  // Hard-coded offset
                        equippedTurrets[i].Activate((other.transform.position - transform.position).normalized);//gameObject.transform.forward);  // Hard-coded offset
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            targetInRange = false;
    }

    public void SetName(string name)
    {
        shipName = name;
    }

    public string GetName()
    {
        return shipName;
    }

    public float GetTurnRadius()
    {
        // The turn radius returned is a SQUARED VALUE
        if (gameObject.GetComponent<TestPathfinding>() != null)
            return gameObject.GetComponent<TestPathfinding>().GetTurnRadius();
        return 0;
    }





    public void SetFlockCount(int setCount)
    {
        maxFlockCount = setCount;
    }
    public int GetMaxFlockCount()
    {
        return maxFlockCount;
    }
    public int GetFlockCount()
    {
        return squad.Count;
    }
    public int AddToFlock(Ship add)
    {
        if (squad.Count < maxFlockCount)
        {
            squad.Add(add);
            return squad.Count;
        }
        return -1;
    }
    public void RemoveFromFlock(FlockBehaviour remove)
    {
        squad.Remove(remove.gameObject.GetComponent<Ship>());
    }
    public void ClearFlock()
    {
        squad.Clear();
    }

    // This will probably be moved to ship.cs
    public List<Ship> GetFlock()
    {
        return squad;
    }
}
