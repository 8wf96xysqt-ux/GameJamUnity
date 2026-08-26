using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Boss : EnemyBase
{
    [Header("プレイヤー")]
    [SerializeField] private Transform player;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("弾")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("弾の発射角度")]
    [SerializeField] private float minAngle = 20f;
    [SerializeField] private float maxAngle = 70f;

    [Header("トラップ")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private List<Transform> spawnPointList;
    private bool trapSpawned;
    [Header("攻撃間隔")]
    [SerializeField] private float attackInterval = 5f;

    [Header("突進")]
    [SerializeField] private float rushSpeed = 10f;

    [Header("攻撃後の振り向き")]
    [SerializeField] private float turnSpeed = 3f;

    [Header("突進警告")]
    [SerializeField] private GameObject rushWarning;
    [SerializeField] private float warningTime = 1.5f;
    [Header("ボスHP")]
    [SerializeField] private Slider hpSlider;
    [Header("死亡")]
    [SerializeField] private string nextSceneName = "GameClear";
    private bool isDying;


    private float attackTimer;

    private List<int> attackOrder = new List<int>();
    private int attackIndex;
    private Vector3 rushDirection;
    private Vector3 savedPositionBeforeRush;

    private bool isAttacking;
    private bool isRushing;
    private bool isInvincible;

    private void Start()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        base.Start();

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;
        }


        attackTimer = attackInterval;

        if (rushWarning)
        {
            rushWarning.SetActive(false);
        }

        CreateAttackOrder();
    }


    private void CreateAttackOrder()
    {
        attackOrder.Clear();

        attackOrder.Add(0);
        attackOrder.Add(1);
        attackOrder.Add(2);

        for (int i = 0; i < attackOrder.Count; i++)
        {
            int randomIndex = Random.Range(i, attackOrder.Count);

            int temp = attackOrder[i];
            attackOrder[i] = attackOrder[randomIndex];
            attackOrder[randomIndex] = temp;
        }

        attackIndex = 0;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            LookAtPlayer();
        }

        if (isRushing)
        {
            RushMove();
            return;
        }

        if (isAttacking)
        {
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            StartRandomAttack();
        }
    }

    // プレイヤーの方向を向く
    private void LookAtPlayer()
    {
        if (!player)
        {
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    // 攻撃をランダムに選択
    private void StartRandomAttack()
    {
        isAttacking = true;

        if (attackIndex >= attackOrder.Count)
        {
            CreateAttackOrder();
        }

        int attackType = attackOrder[attackIndex];

        attackIndex++;

        Debug.Log("Boss Attack Type: " + attackType);

        switch (attackType)
        {
            case 0:
                StartFireBallAttack();
                break;

            case 1:
                StartTrapAttack();
                break;

            case 2:
                StartRushAttack();
                break;
        }
    }

    // ファイヤーボール攻撃開始
    private void StartFireBallAttack()
    {
        if (!bulletPrefab)
        {
            Debug.LogWarning("Bullet Prefabが設定されていません");
            EndAttack();
            return;
        }

        if (!bulletSpawnPoint)
        {
            Debug.LogWarning("Bullet Spawn Pointが設定されていません");
            EndAttack();
            return;
        }

        Debug.Log("Boss：ファイヤーボール攻撃開始");

        animator.SetTrigger("FireBall");
    }

    // Animation Eventから呼ぶ
    // ファイヤーボールを発射する瞬間に設定
    public void FireBall()
    {
        if (!bulletPrefab || !bulletSpawnPoint)
        {
            return;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
        );

        float angle = Random.Range(minAngle, maxAngle);

        if (Random.value < 0.5f)
        {
            angle *= -1f;
        }

        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        FireBullet fireBullet = bullet.GetComponent<FireBullet>();

        if (fireBullet)
        {
            fireBullet.Initialize(direction);
        }
        else
        {
            Debug.LogWarning(
                "BulletPrefabにFireBulletがありません"
            );
        }

        Debug.Log("Boss：ファイヤーボール発射");
    }

    // トラップ攻撃開始
    private void StartTrapAttack()
    {
        if (!trapPrefab)
        {
            Debug.LogWarning("trapPrefabが設定されていません");
            EndAttack();
            return;
        }

        if (spawnPointList == null || spawnPointList.Count == 0)
        {
            Debug.LogWarning("spawnPointListが空です");
            EndAttack();
            return;
        }

        trapSpawned = false;

        Debug.Log("Boss：トラップ攻撃開始");

        animator.SetTrigger("Trap");
    }

    // Animation Eventから呼ぶ
    // トラップを設置する瞬間に設定
    public void SpawnTrap()
    {
        if (trapSpawned)
        {
            return;
        }

        if (!trapPrefab)
        {
            return;
        }

        if (spawnPointList == null || spawnPointList.Count == 0)
        {
            return;
        }

        trapSpawned = true;

        int pointIndex = Random.Range(
            0,
            spawnPointList.Count
        );

        Transform spawnPoint = spawnPointList[pointIndex];

        Instantiate(
            trapPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Debug.Log("Boss：トラップ設置");
    }

    // 突進攻撃開始
    private void StartRushAttack()
    {
        if (!player)
        {
            Debug.LogWarning("Playerが設定されていません");
            EndAttack();
            return;
        }

        savedPositionBeforeRush = transform.position;

        rushDirection = player.position - transform.position;
        rushDirection.y = 0f;

        if (rushDirection.sqrMagnitude < 0.001f)
        {
            EndAttack();
            return;
        }

        rushDirection.Normalize();

        transform.forward = rushDirection;

        Debug.Log("Boss：突進警告開始");

        StartCoroutine(RushWarningCoroutine());

        isInvincible = true;
    }

    // 突進前の警告
    private IEnumerator RushWarningCoroutine()
    {
        if (rushWarning)
        {
            rushWarning.SetActive(true);
        }

        float timer = 0f;

        while (timer < warningTime)
        {
            timer += Time.deltaTime;

            if (rushWarning)
            {
                bool visible =
                    Mathf.FloorToInt(timer * 8f) % 2 == 0;

                rushWarning.SetActive(visible);
            }

            yield return null;
        }

        if (rushWarning)
        {
            rushWarning.SetActive(false);
        }

        Debug.Log("Boss：突進アニメーション開始");

        animator.SetTrigger("Rush");
    }

    // Animation Eventから呼ぶ
    // 突進モーションが始まる瞬間に設定
    public void RushStart()
    {
        Debug.Log("Boss：突進開始");

        isRushing = true;
    }

    // 突進中の移動
    private void RushMove()
    {
        transform.position +=
            rushDirection *
            rushSpeed *
            Time.deltaTime;
    }

    // Animation Eventから呼ぶ
    // 突進モーションが終わる瞬間に設定
    public void RushEnd()
    {
        Debug.Log("Boss：突進終了");

        isRushing = false;
       

        animator.SetTrigger("Return");
    }


    // Animation Eventから呼ぶ
    // 戻るモーションの中で設定
    public void ReturnPosition()
    {
        Debug.Log("Boss：元の位置へ戻る");

        transform.position = savedPositionBeforeRush;
    }

    // Animation Eventから呼ぶ
    // 攻撃アニメーションの最後に設定
    public void EndAttack()
    {
        isAttacking = false;
        isRushing = false;
        isInvincible = false;

        attackTimer = attackInterval;

        Debug.Log("Boss：攻撃終了");
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (isInvincible)
        {
            return;
        }

        TakeDamage(1);
    }

    public override void TakeDamage(int damage)
    {
        if (isDead || isDying) return;

        currentHp -= damage;

        if (hpSlider != null)
        {
            hpSlider.value = currentHp;
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }


    protected override void Die()
    {
        if (isDying) return;

        isDying = true;
        isDead = true;

        // 攻撃を停止
        isAttacking = false;
        isRushing = false;
        isInvincible = true;

        // 突進警告を消す
        if (rushWarning != null)
        {
            rushWarning.SetActive(false);
        }

        // 死亡アニメーション
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Debug.Log("Boss：死亡");
    }

    // 死亡アニメーションの最後にAnimation Eventで呼ぶ
    public void ChangeScene()
    {
        Debug.Log("Boss：死亡アニメーション終了 → シーンチェンジ");

        SceneManager.LoadScene(nextSceneName);
    }

}