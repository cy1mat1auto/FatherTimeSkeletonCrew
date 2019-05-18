using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploBarrel2 : MonoBehaviour
{
    public ParticleSystem explosion;
    public float health, damage;
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        explosion.Pause();
        timer = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            explosion.transform.position = transform.position;
            explosion.Play();
            transform.GetComponent<Renderer>().enabled = false;

            Collider[] inRadius = Physics.OverlapSphere(transform.position, 50f);
            foreach (Collider hits in inRadius)
            {
                if (hits.CompareTag("Terrain"))
                    Destroy(GameObject.Find(hits.name));
                else if (hits.CompareTag("Enemy"))
                    hits.gameObject.GetComponent<EnemyHealth>().CurrentHealth -= damage;
                else if (hits.CompareTag("Player"))
                    hits.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= damage;
                else if (hits.CompareTag("Explosive"))
                    hits.GetComponent<ExploBarrel2>().health -= 1;
            }

            Collider[] inRadius2 = Physics.OverlapSphere(transform.position, 20f);
            foreach (Collider hits in inRadius)
            {
                if (hits.CompareTag("Enemy"))
                    hits.gameObject.GetComponent<EnemyHealth>().CurrentHealth -= damage;
                else if (hits.CompareTag("Player"))
                    hits.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= damage;
            }
        }

        else if (health < 0)
        {
            transform.GetComponent<Renderer>().enabled = false;
        }
    }
}
