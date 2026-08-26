using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextManager : MonoBehaviour
{
    // テキスト
    [SerializeField] TextMeshProUGUI m_StartText;
    [SerializeField] TextMeshProUGUI m_EndText;
    
    // 選んでいるテキストのカウント
    public int m_SelectTextCount { get; private set; }
    private int m_PrevSelectTextCount = -1;

    // 点滅の間隔
    [SerializeField]
    private float m_WaitFlashTime = 0.0f;

    private Coroutine m_FlashCoroutine;

    public static TextManager Instance { get; private set; }

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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_SelectTextCount != m_PrevSelectTextCount)
        {
            if (m_FlashCoroutine != null) StopCoroutine(m_FlashCoroutine);

            // 選ばれなくなった方のテキストを不透明にリセット
            if (m_PrevSelectTextCount != -1)
            {
                var previous = (m_PrevSelectTextCount == 0) ? m_StartText : m_EndText;
                ResetAlpha(previous);
            }

            var target = (m_SelectTextCount == 0) ? m_StartText : m_EndText;
            ResetAlpha(target); // 新しく点滅を始める方も不透明から開始
            m_FlashCoroutine = StartCoroutine(Flashing(target));

            m_PrevSelectTextCount = m_SelectTextCount;
        }
    }

    IEnumerator Flashing(TextMeshProUGUI selectText)
    {
        const float step = 0.05f; // 1フレームあたりの変化量（お好みで調整）
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                var c = selectText.color;
                c.a = Mathf.Clamp01(c.a - step);
                selectText.color = c;
                yield return new WaitForSeconds(m_WaitFlashTime);
            }
            for (int j = 0; j < 20; j++)
            {
                var c = selectText.color;
                c.a = Mathf.Clamp01(c.a + step);
                selectText.color = c;
                yield return new WaitForSeconds(m_WaitFlashTime);
            }
        }
    }

    // 次のテキストにカウントを進める関数
    public void DownTextCount()
    {
        m_SelectTextCount++;

        if(m_SelectTextCount > 1)
        {
            m_SelectTextCount = 0;
        }
    }

    public void UpTextCount()
    {
        m_SelectTextCount--;

        if(m_SelectTextCount < 0)
        {
            m_SelectTextCount = 1;
        }
    }

    // アルファ値リセット関数
    private void ResetAlpha(TextMeshProUGUI text)
    {
        var c = text.color;
        c.a = 1f;
        text.color = c;
    }
}
