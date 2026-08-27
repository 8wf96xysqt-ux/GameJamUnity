using UnityEngine;
using TMPro;

// コインの回転と取得処理と取得した際のコインの獲得数を増やすスクリプト

public class Coin : MonoBehaviour
{
    private int countCoin = 0;

    [HideInInspector]
    public TextMeshProUGUI countCoinText;

    private void Start()
    {
        if (countCoinText == null)
        {
            countCoinText = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(); // TextMeshProUGUIコンポーネントを取得
        }
    }

    private void Update()
    {
        transform.Rotate(150 * Time.deltaTime, 0, 0); // コインを回転させる
    }

    void OnTriggerEnter(Collider collison) // コインに触れた時の処理
    {
        if (collison.CompareTag("Eraser"))
        {
            if (countCoin != 0)
            {
                countCoinText.text = "×" + countCoin.ToString();
            }

            Destroy(gameObject); // コインを消す 

            countCoin++; // 取得したコインの数を増やす

        }
    }

}
