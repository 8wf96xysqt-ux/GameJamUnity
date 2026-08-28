using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// SEの種類
public enum SEType
{
    Pulling,
    Shoot,
    Bump
}

public class EraserManager : MonoBehaviour
{
    // 消しゴムのPrefab
    public GameObject m_EraserPrefab;
    // スポーンポイント
    public GameObject m_SpawnPoint;

    // プレイカメラ
    [SerializeField]
    private PlayCamera m_Camera;

    // SE
    [SerializeField]
    private AudioClip Pulling;
    [SerializeField]  
    private AudioClip Shoot;
    [SerializeField]
    private AudioClip Bump;

    // Effect
    [SerializeField]
    private GameObject m_HitEffectPrefab;
    [SerializeField]
    private float m_EffectOffsetY;


    //SEの種類とAudioClipの対応
    [System.Serializable]
    private struct SEData
    {
        public SEType Type;
        public AudioClip Clip;
    }

    [SerializeField] 
    private List<SEData> m_SeList;
    private Dictionary<SEType,AudioClip> m_SeDictionary;

    AudioSource audioSource;

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

        // リストから辞書を作成
        m_SeDictionary = new Dictionary<SEType, AudioClip> ();

        foreach(var data in m_SeList)
        {
            m_SeDictionary[data.Type] = data.Clip;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // オーディオソースを取得
        audioSource = GetComponent<AudioSource>();
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
        // 消しゴムを生成
        GameObject eraser = Instantiate(m_EraserPrefab, spawnpoint, Quaternion.identity);
        // カメラに追従対象を通知
        m_Camera.SetTarget(eraser.transform, true);
        // 消しゴム自身にも「消えるときに使うカメラ」の参照を渡す
        EraserController controller = eraser.GetComponent<EraserController>();
        controller.Init(m_Camera);
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

    // SE再生関数
    public void PlaySE(SEType type)
    {
        if(m_SeDictionary.TryGetValue(type, out AudioClip clip) && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SE再生失敗: {type} / 辞書に存在={m_SeDictionary.ContainsKey(type)}");
        }
    }

    // エフェクト再生関数
    public void PlayEffect(Vector3 position)
    {
        position.y += m_EffectOffsetY;
        Instantiate(m_HitEffectPrefab, position, Quaternion.identity);
    }
    
}
