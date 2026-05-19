using System.Collections;
using JetBrains.Annotations;
// using Microsoft.Unity.VisualStudio.Editor;
using Unity.Properties;
using UnityEngine;

using UnityEngine.UI;

public class Player_Prototype : MonoBehaviour
{
    private float hp;
    private float max_hp = 100f;
    [SerializeField] private Image hpBar; //ui오브젝트 image컴포넌트 참조 변수
    bool isTouching = false;
    [SerializeField]private float monster_damage = 1;

    // 프로퍼티
    public float property_hp
    {
        get{return hp;}
        set
        {
            hp = Mathf.Clamp(value, 0, max_hp);
            hpBar.fillAmount = hp / max_hp;    
        }
    }

    void Start()
    {
        property_hp = 100;
    }

    void Update()
    {
        Move();
    }

    public float speed = 10.0f;
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

        transform.position += vec * Time.deltaTime * speed;
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enamy"))
        {
            isTouching = true;
            StartCoroutine(DamageRoutine()); // 데미지 딜레이 함수 실행
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enamy"))
        {
            isTouching = false;
        }
    }

    IEnumerator DamageRoutine() // IEnumerator == 저장함수처럼 동작함
    {
        while(isTouching)
        {
            property_hp -= monster_damage;
            Debug.Log(property_hp);

            yield return new WaitForSeconds(1f); // 1초 대기
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
    // ontriggerenter - bool값
    // bool값으로 데미지 로직 관리
    // ontriggerexit - 빠져나가면 bool값 변경 및 데미지 로직 중단