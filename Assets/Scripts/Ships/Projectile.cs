using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool targetEnemy = true;
    string[] targetList;
    float moveSpeed = 1f;
    float damage = 1f;
    float lifespan = 10f;


    // PLEASE WORK TO IGNORE LAYER WITH THE WAYPOINTS!!!!

    public void TargetEnemy(bool targetEnemy)
    {
        this.targetEnemy = targetEnemy;
        
        if(this.targetEnemy)
        {
            targetList = new string[2];
            targetList[0] = "Enemy";
            targetList[1] = "Boss";
            // List all the potential targets to look for
        }
        else
        {
            targetList = new string[2];
            targetList[0] = "Player";
            targetList[1] = "Finish";   // Temp
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TargetEnemy(targetEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;

        // Might act to disable instead of destroy
        if (lifespan <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        for(int i = 0; i < targetList.Length; i++)
        {
            if (collision.gameObject.tag == targetList[i])
                Destroy(gameObject);
        }
    }
}
