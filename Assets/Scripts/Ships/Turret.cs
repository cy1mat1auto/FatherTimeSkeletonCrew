using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Parent class Turrets, with cooldown time, range and attack type
public class Turret : MonoBehaviour
{
    public GameObject projectile;

    protected float maxCooldownTimer = 3f;
    protected float cooldownTimer = 0f;
    protected float maxBurstDelay = 0.3f;
    protected float burstDelay;
    protected int maxBurstCount;
    protected int burstCount = 1;
    protected float power = 1f;
    protected float shotSpeed = 500f;
    protected bool activateAttack = false;
    protected Vector3 direction;
    protected Vector3 positionOffset;

    // The threshold for turret behaviour
    protected float aggressionThreshold = 0f;
    protected float defenceThreshold = 0f;
    protected float priority = 0f;  // Turret priority behaviour. Higher priority means it will be considered first
    protected float aggressionDecrement = 0f;   // How much will the aggression level be decremented if this is activated
    protected float defenceDecrement = 0f;  // How much will the defence level be decremented if this is activated
 

    protected bool targetEnemy = true;  // True means it targets enemies, False = targets player

    // Used by A.I. If turret activation is successful, return true
    public virtual bool Activate(float aggressionValue, float defenceValue, Vector3 direction)
    {
        if ((cooldownTimer <= 0f) && ((aggressionValue >= aggressionThreshold) || (defenceValue >= defenceThreshold)))
        {
            Activate(direction);
            return true;
        }
        return false;
    }

    public virtual void Activate(Vector3 direction)
    {
        if (cooldownTimer <= 0f)
        {
            activateAttack = true;
            this.direction = direction;
        }
    }

    public void SetTurretOffset(Vector3 positionOffset)
    {
        this.positionOffset = positionOffset;
    }

    public void Init(bool targetEnemy, float cooldown, float power, float projectileSpeed, int burstCount, float burstDelay, float aggressionThreshold = 0f, float defenceThreshold = 1.1f)
    {
        this.targetEnemy = targetEnemy;
        maxCooldownTimer = cooldown;
        cooldownTimer = cooldown;
        this.power = power;
        maxBurstCount = burstCount;
        this.burstCount = burstCount;
        maxBurstDelay = burstDelay;
        this.burstDelay = burstDelay;
        shotSpeed = projectileSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Testing
        projectile = (GameObject)Resources.Load("Testing");
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if(activateAttack)
        {
            burstDelay -= Time.deltaTime;
            if (burstDelay <= 0)
            {
                GameObject projectileClone = Instantiate(projectile, transform.position + positionOffset, transform.rotation);
                projectileClone.GetComponent<Projectile>().TargetEnemy(targetEnemy);
                GameObject homingAttachment = new GameObject();
                homingAttachment.transform.position = projectileClone.transform.position;
                homingAttachment.AddComponent<Homing>();
                homingAttachment.GetComponent<Homing>().Init(projectileClone.GetComponent<Projectile>(), 150);

                projectileClone.GetComponent<Rigidbody>().velocity = direction * shotSpeed;


                burstCount--;
                burstDelay = maxBurstDelay;

                if (burstCount <= 0)
                {
                    burstCount = maxBurstCount;
                    cooldownTimer = maxCooldownTimer;
                    activateAttack = false;
                }
            }
        }
    }


    // GETTERS AND SETTERS

    public void SetAggressionThreshold(float threshold)
    {
        aggressionThreshold = threshold;
    }
    public void SetAggressionCooldown(float setLevel)
    {
        // Input a negative value if a negative decrement is desired
        aggressionDecrement = setLevel;
    }
    public float GetAggressionCooldown()
    {
        return aggressionDecrement;
    }
    public void SetDefenceThreshold(float threshold)
    {
        defenceThreshold = threshold;
    }
    public float GetAggressionThreshold()
    {
        return aggressionThreshold;
    }
    public float GetDefenceThreshold()
    {
        return defenceThreshold;
    }
    public void SetDefenceCooldown(float setLevel)
    {
        defenceDecrement = setLevel;
    }
    public float GetDefenceCooldown()
    {
        return defenceDecrement;
    }
}
