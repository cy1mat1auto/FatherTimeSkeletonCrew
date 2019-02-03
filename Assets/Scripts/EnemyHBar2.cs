using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHBar2 : MonoBehaviour
{
    //Attach this script to the enemy, assign the itself to be Enemy, and a UI slider to be Bar.

    public Camera PlayerView;
    public Canvas HUD;
    public GameObject OverallBar;
    public Slider Bar;
    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        Bar.transform.SetParent(HUD.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.GetComponentInParent<Renderer>().isVisible)
        {
            OverallBar.SetActive(true);
            Bar.transform.position = PlayerView.WorldToScreenPoint(Enemy.transform.position);
        }
        
        else
        {
            OverallBar.SetActive(false);
        }
    }
}
