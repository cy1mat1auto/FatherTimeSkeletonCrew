using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JockeyWeapons : MonoBehaviour
{
    public GameObject OneLaser;
    public GameObject PlayerView;
    private RaycastHit vision;
	private float rayLength;
    public static bool onObject;
    public static Vector3 laserEnd;
    public static Transform missileTarget;

    //For creating missiles:
    public GameObject ProjectileGeneric;
    public GameObject Port;
    private GameObject LoadedMissile = null;

    // Start is called before the first frame update
    void Start()
    {
		rayLength = 200;
        onObject = false;
        ProjectileGeneric = Resources.Load<GameObject>("ProjectilePortGen");
        ProjectileGeneric.GetComponent<MissileScript2>().jockey01 = gameObject;
        ProjectileGeneric.SetActive(false);

    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetMouseButtonDown(1))
        {

            LoadedMissile = GameObject.Instantiate(ProjectileGeneric, Port.transform.position, Port.transform.rotation);
            LoadedMissile.SetActive(true);
            if (onObject)
            {
                LoadedMissile.GetComponent<MissileScript2>().target = vision.collider.transform;
                LoadedMissile.GetComponent<MissileScript2>().homing = true;
            }


            //Debug.Log("NewMissileFired");
        }
    }
}
