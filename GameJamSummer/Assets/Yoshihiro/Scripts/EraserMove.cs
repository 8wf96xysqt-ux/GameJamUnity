using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class EraserMove : MonoBehaviour
{
    private Rigidbody m_rb;
    private ConfigurableJoint m_joint;

    // 後ろに引ける最大距離
    [SerializeField]
    private float m_MaxPullDistance = 0.0f;
    // 戻ろうとする力の強さ
    [SerializeField]
    private float m_SpringPower = 0.0f;
    // 引ける最大距離に収束するまでの減衰量
    [SerializeField]
    private float m_Damping = 0.0f;
    // 引く向き
    Vector3 m_PullDirection;

    // 後ろに引く力
    [SerializeField]
    private float m_BackForce = 0.0f;
    // 前に飛ばす力
    [SerializeField]
    private float m_ForwardForce = 0.0f;

    // 引っ張っているのかフラグ
    private bool m_IsPulling = false;
    // 発射フラグ
    private bool m_IsShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_IsShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        // キーの入力情報
        var currentKeyboard = Keyboard.current;
        // キーボードの入力が取得出来ないなら終了
        if (currentKeyboard == null) return;

        // Zキーの入力情報を取得
        var zKey = currentKeyboard.zKey;
        // Zキーを押している間引っ張って離したら発射
        if (zKey.wasPressedThisFrame && !m_IsShoot)
        {
            // 引っ張りを開始
             StartPull();
            // 引っ張りフラグをオン
             m_IsPulling = true;
        }
        else if (zKey.wasReleasedThisFrame)
        {
            // 離す
            Release();
            // 引っ張りフラグをオフ
            m_IsPulling = false;
            m_IsShoot = true;
        }

        // 左右移動
        if(!m_IsPulling)
        {
            Move(currentKeyboard);
        }
    }

    private void FixedUpdate()
    {
        // 引っ張りフラグがオンなら引っ張り継続関数を呼ぶ
        if (m_IsPulling)
        {
            Pulling();
        }
    }

    // 移動関数
    void Move(Keyboard current)
    {
        // 左右の矢印キーの入力情報を取得
        var leftArrow = current.leftArrowKey;
        var rightArrow = current.rightArrowKey;

        // 左右移動
        if(leftArrow.isPressed)
        {
            this.transform.position = this.transform.position + new Vector3(-0.01f, 0.0f, 0.0f);
        }
        else if(rightArrow.isPressed)
        {
            this.transform.position = this.transform.position + new Vector3(0.01f, 0.0f, 0.0f);
        }

    }

    // 引っ張り開始関数
    void StartPull()
    {
        // ConfigurableJointを取得
        m_joint = this.gameObject.AddComponent<ConfigurableJoint>();
        // 取得できないなら終了
        if (m_joint == null) return;

        Debug.Log("get joint");

        EraserManager.Instance.PlaySE(SEType.Pulling);

        // 接続するアンカーを自動では設定させない
        m_joint.autoConfigureConnectedAnchor = false;
        // アンカーの位置を設定
        m_joint.connectedAnchor = this.transform.position;

        // 動ける軸を設定

        // X、Y軸を移動は固定
        m_joint.xMotion = ConfigurableJointMotion.Locked;
        m_joint.yMotion = ConfigurableJointMotion.Locked;

        // Z軸の移動は制限付きで可能
        m_joint.zMotion = ConfigurableJointMotion.Limited;
        m_joint.linearLimit = new SoftJointLimit { limit = m_MaxPullDistance };
        m_joint.linearLimitSpring = new SoftJointLimitSpring { spring = m_SpringPower, damper = m_Damping };

        // 回転は全ての軸でロック       
        m_joint.angularXMotion = ConfigurableJointMotion.Locked;
        m_joint.angularYMotion = ConfigurableJointMotion.Locked;
        m_joint.angularZMotion = ConfigurableJointMotion.Locked;

        m_PullDirection = -this.transform.forward;
    }

    // 引っ張り継続関数
    void Pulling()
    {
        // ジョイントがなければ終了
        if (m_joint == null) return;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        // 後ろに引き続ける
        m_rb.AddForce(m_PullDirection* m_BackForce);
    }

    // リリース関数
    void Release()
    {
        // ジョイントを削除
        Destroy(m_joint);
        // 現在の速度ベクトルを0に
        m_rb.linearVelocity = Vector3.zero;
        EraserManager.Instance.PlaySE(SEType.Shoot);
        // 前に向かって発射
        m_rb.AddForce(this.transform.forward * m_ForwardForce, ForceMode.Impulse);
    }
}
