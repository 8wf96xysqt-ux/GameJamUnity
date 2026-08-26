using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        [Header("敵のPrefab")]
        public GameObject enemyPrefab;

        [Header("出現数")]
        [Min(1)]
        public int spawnCount = 1;
    }

    [System.Serializable]
    public class WaveData
    {
        [Header("このWaveで出す敵")]
        public EnemySpawnData[] enemies;

        [Header("このWaveで使うSpawn Point")]
        public Transform[] spawnPoints;
    }

    [Header("Wave設定")]
    [SerializeField] private WaveData[] waves;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Boss UI")]
    [SerializeField] private GameObject bossUI;
    [SerializeField] private Slider bossHpSlider;

    private int currentWave;
    private int enemiesAlive;

    private void Start()
    {
        if (bossUI != null)
        {
            bossUI.SetActive(false);
        }

        StartWave();
    }



    private void StartWave()
    {
        if (currentWave < waves.Length)
        {
            SpawnWave();
        }
        else
        {
            SpawnBoss();
        }
    }

    private void SpawnWave()
    {
        WaveData wave = waves[currentWave];

        // 敵の合計数
        enemiesAlive = 0;

        foreach (EnemySpawnData enemy in wave.enemies)
        {
            enemiesAlive += enemy.spawnCount;
        }

        // Spawn Pointをシャッフル
        List<Transform> availablePoints =
            new List<Transform>(wave.spawnPoints);

        Shuffle(availablePoints);

        int pointIndex = 0;

        foreach (EnemySpawnData enemy in wave.enemies)
        {
            for (int i = 0; i < enemy.spawnCount; i++)
            {
                if (enemy.enemyPrefab == null)
                {
                    Debug.LogWarning(
                        "Enemy Prefabが設定されていません"
                    );

                    continue;
                }

                if (pointIndex >= availablePoints.Count)
                {
                    Debug.LogWarning(
                        "Spawn Pointが足りません。"
                    );

                    return;
                }

                Transform spawnPoint =
                    availablePoints[pointIndex];

                Instantiate(
                    enemy.enemyPrefab,
                    spawnPoint.position,
                    spawnPoint.rotation
                );

                pointIndex++;
            }
        }

        Debug.Log(
            "Wave " + (currentWave + 1) +
            " 開始！ 敵数: " + enemiesAlive
        );
    }

    private void Shuffle(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex =
                Random.Range(i, list.Count);

            Transform temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;

        Debug.Log(
            "残り敵数：" + enemiesAlive
        );

        if (enemiesAlive <= 0)
        {
            currentWave++;

            StartWave();
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogWarning("Boss Prefabが設定されていません");
            return;
        }

        if (bossSpawnPoint == null)
        {
            Debug.LogWarning("Boss Spawn Pointが設定されていません");
            return;
        }

        // Bossを生成
        GameObject bossObject = Instantiate(
            bossPrefab,
            bossSpawnPoint.position,
            bossSpawnPoint.rotation
        );

        Debug.Log("Bossをスポーンしました");

        // Boss UIを表示
        if (bossUI != null)
        {
            bossUI.SetActive(true);
            Debug.Log("Boss UIをONにしました");
        }
        else
        {
            Debug.LogError("bossUIが設定されていません！");
        }

        // BossにPlayerとHPバーを渡す
        Boss boss = bossObject.GetComponent<Boss>();

        if (boss != null)
        {
            boss.Initialize(
                player,
                bossHpSlider
            );
        }
        else
        {
            Debug.LogError("Bossコンポーネントが見つかりません！");
        }
    }

}
