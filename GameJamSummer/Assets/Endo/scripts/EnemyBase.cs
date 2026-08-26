using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField]
    protected int maxHp = 10;

    protected int currentHp;
    protected bool isDead = false;

    protected virtual void Start()
    {
        currentHp = maxHp;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        // WaveManagerに敵を倒したことを通知
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
