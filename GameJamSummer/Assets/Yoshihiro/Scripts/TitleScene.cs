using UnityEngine.InputSystem;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        }
        else if(downArrowKey.wasPressedThisFrame)
        {
            TextManager.Instance.DownTextCount();
        }

        if(enterKey.wasPressedThisFrame)
        {
            // 決定
            if (TextManager.Instance.m_SelectTextCount == 0)
            {

            }
            else if (TextManager.Instance.m_SelectTextCount == 1)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                //Application.Quit();
            }
        }
       
    }
}
