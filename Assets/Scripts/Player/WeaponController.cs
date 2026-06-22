using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Transform target;
    [SerializeField]private GameObject bullet_prefab;
    [SerializeField]private float attackSpeed = 0.5f;
    private float attackTimer;
    void Update()
    {
        FindClosestEnemy();

        if(target != null)
        {
            RotateToTarget();
            attackTimer -= Time.deltaTime;

            if(attackTimer <= 0)
            {
                BulletShoot();
                attackTimer = attackSpeed;
            }
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 적 객체 배열

        float closestDistance = Mathf.Infinity; // 초기값 = 무한
        Transform closestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if(distance < closestDistance) // 최소 거리 = 저장
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        target = closestEnemy;
    }

    void RotateToTarget()
    {
        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    void BulletShoot()
    {
        Instantiate(bullet_prefab, transform.position, transform.rotation);
    }
}
