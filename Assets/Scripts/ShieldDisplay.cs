using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour
{

    public GameObject Jockey;
    public Text ShieldNumber;
    public double ShieldCapacity;
 
    // Start is called before the first frame update
    void Start()
    {
        Jockey = GameObject.FindGameObjectWithTag("Player");
        ShieldCapacity = Jockey.GetComponent<ShieldHealth>().ShieldCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        ShieldCapacity = Jockey.GetComponent<ShieldHealth>().ShieldCapacity;
        GetComponent<Slider>().value = (float)(ShieldCapacity / 100);
        ShieldNumber.text = ShieldCapacity.ToString();
    }
}
