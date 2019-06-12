using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JockeyWeapons : MonoBehaviour
{
    public GameObject OneLaser;
    public GameObject PlayerView;
    private RaycastHit vision;
	private float rayLength;
    public static bool onObject;
    public static Vector3 laserEnd;
    public static Transform missileTarget;

    //For Regulating Missile system (HUD, fire rate and reload speed):
    public int Capacity = 4;
    public int NumberLoaded;
    public float ReloadSpeed = 1;
    public float FireRate = 1.5f;
    private float TimeLastFired;
    public Slider MissileStatus = null;
    private bool PortReady = true;

    //For creating missiles:
    public GameObject ProjectileGeneric;
    public GameObject Port;
    public GameObject Starboard;
    private GameObject LoadedMissile = null;

    // Start is called before the first frame update
    void Start()
    {
		rayLength = 200;
        onObject = false;
        ProjectileGeneric = Resources.Load<GameObject>("ProjectilePortGen");
        ProjectileGeneric.GetComponent<MissileScript2>().jockey01 = gameObject;
        ProjectileGeneric.SetActive(false);
        NumberLoaded = Capacity;
        TimeLastFired = 0;

        if (MissileStatus == null)
        {
            MissileStatus = GameObject.FindGameObjectWithTag("PlayerHUD").transform.Find("MissileStatus").GetComponent<Slider>();
        }

        MissileStatus.value = NumberLoaded;

        if (PlayerView == null)
        {
            PlayerView = gameObject.transform.Find("PlayerView").gameObject;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(PlayerView.transform.position, PlayerView.transform.forward * rayLength, Color.red, 2.0f);
        onObject = Physics.Raycast(PlayerView.transform.position, PlayerView.transform.forward * rayLength, out vision, rayLength);
        if (onObject)
        {
            laserEnd = vision.collider.transform.position;
            missileTarget = vision.collider.transform;
            if (vision.transform.tag == "Enemy" && OneLaser.GetComponent<laserScript>().pressed)
            {
                vision.transform.gameObject.GetComponent<EnemyHealth>().CurrentHealth -= 1;
            }

            //for hitting explosive barrels;
            else if (vision.transform.tag == "Explosive" && OneLaser.GetComponent<laserScript>().pressed)
            {
                vision.transform.gameObject.GetComponent<ExploBarrel2>().health -= 1;
            }
        }
        else
        {
            laserEnd = transform.forward * 100000;
        }

        if (Input.GetMouseButton(1) && NumberLoaded > 0 && Time.time - TimeLastFired >= FireRate)
        {
            TimeLastFired = Time.time;
            if (PortReady)
            {
                LoadedMissile = GameObject.Instantiate(ProjectileGeneric, Port.transform.position, Port.transform.rotation);
                LoadedMissile.SetActive(true);
                PortReady = false;
            }

            else
            {
                LoadedMissile = GameObject.Instantiate(ProjectileGeneric, Starboard.transform.position, Starboard.transform.rotation);
                LoadedMissile.SetActive(true);
                PortReady = true;
            }

            NumberLoaded -= 1;

            if (onObject)
            {
                LoadedMissile.GetComponent<MissileScript2>().target = vision.collider.transform;
                LoadedMissile.GetComponent<MissileScript2>().homing = true;
            }


            //Debug.Log("NewMissileFired");
        }

        if (Time.time - TimeLastFired >= ReloadSpeed && NumberLoaded != Capacity)
        {
            NumberLoaded += 1;
            TimeLastFired = Time.time;
        }

        MissileStatus.value = NumberLoaded;
    }
}
