using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public EnemyData_SO enemyData;
    #region EnemyDataの変数
    private float blowTime
    {
        //吹っ飛ばすの状態に直すの時間
        get { if (enemyData != null) return enemyData.blowTime; else return 0; }
        set { enemyData.blowTime = value; }
    }
    private bool attackable
    {
        //攻撃の可否を判断する
        get { if (enemyData != null) return enemyData.attackable; else return false; }
    }
    private bool removable
    {
        //移動可能かどうかを判断する
        get { if (enemyData != null) return enemyData.removable; else return false; }
    }
    private bool blowable
    {
        //打ち飛べるかどうかを判断する
        get { if (enemyData != null) return enemyData.blowable; else return false; }
        set { enemyData.blowable = value; }
    }
    private bool beingBlow
    {
        //飛ばされているかどうかを判断する
        get { if (enemyData != null) return enemyData.beingBlow; else return false; }
    }
    #endregion

    private Rigidbody2D rb2d;
    private Collider2D col2d;
    private GameObject PlayerObject;
    private bool inPlayerAttackRange = false;
    private bool shoted;//吹っ飛ばすの状態
    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;
    private float ForcePoint = 700;
    private float actTime;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // ジャストアタックのタイミングの上
        if (blowable)
        {
            //Eキーを押した時、playerも攻撃できるなら
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1"))
            {
                if (inPlayerAttackRange)
                {
                    shoted = true;
                    BlowAway();
                    actTime = 0;
                    col2d.isTrigger = false;
                }
            }
        }
        else
        {
            shoted = false;
        }

        if (!shoted)
        {
            ToStop();
        }
    }

    private void BlowAway()
    {
        // プレイヤーの攻撃範囲にいるとき
        if (inPlayerAttackRange)
        {
            //方向
            shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
            if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
            {
                shotIt.x = Mathf.Sign(shotrote.x);
            }
            else
            {
                shotIt.x = 0;
            }

            if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
            {
                shotIt.y = Mathf.Sign(shotrote.y);
            }
            else
            {
                shotIt.y = 0;
            }

            //ーーーーーーーーー問題点（ForcePointはこれをゲットできない）ーーーーーーーーー
            //switch (area)
            //{
            //    case currentAttackRange.Red: ForcePoint = redForce; break;
            //    case currentAttackRange.white: ForcePoint = whiteForce; break;
            //    case currentAttackRange.yellow: ForcePoint = yellowForce; break;
            //}

            //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
            rb2d.AddForce(shotIt * ForcePoint);

        }
    }

    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            col2d.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("wall")) || (col.CompareTag("HitObj")))
        {
            col2d.isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = false;
        }
    }
}
