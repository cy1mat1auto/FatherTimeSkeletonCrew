using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public double MaxHealth;
    public double CurrentHealth;


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
        CurrentHealth = Mathf.Clamp((float) CurrentHealth, 0, (float) MaxHealth);
    }
}
