using UnityEngine;

// コインの回転と取得処理のスクリプト

public class Coin : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab; // 爆発エフェクトのプレハブ

    private void Update()
    {
        transform.Rotate(150 * Time.deltaTime, 0, 0); // コインを回転させる
    }

    void OnTriggerEnter(Collider collison) // コインに触れた時の処理
    {
        if (collison.CompareTag("Eraser"))
        {
            CoinManager.Instance.PlaySE();
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // 爆発エフェクトを生成
            Destroy(gameObject); // コインを消す 
        }
           
    }

}
