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


    protected bool targetEnemy = true;  // True means it targets enemies, False = targets player


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

    public void Init(bool targetEnemy, float cooldown, float power, float projectileSpeed, int burstCount, float burstDelay)
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
                homingAttachment.GetComponent<Homing>().Init(projectileClone.GetComponent<Projectile>(), 90);

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
}
