using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploBarrel : MonoBehaviour
{
    public ParticleSystem ExploSource;
    public float health = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Collider [] InRadius = Physics.OverlapSphere(transform.position, 100f);
            Debug.Log(InRadius.Length);
            foreach(Collider hits in InRadius)
            {
                if(hits.CompareTag("Terrain"))
                {
                    Debug.Log(hits.name);
                    Destroy(GameObject.Find(hits.name));
                    Debug.Log("TerrainGone");
                }
            }
        }
    }
}
