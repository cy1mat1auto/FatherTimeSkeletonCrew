using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[System.Serializable]

public class ShieldHealth : MonoBehaviour
{
    public double ShieldCapacity;
    private double Health;
    private double difference;
    public double ShieldRechargeRate;
    public float ShieldRechargeDelay;
    //public double SaveShield;

    // Start is called before the first frame update
    void Start()
    {
        ShieldCapacity = PlayerDatabase.shieldCapacityBase * PlayerDatabase.shieldCapacity;
        Health = gameObject.GetComponent<PlayerHealth>().CurrentHealth;
        ShieldRechargeRate = PlayerDatabase.shieldRechargeRate;
        ShieldRechargeDelay = 1f;
        InvokeRepeating("Recharge", 1, ShieldRechargeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        //if (GetComponent<SaveAll>() == null)
        //{
        //    ShieldCapacity = Mathf.Clamp((float)ShieldCapacity, 0, 100f);
        //}
        //
        //else if (GetComponent<SaveAll>().NewLoad)
        //{
        //   ShieldCapacity = PlayerDatabase.shieldCapacityBase * PlayerDatabase.shieldCapacity;
        //}

        //PlayerPrefs.SetFloat("ShieldCap", (float)ShieldCapacity);
        //PlayerPrefs.GetFloat("ShieldCap");

        ShieldCapacity = Mathf.Clamp((float)ShieldCapacity, 0, 100f);

        if (ShieldCapacity > 0)
        {
            if (Health != gameObject.GetComponent<PlayerHealth>().CurrentHealth)
            {
                difference = Health - gameObject.GetComponent<PlayerHealth>().CurrentHealth;
                if (difference > 0 && difference <= ShieldCapacity)
                {
                    ShieldCapacity -= difference;
                    gameObject.GetComponent<PlayerHealth>().CurrentHealth += difference;
                }
                else if (difference > 0 && difference > ShieldCapacity)
                {
                    gameObject.GetComponent<PlayerHealth>().CurrentHealth += difference;
                    gameObject.GetComponent<PlayerHealth>().CurrentHealth -= (difference - ShieldCapacity);
                    ShieldCapacity = 0;
                }
               
            }
        }
        Health = gameObject.GetComponent<PlayerHealth>().CurrentHealth;

    }
    void Recharge()
    {
        ShieldCapacity += ShieldRechargeRate;
        ShieldCapacity = Mathf.Clamp((float)ShieldCapacity, 0, 100f);
    }
}
