using UnityEngine;


public class Coin : MonoBehaviour
{
    private int countCoin;

    private void Update()
    {
        transform.Rotate(150 * Time.deltaTime, 0, 0); // コインを回転させる
    }

    void OnTriggerEnter(Collider collison)
    {
        //if (collison.CompareTag("Player"))
        //{
        //    Destroy(gameObject); // コインを消す
        //    Debug.Log("コインを取得しました");
        //    countCoin++; // 取得したコインの数を増やす
    }
}
