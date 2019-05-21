using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWP : MonoBehaviour
{
    public GameObject Jockey01;
    public int Order;
    public bool Terminal = false;
    public GameObject NextPoint;
    public GameObject Boss;

    // Start is called before the first frame update
    void Start()
    {
        if (Jockey01 == null)
        {
            Jockey01 = GameObject.FindGameObjectWithTag("Player");
        }

        if (Order == 1)
        {
            Jockey01.transform.Find("Pointer01").gameObject.GetComponent<GuideArrow>().Destination = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Terminal == false)
            {
                Jockey01.transform.Find("Pointer01").gameObject.GetComponent<GuideArrow>().Destination = NextPoint;
                Destroy(gameObject);
            }

            else
            {
                Boss.GetComponent<EnemyHealth>().enabled = true;
                Boss.GetComponent<EnemyHBar2>().enabled = true;
                Boss.GetComponent<BossAIDemo>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}
