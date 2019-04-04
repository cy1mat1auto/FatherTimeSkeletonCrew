using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWP : MonoBehaviour
{
    //public SpriteRenderer Holder;
    public GameObject OnScreen;
    public Camera PlayerView;
    public Canvas HUD;
    public int Order;
    public bool Destination = false;
    public GameObject ThisPoint;
    public GameObject NextPoint;
    public GameObject Boss;

    // Start is called before the first frame update
    void Awake()
    {
        OnScreen.transform.SetParent(HUD.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Vector3.Angle(PlayerView.transform.forward, (ThisPoint.transform.position - PlayerView.transform.position))) < 50f)
        {
            OnScreen.transform.position = PlayerView.WorldToScreenPoint(ThisPoint.transform.position);
            //OverallBar.GetComponent<RectTransform>().pivot = new Vector2(-0.1f, 0);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Destination == false)
            {
                NextPoint.SetActive(true);
                Destroy(OnScreen);
                Destroy(gameObject);
            }

            else
            {
                Boss.GetComponent<EnemyHealth>().enabled = true;
                Boss.GetComponent<EnemyHBar2>().enabled = true;
                Boss.GetComponent<BossAIDemo>().enabled = true;
                Destroy(OnScreen);
                Destroy(gameObject);
            }
        }
    }
}
