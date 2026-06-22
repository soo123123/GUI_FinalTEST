using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    private Rigidbody2D rb;
    private Vector2 randomDirection; // 플레이어 무적동안 이동 방향
    private float randomMoveTimer; // 랜덤 이동 시간
    [SerializeField]private float moveSpeed = 5f;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(player.gameObject.layer == LayerMask.NameToLayer("InvinciblePlayer"))
        {
            RandomMove();
        }
        else
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    void RandomMove()
    {
        randomMoveTimer -= Time.fixedDeltaTime;

        if(randomMoveTimer <= 0)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            randomMoveTimer = 0.5f; // 하드코딩 나중에 수정할 것.(유틸 모듈에 합쳐도 될듯)
        }

        rb.MovePosition(rb.position + randomDirection * moveSpeed * 0.5f * Time.fixedDeltaTime);
    }
}

// --- //
// 1. player 태그를 가진 오브젝트의 transform값을 받음.
// 2. 받은 transform값으로 이동