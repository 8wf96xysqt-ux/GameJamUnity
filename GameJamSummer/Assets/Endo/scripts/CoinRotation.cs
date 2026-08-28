using TMPro;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    private static int countCoin = 0;

    [HideInInspector]
    public TextMeshProUGUI countCoinText;

    [SerializeField]
    GameObject explosionPrefab; // 爆発エフェクトのプレハブ

    private void Start()
    {
        if (countCoinText == null)
        {
            GameObject coinCountObject = GameObject.Find("CoinCount");

            if (coinCountObject != null)
            {
                countCoinText = coinCountObject.GetComponent<TextMeshProUGUI>();
            }
        }

        UpdateCoinText();
    }

    private void Update()
    {
        transform.Rotate(0, 150 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Eraser"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // 爆発エフェクトを生成

            countCoin++;

            UpdateCoinText();

            CoinManager.Instance.PlaySE();

            Destroy(gameObject);
        }
    }

    private void UpdateCoinText()
    {
        if (countCoinText != null)
        {
            countCoinText.text = "×" + countCoin.ToString();
        }
    }
}
