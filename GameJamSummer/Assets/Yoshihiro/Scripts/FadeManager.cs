using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    // フェード用画像
    [SerializeField]
    private CanvasGroup m_FadeCanvasGroup;

    // 暗転にかかる時間
    [SerializeField] private float m_FadeDuration = 1.0f;

    public static FadeManager Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeCoroutine(1.0f, 0.0f));
    }

   // シーンロード
   public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 暗転
        yield return StartCoroutine(FadeCoroutine(0.0f, 1.0f));

        SceneManager.LoadScene(sceneName);
        
        yield return StartCoroutine(FadeCoroutine(1.0f, 0.0f));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        float elapsed = 0.0f;
        m_FadeCanvasGroup.alpha = startAlpha;
        m_FadeCanvasGroup.blocksRaycasts = true;

        while (elapsed < m_FadeDuration)
        {
            elapsed += Time.deltaTime;
            m_FadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / m_FadeDuration);
            yield return null;
        }

        m_FadeCanvasGroup.alpha = endAlpha;
        m_FadeCanvasGroup.blocksRaycasts = (endAlpha > 0.99f);
    }
}
