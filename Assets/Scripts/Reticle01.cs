using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle01 : MonoBehaviour
{
    public GameObject target;
    public Camera PlayerView;
    public Canvas HUD;
    public GameObject reticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reticle.transform.position = PlayerView.WorldToScreenPoint(target.transform.position);
    }
}
