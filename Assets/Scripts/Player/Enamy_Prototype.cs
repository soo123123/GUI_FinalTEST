using UnityEngine;

public class Enamy_Prototype : MonoBehaviour
{
    Transform player;
    [SerializeField]private float speed = 5f;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        enamy_working();
    }

    void enamy_working()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}

// --- //
// 1. player 태그를 가진 오브젝트의 transform값을 받음.
// 2. 받은 transform값으로 이동