using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float MaxHealth;
    public float CurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = PlayerDatabase.maxHealth;
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SaveAll>().NewLoad)
        {
            MaxHealth = PlayerDatabase.maxHealth;
            CurrentHealth = PlayerDatabase.currentHealth;
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
    }
}
