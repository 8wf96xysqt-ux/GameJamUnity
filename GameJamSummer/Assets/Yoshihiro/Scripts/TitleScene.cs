using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private AudioClip CursolMove;
    [SerializeField]
    private AudioClip TapEnter;

    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TextManager.Instance == null) return;

        // キーの入力情報
        var currentKeyboard = Keyboard.current;
        // キーボードの入力が取得出来ないなら終了
        if (currentKeyboard == null) return;

        // Enterキーの入力情報を取得
        var enterKey = currentKeyboard.enterKey;
        // 上下矢印キーの入力情報を取得
        var upArrowKey = currentKeyboard.upArrowKey;
        var downArrowKey = currentKeyboard.downArrowKey;

        // テキスト選択
        if(upArrowKey.wasPressedThisFrame)
        {
            TextManager.Instance.UpTextCount();
            audioSource.PlayOneShot(CursolMove);
        }
        else if(downArrowKey.wasPressedThisFrame)
        {
            TextManager.Instance.DownTextCount();
            audioSource.PlayOneShot(CursolMove);
        }

        if(enterKey.wasPressedThisFrame)
        {
            // 決定
            if (TextManager.Instance.m_SelectTextCount == 0)
            {
                audioSource.PlayOneShot(TapEnter);
                FadeManager.Instance.LoadScene("PlayScene");
            }
            else if (TextManager.Instance.m_SelectTextCount == 1)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                //Application.Quit();
            }
        }
       
    }
}
