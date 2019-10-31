using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Collider Main;
    public Collider Crit1;
    public float MaxHealth;
    public float CurrentHealth;
    public float DespawnDelay = 1.5f;
    private float DeathTime;
    private bool Dead = false;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            if (!Dead)
            {
                DeathTime = Time.time;
                Destroy(GetComponent<EnemyHBar2>().OverallBar);
                GetComponent<EnemyAI_RL3>().SendMessage("EndBehavior");
                GetComponent<EnemyAI_RL3>().enabled = false;
                Dead = true;
            }

            else
            {
                if (Time.time - DeathTime >= DespawnDelay)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
