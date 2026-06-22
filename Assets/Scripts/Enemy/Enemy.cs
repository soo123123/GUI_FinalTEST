using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Transform player;
    private Rigidbody2D rb;
    private Vector2 randomDirection; // 플레이어 무적동안 이동 방향
    private float randomMoveTimer; // 랜덤 이동 시간
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField] private int maxHp = 10;
    private int currentHp;
    [SerializeField] private Slider hpBar;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        currentHp = maxHp;

        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;

        // this //
        Debug.Log(hpBar.value);
    }

    void FixedUpdate()
    {
        if(player == null)
            return;
        
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

    // 주변 적 탐색
    Collider2D[] nearbyEnemies =
        Physics2D.OverlapCircleAll(transform.position, 1f);

    Vector2 separation = Vector2.zero;

    foreach (Collider2D enemy in nearbyEnemies)
    {
        if (enemy.gameObject == gameObject)
            continue;

        if (enemy.CompareTag("Enemy"))
        {
            separation +=
                ((Vector2)transform.position -
                 (Vector2)enemy.transform.position).normalized;
        }
    }

    Vector2 moveDir =
        (dir + separation * 0.7f).normalized;

    rb.MovePosition(
        rb.position +
        moveDir * moveSpeed * Time.fixedDeltaTime
    );
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

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        hpBar.value = currentHp;


        Debug.Log($"HP:{currentHp}");
        if(currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Score += 10;
        Destroy(gameObject);
    }
}
