using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverScene : MonoBehaviour
{
    [SerializeField]
    private AudioClip Enter;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // キーの入力情報
        var currentKeyboard = Keyboard.current;
        // キーボードの入力が取得出来ないなら終了
        if (currentKeyboard == null) return;

        var spaceKey = currentKeyboard.spaceKey;

        if (spaceKey.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(Enter);
            FadeManager.Instance.LoadScene("TitleScene");
        }
    }
}
