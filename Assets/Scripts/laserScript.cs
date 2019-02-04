using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {
	public Transform startPoint;
	LineRenderer laserLine;
    private bool pressed;

	// Use this for initialization
	void Start () {
		laserLine = GetComponentInChildren<LineRenderer> ();
		laserLine.SetWidth (.2f, .2f);
        pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            pressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
        }
        if (pressed)
        {
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, JockeyWeapons.laserEnd);
        }
        else if (!pressed)
        {
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, startPoint.position);
        }

        if (Input.GetMouseButtonDown(1))
        {

        }
    }
}
