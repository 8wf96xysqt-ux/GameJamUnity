using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserController : MonoBehaviour
{
    private PlayCamera m_Camera;

    public void Init(PlayCamera playCamera)
    {
        m_Camera = playCamera;
    }

    // 敵と当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        // 敵と当たったら消滅
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit!!!!!!!");

            // SEを再生
            EraserManager.Instance.PlaySE(SEType.Bump);
            // ヒットエフェクトを再生
            EraserManager.Instance.PlayEffect(this.transform.position);

            // カメラへ通知
            if(m_Camera != null)
            {
                m_Camera.ClearTarget();
            }

            // スポーンタイマーをスタート
            EraserManager.Instance.m_IsStartSpawnTimer = true;

            Destroy(this.gameObject);
        }
    }
}
