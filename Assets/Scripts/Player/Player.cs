using System.Collections;
using JetBrains.Annotations;
// using Microsoft.Unity.VisualStudio.Editor;
using Unity.Properties;
using UnityEngine;

using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private float hp;
    private float maxHp = 100f;
    private float knockbackPower = 5f;
    private Rigidbody2D rb;
    [SerializeField] private Image hpBar; // ui오브젝트 image컴포넌트 참조 변수
    [SerializeField]private float monsterDamage = 1;

    private bool isInvincible; // 데미지 함수 + 무적 적용 함수 bool값

    // 프로퍼티
    public float currentHp
    {
        get{return hp;}
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            hpBar.fillAmount = hp / maxHp;    
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = 100;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // 0. 위치에 대한 값을 수시로 변경하기 위해 Update()에 넣기
        // 1. 키보드 입력받기
        // 2. 벡터 값으로 넣기
        // 3. 벡터 값 기반으로 위치 좌표 변경
        // 4. 스피드 값 받아서 포지션에 입력하기 defalut == 10
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 vec = new Vector3(x, y, 0);

        transform.position += vec * Time.deltaTime * moveSpeed;
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && !isInvincible)
        {
            Debug.Log("함수 호출 전: "+currentHp);
            TaskDamage(monsterDamage, other.transform);
        }
    }

    void TaskDamage(float damage, Transform enemy) // 데미지 함수
    {
        currentHp -= damage;
        Debug.Log("데미지 함수: "+currentHp);
        Vector2 knockDir = (transform.position - enemy.position).normalized;

        //this
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockDir * knockbackPower, ForceMode2D.Impulse);
        //this

        StartCoroutine(InvincibleCoroutine());
    }

    IEnumerator InvincibleCoroutine()  // 무적 함수
    {
        isInvincible = true;

        Debug.Log("무적함수: "+currentHp);

        gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");
        yield return new WaitForSeconds(1f);

        gameObject.layer = LayerMask.NameToLayer("Default");

        isInvincible = false;
    }

}

// --- //
// 1. 이동 로직 구현
// 2. 플레이어 체력 및 UI 구현
    // Canvas - image 생성
    // Render Mode - World Space로 변경
    // Canvas를 Player의 자식으로 넣기
    // canvas, image 사이즈 조절
    // 프로퍼티 setter에 Mathf.Clamp() 사용 == hp 입력, 최소, 최대값 보정
    //fillAmount == UI이미지가 얼마나 채워져 있는지 나타내는 값 -> max값에 현재 채력을 나눠서 값을 계산
    // 이미지 체력바 객체에 실제 ui 이미지 넣기 == Image hpBar -> ui 컴포넌트 넣기
    // hp ui에 이미지 삽입
    // hp ui - image type = Filled && Fill Method = Horizontal && Fill Origin = Left로 설정 -> 이미지를 실제로 동작하게 하는 작업
// 3. 몹 플레이어 접촉 시 지속 피해
    // 조건 1: 적 콜라이더 닿으면 데미지 입기
    // 조건 2: 데미지와 동시에 무적시간 1초 부여
        // 구현 1: 트리거 안에 if (tag == enemy) than 데미지 + 무적시간 함수 넣기
        // 구현 2: 무적 함수에서 플레이어 레이어를 무적 레이어로 변경하고 1초의 딜레이를 준뒤 다시 기본 레이어로 변경.(유니티 설정에서 physics2d에서 레이어 콜라이션에서 무적 레이어는 enemy레이어 데미지 안받게 설정)