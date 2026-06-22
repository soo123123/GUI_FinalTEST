using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bullet_speed = 10f;
    [SerializeField] private int damage = 1;
    void Update()
    {
        transform.position += transform.right * bullet_speed * Time.deltaTime;
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
