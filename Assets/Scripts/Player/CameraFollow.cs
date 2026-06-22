using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if(player == null)
            return;

        transform.position = new Vector3(
            player.position.x,
            player.position.y,
            -10f
        );
    }
}