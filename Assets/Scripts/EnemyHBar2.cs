using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHBar2 : MonoBehaviour
{
    //Attach this script to the enemy, assign an empty gameobject with offset from model mesh to be "Enemy", and a UI slider to be "Bar".

    public Camera PlayerView;
    public Canvas HUD;
    public GameObject OverallBar;
    public Slider Bar;
    public GameObject Enemy;

    //These next values will dictate the healthbar's fill.
    public float MaxHealth;
    public float CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        Bar.transform.SetParent(HUD.transform);
        OverallBar.SetActive(false);
        MaxHealth = GetComponent<EnemyHealth>().MaxHealth;
        CurrentHealth = GetComponent<EnemyHealth>().CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Vector3.Angle(PlayerView.transform.forward, (transform.position - PlayerView.transform.position))) < 50f)
        {
            OverallBar.SetActive(true);
            CurrentHealth = GetComponent<EnemyHealth>().CurrentHealth;
            Bar.value = CurrentHealth / MaxHealth;
            Bar.transform.position = PlayerView.WorldToScreenPoint(Enemy.transform.position);
            OverallBar.GetComponent<RectTransform>().pivot = new Vector2(-0.1f, 0);
        }
        
        else
        {
            OverallBar.SetActive(false);
        }
    }
}
