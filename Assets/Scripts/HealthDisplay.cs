using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public GameObject Jockey;
    public Text HealthNumber;
    public float MaxHealth;
    public float CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = Jockey.GetComponent<PlayerHealth>().CurrentHealth;
        MaxHealth = Jockey.GetComponent<PlayerHealth>().MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<SaveAll>().NewLoad)
        {
            MaxHealth = PlayerDatabase.maxHealth;
        }
        CurrentHealth = Jockey.GetComponent<PlayerHealth>().CurrentHealth;
        GetComponent<Slider>().value = CurrentHealth / MaxHealth;
        HealthNumber.text = CurrentHealth.ToString();
    }
}
