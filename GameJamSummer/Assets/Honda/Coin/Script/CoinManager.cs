using System.Collections.Generic;
using TMPro;
using UnityEngine;

// コインを時間差でランダム(リスト内からランダムの数選んで)に出現させ、一定時間が経ったら消えるスクリプト
// 取得した際に効果音を鳴らす処理(このスクリプト内で書く)

public class CoinManager : MonoBehaviour
{
    private List<Vector3> coinPositions = new List<Vector3>();

    public GameObject coinPrefab;

    public float displayDelay; // 表示までの待機時間（秒）

    private float timer;       // 経過時間を計測するタイマー 

    [HideInInspector]
    public TextMeshProUGUI countCoinText;

    public static CoinManager Instance { get; private set; }

    private AudioSource audioSource; // コイン取得時の効果音

    [SerializeField]
    private AudioClip coinSoundClip; // コイン取得時の効果音クリップ

    public int countCoin;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        countCoin = 0;

        timer = 0.0f;
        
        GameObject[] coinpoints = GameObject.FindGameObjectsWithTag("CoinPoint");

        foreach (GameObject coinpoint in coinpoints)
        {
            coinPositions.Add(coinpoint.transform.position);
        }

        if (countCoinText == null)
        {
            countCoinText = GameObject.Find("CoinCount").GetComponent<TextMeshProUGUI>(); // TextMeshProUGUIコンポーネントを取得
        }

        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得

    }


    void Update()
    {
        timer += Time.deltaTime;
        
        if(timer >= displayDelay)
        {
            timer = 0.0f;

            List<Vector3> tempPositions = new List<Vector3>(coinPositions);

            int SpawnCoinCount = Random.Range(1, 3); // 1から2までのランダムな数を生成

            for (int i = 0; i < SpawnCoinCount; i++)
            {
                if (tempPositions.Count > 0)
                {
                    int randomIndex = Random.Range(0, tempPositions.Count);
                    Vector3 randomPosition = tempPositions[randomIndex];

                    GameObject coin = Instantiate(coinPrefab);
                    coin.transform.position = randomPosition;

                    Destroy(coin, 3.0f);

                    tempPositions.RemoveAt(randomIndex); // 選ばれた位置をリストから削除
                }
            }
        }
    }

   public void PlaySE()
    {
        audioSource.PlayOneShot(coinSoundClip); // コイン取得時の効果音を再生
    }
}
