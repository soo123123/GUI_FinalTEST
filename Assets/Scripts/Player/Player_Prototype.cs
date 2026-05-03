using UnityEngine;

public class Player_Prototype : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 vec = new Vector3(x, y, 0);

        transform.position += vec * Time.deltaTime;
    }
}
