using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public GameObject Jockey;
    public Text HealthNumber;
    public double MaxHealth;
    public double CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        Jockey = GameObject.FindGameObjectWithTag("Player");
        CurrentHealth = Jockey.GetComponent<PlayerHealth>().CurrentHealth;
        MaxHealth = Jockey.GetComponent<PlayerHealth>().MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Jockey.GetComponent<SaveAll>().NewLoad)
        //{
        //    MaxHealth = PlayerDatabase.maxHealthBase * PlayerDatabase.maxHealth;
        //    CurrentHealth = PlayerDatabase.currentHealth;
        //}

        CurrentHealth = Jockey.GetComponent<PlayerHealth>().CurrentHealth;
        GetComponent<Slider>().value = (float) (CurrentHealth / MaxHealth);
        HealthNumber.text = CurrentHealth.ToString();
    }

}
