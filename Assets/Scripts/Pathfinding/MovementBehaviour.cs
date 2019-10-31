using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementTypes {A, FLOCK, BFS, ASTAR};

public abstract class MovementBehaviour : MonoBehaviour
{
    protected float totalPriority = 0f; // priority with or w/o additionalPriority. How or when these are added depends on the child classes.
    protected float priority = 0f;  // Higher priority means this behaviour will override others when its conditions are met
    protected float additionalPriority = 0f;   // Additional priority value to elevate this behaviour when necessary. Eg. With Default behaviour
    protected bool sharedBehaviour = false;  // If true, this behaviour can run concurrently with other movement behaviours
    protected bool activated = false;   // Whether this behaviour is currently used or not (0 = false, 1 = true)
    protected bool defaultBehavior = false; // Whether this behaviour is the default
    protected float detectionRadius = 100f; // Activate if something enters radius of detection
    protected Ship parent;  // The reference

    // Ship stats
    protected float turnRadius = 0f;

    public virtual void SetTurnRadius(float newRadius)
    {
        turnRadius = Mathf.Pow(newRadius, 2);
    }
    public float GetTurnRadius()
    {
        return turnRadius;
    }
    public float GetPriority()
    {
        return priority;
    }
    public void SetPriority(float newValue)
    {
        totalPriority += newValue - priority;   // Adjust the totalPriority to accomodate changes in values
        priority = newValue;
    }
    public float GetAdditionalPriority()
    {
        return additionalPriority;
    }
    public void SetAdditionalPriority(float newValue)
    {
        additionalPriority = newValue;
    }
    public float GetTotalPriority()
    {
        return totalPriority;
    }

    public bool IsActivated()
    {
        return activated;
    }
    public void SetDefaultBehaviour(bool setDefault)
    {
        // A behaviour that must always be active even if the conditions are not specifically met
        defaultBehavior = setDefault;
    }
    public bool GetDefaultBehaviour()
    {
        return defaultBehavior;
    }
    public void SetSharedBehaviour(bool setShared)
    {
        // Force shared behaviour. Unexpected properties might arise
        sharedBehaviour = setShared;
    }
    public bool GetSharedBehaviour()
    {
        return sharedBehaviour;
    }

    // Initialize the behaviour, like a constructor
    public virtual void Init(Ship parent, float priorityValue = -1f)
    {
        this.parent = parent;
        if (priorityValue > 0)
            priority = priorityValue;

        totalPriority = priority;
    }
 
    public virtual void PingParent()
    {
        // When activated, will alert the parent
        //activated = false;
        parent?.BehaviourActivation(this);
    }

    public abstract void SetTarget(GameObject target);  // Who the ship is currently targeting
    public abstract bool ExecuteBehaviour(); // A return value of false means that it is ready to deactivate
    public abstract void EndBehaviour();    // What should happen after disengaging from behaviour
}
