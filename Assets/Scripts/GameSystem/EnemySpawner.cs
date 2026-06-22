using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private float spawnTimer;
    private float spawnInterval = 2f;
    private float gameTime;
    private int spawnCount = 1;

    void Update()
    {
        gameTime += Time.deltaTime;
        spawnInterval = Mathf.Max(
            0.2f,
            2f - gameTime * 0.03f
        );
        spawnTimer -= Time.deltaTime;

        if(spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }

        spawnCount = 1 + Mathf.FloorToInt(gameTime/20f);
    }

    void SpawnEnemy()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        for(int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos;
            do
            {
                float x = UnityEngine.Random.Range(-18f, 18f);
                float y = UnityEngine.Random.Range(-18f, 18f);

                spawnPos = new Vector3(x, y, 0);
            } while(Vector3.Distance(spawnPos, player.position) < 5f);

            Instantiate(
                enemyPrefab,
                spawnPos,
                Quaternion.identity
            );
        }
    }

}