using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public GameObject Pickup;
    public float Healing = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerHealth>().CurrentHealth != other.GetComponent<PlayerHealth>().MaxHealth)
        {
            other.GetComponent<PlayerHealth>().CurrentHealth += Healing;
            Destroy(Pickup);
        }
    }

}
