using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class containing all the gun modules, behaviour and pathfinding (needs to be added as prefabs for visual effect)
public class Ship : MonoBehaviour
{
    List<MovementBehaviour> movementBehaviour = new List<MovementBehaviour>();  // Complete list of all behaviours this ship is capable of
    List<MovementBehaviour> activatedBehaviours = new List<MovementBehaviour>();    // List of behaviours that have been activated (BehaviourActivation)

    protected string[] targetTypes = {"Player"};
    protected GameObject mainTarget;
    protected Turret[] equippedTurrets;
    protected int numTurrets = 2;
    protected bool targetInRange = false;
    protected bool engaged = false; // Prevents the ship from re-engaging with different enemies constantly if they are detected
    protected string shipName = ""; // Used for identification in flocking and targeting

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


    /* CURRENTLY UNUSED
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
    */
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

    public void InsertionSortBehaviourPriority()
    {
        // Insertion sort. The list will very likely be small and nearly sorted, with additional behaviour being added sparsely. 
        // If initial loading of movementbehaviour types is high, can init with a different type of sort. Sort in descending order
        if (activatedBehaviours.Count <= 1)
            return;
        int reference = 0;
        MovementBehaviour temp;
        for(int i = 0; i < activatedBehaviours.Count; i++)
        {
            reference = i - 1;
            while (reference >= 0)
            {
                // .GetPriority() applies to all behaviour. .GetAdditionalPriority() also applies to all, but is used with .IsActivated() for default behaviour that would be overridden unless activated
                if(activatedBehaviours[i].GetTotalPriority() >= activatedBehaviours[reference].GetTotalPriority())
                {
                    temp = activatedBehaviours[i];
                    activatedBehaviours[i] = activatedBehaviours[reference];
                    activatedBehaviours[reference] = temp;
                    reference = -1;
                }
                reference--;
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHp = maxHp;

        movementBehaviour.Add(AssignMovementBehaviour.CreateBehaviour(MovementTypes.A, gameObject));
        movementBehaviour.Add(AssignMovementBehaviour.CreateBehaviour(MovementTypes.FLOCK, gameObject));

        for(int i = 0; i < movementBehaviour.Count; i++)
        {
            movementBehaviour[i].Init(this);
            movementBehaviour[i].SetTurnRadius(600);
        }

        // Add the first behaviour as the default for movement (can set according to what movement should be most important to set as default)
        movementBehaviour[0].SetDefaultBehaviour(true);
        movementBehaviour[0].SetAdditionalPriority(movementBehaviour[0].GetPriority());
        movementBehaviour[0].SetPriority(0);
        BehaviourActivation(movementBehaviour[0]);

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
            //  engaged = true;
            mainTarget = GameObject.FindGameObjectWithTag("Player");
            for (int i = 0; i < movementBehaviour.Count; i++)
            {
                movementBehaviour[i].SetTarget(mainTarget);
            }
        }

        // Where ship movement behaviour is executed (Assume sorted via InsertionSortBehaviourPriority())
        for(int i = 0; i < activatedBehaviours.Count; i++)
        {
            // If the behaviour no longer applies and is not the default
            if(!activatedBehaviours[i].ExecuteBehaviour())
            {
                activatedBehaviours[i].EndBehaviour();
                if(!activatedBehaviours[i].GetDefaultBehaviour())
                    activatedBehaviours.Remove(activatedBehaviours[i]);
            }
            if(!activatedBehaviours[i].GetSharedBehaviour())
                i = activatedBehaviours.Count;
        }

        if (currentHp <= 0)
        {
            for(int i = 0; i < numTurrets; i++)
            {
                Destroy(equippedTurrets[i]);
            }
            Destroy(gameObject);
        }
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
    }

    public GameObject[] GetPatrolPoints()
    {
        return patrolPoints;
    }


    // DO NOT INHERIT THESE
    void OnTriggerEnter(Collider other)
    {
        if(System.Array.Exists(targetTypes, id => id == other.gameObject.tag))
        {
            targetInRange = true;
            mainTarget = other.gameObject;

            if(!engaged)
            {
                engaged = true;
                for(int i = 0; i < movementBehaviour.Count; i++)
                {
                    movementBehaviour[i].SetTarget(mainTarget);
                }
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


    public void BehaviourActivation(MovementBehaviour alert)
    {
        // Assuming the list of behaviours will be small enough for List.Contains() to not cause problems
        if(!activatedBehaviours.Contains(alert))
            activatedBehaviours.Add(alert);
        // Sort regardless of if in list or not because it might have an updated priority value
        InsertionSortBehaviourPriority();
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
