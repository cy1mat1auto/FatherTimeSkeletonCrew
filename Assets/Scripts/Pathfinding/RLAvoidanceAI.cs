using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLAvoidanceAI : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject Goal;
    public float Range = 150f;
    private RaycastHit view, front;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        InvokeRepeating("Seeking", 0, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector3(0, 0, 250f), ForceMode.Force);

    if (Vector3.Angle(transform.forward, Goal.transform.position - transform.position) <= 45f)
        {
            if (view.transform.gameObject != Goal)
            {

                Vector3 ImmediateGoal = view.transform.position + view.collider.bounds.size.y * -transform.up * 1.3f;
                if (Vector3.Distance(transform.position, ImmediateGoal) > 1f)
                {
                    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ImmediateGoal - transform.position, transform.up), Time.deltaTime * 4f);
                }
            }

            else
            {
                rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Goal.transform.position - transform.position, transform.up), Time.deltaTime * 4f);
            }
        }

    else
        {
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Goal.transform.position - transform.position, transform.up), Time.deltaTime * 4f);
        }

    }

    void Seeking()
    {
        Physics.Raycast(transform.position, Goal.transform.position - transform.position, out view, Range);
        Physics.Raycast(transform.position, transform.forward, out front, Range);
    }
}
