using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float MaxHealth;
    public float CurrentHealth;
    public float HealthBounds;


    // Start is called before the first frame update
    void Start()
    {
        HealthBounds = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        CurrentHealth = HealthBounds;
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
