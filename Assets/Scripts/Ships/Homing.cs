using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maybe turn Homing class into a buff
public class Homing : MonoBehaviour
{
    bool targets = false;
    GameObject target;
    int radius = 5;
    string[] targetTags;
    Projectile parent;


    public void Init(Projectile parent, int setRadius = 5)
    {
        this.parent = parent;
        radius = setRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<SphereCollider>() == null)
            gameObject.AddComponent<SphereCollider>();

        gameObject.GetComponent<SphereCollider>().radius = radius;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        if (parent != null)
            gameObject.transform.SetParent(parent.transform);
        else
            Destroy(gameObject);

        SelectTarget(targets);
    }

    public void SelectTarget(bool targetEnemies)
    {
        targets = targetEnemies;
        targetTags = TargetList.GetTargetTags(targets);
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            parent.GetComponent<Rigidbody>().velocity = (target.transform.position - transform.position).normalized * parent.GetMoveSpeed();
            //parent.transform.position = Vector3.MoveTowards(parent.transform.position, target.transform.position, parent.GetMoveSpeed());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < targetTags.Length; i++)
        {
            if ((other.gameObject.tag == targetTags[i]) && (target == null))
            {
                float newTargetDistance = Mathf.Pow(other.gameObject.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(other.gameObject.transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(other.gameObject.transform.position.z - gameObject.transform.position.z, 2);
                float oldTargetDistance = newTargetDistance + 1;
                if (target != null)
                    oldTargetDistance = Mathf.Pow(target.gameObject.transform.position.x - gameObject.transform.position.x, 2) + Mathf.Pow(target.gameObject.transform.position.y - gameObject.transform.position.y, 2) + Mathf.Pow(target.gameObject.transform.position.z - gameObject.transform.position.z, 2);

                if (newTargetDistance < oldTargetDistance)
                    target = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
            target = null;
    }





}
