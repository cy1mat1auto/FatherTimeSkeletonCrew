using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHBar : MonoBehaviour
{
    public Camera PlayerView;
    public GameObject Holder;
    public Slider Bar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Holder.transform.LookAt(PlayerView.transform);
    }
}
