using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    [Header("プレイヤー")]
    [SerializeField] private Transform player;

    [Header("弾")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("弾の発射角度")]
    [SerializeField] private float minAngle = 20f;
    [SerializeField] private float maxAngle = 70f;

    [Header("トラップ")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private List<Transform> spawnPointList;

    [Header("攻撃間隔")]
    [SerializeField] private float attackInterval = 5f;

    [Header("突進")]
    [SerializeField] private float rushSpeed = 10f;
    [SerializeField] private float rushDuration = 1.5f;
    [Header("攻撃後の振り向き")]
    [SerializeField] private float turnSpeed = 3f;

    [Header("突進警告")]
    [SerializeField] private GameObject rushWarning;
    [SerializeField] private float warningTime = 1.5f;

    private float attackTimer;

    // 突進方向
    private Vector3 rushDirection;

    // 攻撃中か
    private bool isAttacking;
    //突進前の場所保存
    private Vector3 savedPositionBeforeRush;

    private Animator animator;


    private void Start()
    {

        animator = GetComponent<Animator>();

        // 最初の攻撃まで
        attackTimer = attackInterval;

        // 警告を非表示
        if (rushWarning != null)
        {
            rushWarning.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isAttacking)
        {
            LookAtPlayer();
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



    private void LookAtPlayer()
    {
        if (player == null)
        {
            return;
        }

        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation  = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,turnSpeed * Time.deltaTime);
    }



    // 攻撃選択
    private void StartRandomAttack()
    {
        isAttacking = true;

        // 0～2
        int attackType = Random.Range(0, 3);

        switch (attackType)
        {
            case 0:
                Shoot();
                break;

            case 1:
                SpawnTrap();
                break;

            case 2:
                StartRushAttack();
                break;
        }
    }
    // 弾攻撃
    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet Prefabが設定されていません");
            EndAttack();
            return;
        }

        if (bulletSpawnPoint == null)
        {
            Debug.LogWarning("Bullet Spawn Pointが設定されていません");
            EndAttack();
            return;
        }

        // 弾生成
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
        );

        // 角度をランダム
        float angle = Random.Range(
            minAngle,
            maxAngle
        );

        // 左右ランダム
        if (Random.value < 0.5f)
        {
            angle *= -1f;
        }

        // XZ方向
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        // TestBullet取得
        FireBullet testBullet =
            bullet.GetComponent<FireBullet>();

        if (testBullet != null)
        {
            testBullet.Initialize(direction);
        }
        else
        {
            Debug.LogWarning(
                "BulletPrefabにTestBulletがありません"
            );
        }

        Debug.Log("Boss：弾攻撃");

        EndAttack();
    }
    // トラップ攻撃
    private void SpawnTrap()
    {
        if (trapPrefab == null)
        {
            Debug.LogWarning(
                "trapPrefabが設定されていません"
            );

            EndAttack();
            return;
        }

        if (spawnPointList == null ||
            spawnPointList.Count == 0)
        {
            Debug.LogWarning(
                "spawnPointListが空です"
            );

            EndAttack();
            return;
        }

        // ランダムな場所
        int pointIndex = Random.Range(
            0,
            spawnPointList.Count
        );

        Transform spawnPoint =
            spawnPointList[pointIndex];

        // トラップ生成
        Instantiate(
            trapPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        spawnPointList.RemoveAt(pointIndex);

        Debug.Log("Boss：トラップ攻撃");

        EndAttack();
    }
    // 突進
    private void StartRushAttack()
    {
        if (player == null)
        {
            Debug.LogWarning(
                "Playerが設定されていません"
            );

            EndAttack();
            return;
        }

        // プレイヤー方向
        rushDirection =
            player.position - transform.position;

        // Y方向を無視
        rushDirection.y = 0f;

        if (rushDirection == Vector3.zero)
        {
            EndAttack();
            return;
        }

        rushDirection.Normalize();

        // プレイヤー方向を向く
        transform.forward = rushDirection;
        //突進前の場所を保存
        savedPositionBeforeRush = transform.position;

        Debug.Log("Boss：突進警告開始");

        StartCoroutine(RushWarningCoroutine());
    }
    // 突進警告
    private IEnumerator RushWarningCoroutine()
    {
        if (rushWarning != null)
        {
            rushWarning.SetActive(true);
        }

        float timer = 0f;

        while (timer < warningTime)
        {
            timer += Time.deltaTime;

            // 点滅
            if (rushWarning != null)
            {
                bool visible =
                    Mathf.FloorToInt(timer * 8f) % 2 == 0;

                rushWarning.SetActive(visible);
            }

            yield return null;
        }

        // 警告を消す
        if (rushWarning != null)
        {
            rushWarning.SetActive(false);
        }

        Debug.Log("Boss：突進開始");

        // 突進
        StartCoroutine(RushCoroutine());
    }
    // 突進本体
    private IEnumerator RushCoroutine()
    {
        float timer = 0f;

        while (timer < rushDuration)
        {
            transform.position +=
                rushDirection *
                rushSpeed *
                Time.deltaTime;

            timer += Time.deltaTime;

            yield return null;
        }

        Debug.Log("Boss：突進終了");
        //保存しておいた場所をポジションに代入
        transform.position = savedPositionBeforeRush;

        EndAttack();
    }
    // 攻撃終了
    private void EndAttack()
    {
        isAttacking = false;

        attackTimer = attackInterval;
    }
}
