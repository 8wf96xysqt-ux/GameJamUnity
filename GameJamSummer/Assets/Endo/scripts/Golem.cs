using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyBase
{
    [Header("生成するトラップ")]
    public GameObject trapPrefab;

    [Header("生成位置のリスト")]
    public List<Transform> spawnPointList;

    [Header("生成間隔")]
    [SerializeField]
    public float spawnInterval = 10.0f;

    [Header("攻撃開始からトラップ生成までの時間")]
    [SerializeField]
    private float spawnDelay = 2.0f;

    private float timer = 0f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Debug.Log("攻撃処理実行");

            animator.SetTrigger("IsAttack");
            StartCoroutine(SpawnTrapCoroutine());

            timer = 0f;
        }
    }

    private IEnumerator SpawnTrapCoroutine()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnTrap();
    }


    public void SpawnTrap()
    {
        // トラッププレハブが空
        if (trapPrefab == null)
        {
            Debug.Log("trapPrefabが設定されていません");
            return;
        }

        // 生成位置がない
        if (spawnPointList == null || spawnPointList.Count == 0)
        {
            Debug.Log("生成位置のリストが空です");
            return;
        }

        // 生成位置をランダムに1つ選ぶ
        int pointIndex = Random.Range(0, spawnPointList.Count);
        Transform spawnPoint = spawnPointList[pointIndex];

        // トラップを生成
        Instantiate(
            trapPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 使った生成位置を削除
        spawnPointList.RemoveAt(pointIndex);
    }
}
