using System.Collections;
using JetBrains.Annotations;
// using Microsoft.Unity.VisualStudio.Editor;
using Unity.Properties;
using UnityEngine;

using UnityEngine.UI;

public class Player_Prototype : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private float hp;
    private float maxHp = 100f;
    private float knockbackPower = 5f;
    private Rigidbody2D rb;
    [SerializeField] private Image hpBar; //ui오브젝트 image컴포넌트 참조 변수
    [SerializeField]private float monsterDamage = 1;
    private float lastDamageTime = -1f;
    private float damageCooldown = 1f; // 데미지를 받고 다음 데미지까지의 딜레이(무적시간)

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
        if(other.CompareTag("Enemy"))
        {
            if(Time.time - lastDamageTime >= damageCooldown)
            {
                currentHp -= monsterDamage;
                Debug.Log(currentHp);

                Vector2 knockDir = (transform.position - other.transform.position).normalized;
                rb.AddForce(knockDir * knockbackPower, ForceMode2D.Impulse);

                lastDamageTime = Time.time;
            }
        }
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
    // 조건 1: 데미지를 입으면 무조건 1초의 무적시간이 부여된다.
    // 조건 1 구현: 데미지를 입은 시각을 변수(lastDamageTime)에 저장한 뒤, 현재 시각과 비교 했을 때 1초보다 같거나 클 경우에만 데미지를 다시 입히는 식으로 조건문 구성