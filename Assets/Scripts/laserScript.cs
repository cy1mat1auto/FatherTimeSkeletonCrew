using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class laserScript : MonoBehaviour {
	public Transform startPoint;
	LineRenderer laserLine;
    public bool pressed;

    public Slider laserCharge;
    public float depleatSpeed = 0.001f, reloadSpeed = 0.0003f;

	// Use this for initialization
	void Start () {
		laserLine = GetComponentInChildren<LineRenderer> ();
		laserLine.SetWidth (0.2f, 0.2f);
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
            if (laserCharge.value > 0)
                laserCharge.value -= depleatSpeed;
        }
        else
            laserCharge.value += reloadSpeed;
        if (pressed && laserCharge.value > 0.05)
        {
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, JockeyWeapons.laserEnd);
        }
        else if (!pressed || laserCharge.value <= 0.05)
        {
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, startPoint.position);
        }
    }
}
