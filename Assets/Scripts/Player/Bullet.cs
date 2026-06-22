using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bullet_speed = 10f;
    void Update()
    {
        transform.position += transform.right * bullet_speed * Time.deltaTime;
    }
}
