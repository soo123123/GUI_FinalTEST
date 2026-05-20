using UnityEngine;

public class Enemy_Prototype : MonoBehaviour
{
    Transform player;
    private Rigidbody2D rb;
    [SerializeField]private float moveSpeed = 5f;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        // transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        Vector2 dir = (player.position - transform.position).normalized;

        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }
}

// --- //
// 1. player 태그를 가진 오브젝트의 transform값을 받음.
// 2. 받은 transform값으로 이동