using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingPad : MonoBehaviour
{
    public GameObject PlayerView;
    public GameObject PlayerHUD;
    private bool Landed = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerView == null)
        {
            PlayerView = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerView").transform.gameObject;
        }

        if (PlayerHUD == null)
        {
            PlayerHUD = GameObject.FindGameObjectWithTag("PlayerHUD");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Landed == false)
                {
                    Landed = true;
                    other.GetComponent<PlayerMove>().canMove = false;
                    other.transform.rotation = Quaternion.Slerp(other.transform.rotation, transform.rotation, 1f);
                    other.GetComponent<Rigidbody>().MovePosition(transform.position);
                    other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                }

                else
                {
                    Landed = false;
                    PlayerView.GetComponentInParent<FreeCamera2>().FreeRotate = false;
                    other.GetComponent<PlayerMove>().canMove = true;
                    other.GetComponent<Rigidbody>().MovePosition(transform.position + transform.up * 5f);
                } 
            }

            if (Landed == true)
            {
                //Bandaid fix for bug of missiles nudging the player:
                if (other.transform.rotation != transform.rotation)
                {
                    other.transform.rotation = transform.rotation;
                }

                if (other.transform.position != transform.position)
                {
                    other.transform.position = transform.position;
                }

                if (PlayerView.GetComponentInParent<FreeCamera2>().FreeRotate == false)
                {
                    PlayerView.GetComponentInParent<FreeCamera2>().FreeRotate = true;
                }
                
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
