using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField]
    protected int maxHp = 10;

    [Header("ダメージBlink")]
    [SerializeField]
    protected float blinkDuration = 0.1f;

    [Header("死亡時ドロップ/エフェクト")]
    [SerializeField]
    protected GameObject deathObjectPrefab; 

    protected int currentHp;
    protected bool isDead = false;

    private Renderer[] renderers;
    private Coroutine blinkCoroutine;

    protected virtual void Start()
    {
        currentHp = maxHp;

        // Rendererを取得
        renderers = GetComponentsInChildren<Renderer>();

        Debug.Log("Renderer数: " + renderers.Length);
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;

        Debug.Log("ダメージ！ 現在HP: " + currentHp);

        // すでにBlink中ならリセット
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(DamageBlink());

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageBlink()
    {
        int blinkCount = 3;

        for (int i = 0; i < blinkCount; i++)
        {
            // 消す
            SetRenderers(false);

            yield return new WaitForSeconds(blinkDuration);

            // 表示
            SetRenderers(true);

            yield return new WaitForSeconds(blinkDuration);
        }

        blinkCoroutine = null;
    }


    private void SetRenderers(bool enabled)
    {
        if (renderers == null) return;

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        SetRenderers(true);
        if (deathObjectPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0f, 0.5f, 0f);
            Instantiate(deathObjectPrefab, spawnPosition, Quaternion.identity);
        }

        WaveManager waveManager = FindObjectOfType<WaveManager>();

        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }
        else
        {
            Debug.LogWarning("WaveManagerが見つかりません！");
        }

        Destroy(gameObject);
    }

}
