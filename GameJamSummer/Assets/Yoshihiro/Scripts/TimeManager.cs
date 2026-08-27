using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    // タイマーテキスト
    [SerializeField] TextMeshProUGUI m_TimeText;
    // 分数
    [SerializeField] float m_Minute;
    // 秒数
    [SerializeField] float m_Seconds;

    // トータル制限時間
    private float m_TotalTime;
    // 前回の秒数
    private float m_PrevSeconds;

    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // トータル時間を計算
        m_TotalTime = m_Minute * 60 + m_Seconds;
        m_PrevSeconds = -1.0f;
    }

    void Update()
    {
        // 制限時間が0秒以下なら何もしない
        if (m_TotalTime <= 0.0f)
        {
            m_TotalTime = 0.0f;
            return;
        }

        // トータル時間を減算
        m_TotalTime -= Time.deltaTime;
        if (m_TotalTime < 0.0f) m_TotalTime = 0.0f;

        // 残り時間から分・秒を算出
        m_Minute = (int)(m_TotalTime / 60);
        m_Seconds = (int)(m_TotalTime % 60);

        // 秒が変化したときだけテキストを更新
        if ((int)m_Seconds != (int)m_PrevSeconds)
        {
            m_TimeText.text = m_Minute.ToString("00") + ":" + ((int)m_Seconds).ToString("00");
        }
        m_PrevSeconds = m_Seconds;
    }
}