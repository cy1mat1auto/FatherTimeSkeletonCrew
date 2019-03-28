using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileIconScript : MonoBehaviour
{
    public Sprite fullImage, emptyImage;
    public bool missileLoaded;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        if (missileLoaded)
            transform.GetComponent<Image>().sprite = fullImage;
        else
            transform.GetComponent<Image>().sprite = emptyImage;

    }
}
