using UnityEngine;

public class EraserManager : MonoBehaviour
{
    // 消しゴムのPrefab
    public GameObject m_EraserPrefab;
    // スポーンポイント
    public GameObject m_SpawnPoint;

    // 消しゴムのリスポーンタイム
    private float m_SpawnTime = 2.0f;

    // スポーンタイマーフラグ
    public bool m_IsStartSpawnTimer { get; set; }
    // スポーンフラグ
    private bool m_IsSpawn;

    public static EraserManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ゲーム開始時にスポーン
        Spawn();

        // スポーンタイマーフラグをオフ
        m_IsStartSpawnTimer = false;
        // スポーンフラグをオフ
        m_IsSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 消しゴムが存在しているのか確認
        GameObject eraser = GameObject.FindWithTag("Eraser");

        // スポーンタイマー開始フラグがオンなら
        if(m_IsStartSpawnTimer)
        {
            SpawnTimer();
        }

        // もし消しゴムがなく、スポーンが可能なら
        if(eraser == null && m_IsSpawn)
        {
            // リスポーン
            Spawn();
            // スポーン可能フラグをオフ
            m_IsSpawn = false;
        }
    }

    // 生成関数
    void Spawn()
    {
        Vector3 spawnpoint = m_SpawnPoint.transform.position;
        Instantiate(m_EraserPrefab, spawnpoint, Quaternion.identity);
    }

    // スポーンタイマー関数
    void SpawnTimer()
    {
        // スポーンタイマーを進める
        m_SpawnTime -= Time.deltaTime;

        // スポーン可能な時間が経過すれば
        if(m_SpawnTime <= 0.0f)
        {
            m_IsSpawn = true;
            m_IsStartSpawnTimer = false;
            m_SpawnTime = 3.0f;
        }
    }
}
