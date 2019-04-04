using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TargetList
{
    public static string[] allyTags = {"Player", "Finish"};
    public static string[] enemyTags = { "Enemy", "Boss" };

    public static string[] GetTargetTags(bool targetEnemies)
    {
        if (targetEnemies)
            return enemyTags;
        return allyTags;
    }
}


public class Projectile : MonoBehaviour
{
    bool targetEnemy = true;
    string[] targetList;
    float moveSpeed = 300;
    float damage = 1f;
    float lifespan = 10f;
    float damageRadius = 20f;

    // PLEASE WORK TO IGNORE LAYER WITH THE WAYPOINTS!!!!

    public void TargetEnemy(bool targetEnemy)
    {
        this.targetEnemy = targetEnemy;

        targetList = TargetList.GetTargetTags(targetEnemy);
    }
    public string[] GetTargetList()
    {
        return targetList;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
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

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            if (other.gameObject.tag == targetList[i])
                Destroy(gameObject);
        }
    }

}
