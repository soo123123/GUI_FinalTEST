using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Transform target;

    void Update()
    {
        FindClosestEnemy();

        if(target != null)
        {
            RotateToTarget();
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
}
