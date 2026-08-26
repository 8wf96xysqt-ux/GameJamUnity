using UnityEngine;
using System.Collections.Generic;

// コインを時間差でランダム(リスト内からランダムの数選んで)に出現させ、一定時間が経ったら消えるスクリプト

public class CoinManager : MonoBehaviour
{
    private List<Vector3> coinPositions = new List<Vector3>();

    public GameObject coinPrefab;

    public float displayDelay; // 表示までの待機時間（秒）

    private float timer;       // 経過時間を計測するタイマー 

    void Start()
    {
        timer = 0.0f;
        
        GameObject[] coinpoints = GameObject.FindGameObjectsWithTag("CoinPoint");

        foreach (GameObject coinpoint in coinpoints)
        {
            coinPositions.Add(coinpoint.transform.position);
        }

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
}
