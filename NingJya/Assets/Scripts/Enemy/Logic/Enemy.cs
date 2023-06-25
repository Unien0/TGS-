using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerDataの変数
    private float playerHp
    {
        //PlayerのHPをゲットする
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }//ダメージを与えた場合にPlayerの血量を修正する
    }
    private float playerDamage
    {
        //Playerのダメージ値を取得する
        get { if (playerData != null) return playerData.damage; else return 0; }

    }
    private float yellowForce
    {
        //黄色エリアの力
        get { if (playerData != null) return playerData.force; else return 0; }
    }
    private float whiteForce
    {
        //白色エリアの力
        get { if (playerData != null) return playerData.force * 4; else return 0; }
    }
    private float redForce
    {
        //赤エリアの力
        get { if (playerData != null) return playerData.force * 8; else return 0; }
    }
    public bool playerAttackable
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.attackable; else return false; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyDataの変数
    private float enemyHp
    {
        //enemyのhpをゲットする
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemySpeed
    {
        //enemyのhpをゲットする
        get { if (enemyData != null) return enemyData.Speed; else return 0; }
    }
    private float enemyDamage
    {
        //enemyのダメージをゲットする
        get { if (enemyData != null) return enemyData.damage; else return 0; }

    }
    private float blowTime
    {
        //吹っ飛ばすの状態に直すの時間
        get { if (enemyData != null) return enemyData.blowTime; else return 0; }

    }
    private float yellowExistenceTime
    {
        //黄色エリアでの保存時間
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }    
    private float whiteExistenceTime
    {
        //白色エリアでの保存時間
        get { if (enemyData != null) return enemyData.existenceTime*4; else return 0; }

    }
    private float redExistenceTime
    {
        //赤色エリアでの保存時間
        get { if (enemyData != null) return enemyData.existenceTime*8; else return 0; }

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
    }
    private bool beingBlow
    {
        //飛ばされているかどうかを判断する
        get { if (enemyData != null) return enemyData.beingBlow; else return false; }
    }
    #endregion

    private Rigidbody2D rb2d;
    private bool inPlayerAttackRange = false;
    private GameObject PlayerObject;
    private Vector2 moveint;
    // moveintの結果を正負のみの値にする
    private Vector2 moveit;
    // 行動までの待機時間
    private float actTime;
    // 移動処理の停止
    private bool stop = false;
    private bool fix = false;
    public Vector2 shotrote;
    private Vector2 shotIt;
    private bool shoted;//吹っ飛ばすの状態

    //今プレイヤーの攻撃範囲のどこにいる
    private enum currentAttackRange
    {
        Null,
        Red,
        white,
        yellow,
        end
    }

   private currentAttackRange area;

    //仮の吹っ飛ばす力
    private  float ForcePoint = 150;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {
        area = currentAttackRange.Null;
    }

    // Update is called once per frame
    void Update()
    {
        //吹っ飛ばすしない状態に移動する
        if (!shoted)
        {
            Move();
        }
        
        //Eキーを押した時、playerも攻撃できるなら
        if (Input.GetKeyDown(KeyCode.E) && playerAttackable)
        {
            Debug.Log("攻撃");
            shoted = true;
            BlowAway();
        }

        //吹っ飛ばすしたら、止まる時間を計算する
        if (shoted)
        {
            ToStop();
        }        
        //StopBlow();
    }

    /// <summary>
    /// 移動に関する
    /// </summary>
    private void Move()
    {
        if (removable)
        {
            moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
            moveit.x = Mathf.Sign(moveint.x);
            moveit.y = Mathf.Sign(moveint.y);
            rb2d.velocity = moveit * enemySpeed;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
        }
    }

    /// <summary>
    /// 吹っ飛ばすに関するプログラム
    /// </summary>
    private void BlowAway()
    {
        // プレイヤーの攻撃範囲にいるとき
        if (inPlayerAttackRange)
        {
            //方向
            shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
            shotIt.x = Mathf.Sign(shotrote.x);
            shotIt.y = Mathf.Sign(shotrote.y);

            //ーーーーーーーーー問題点（ForcePointはこれをゲットできない）ーーーーーーーーー
            //switch (area)
            //{
            //    case currentAttackRange.Red: ForcePoint = redForce; break;
            //    case currentAttackRange.white: ForcePoint = whiteForce; break;
            //    case currentAttackRange.yellow: ForcePoint = yellowForce; break;
            //}

            //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
            rb2d.AddForce(shotrote * ForcePoint);


            //ーーーーーーーーー調整のためにーーーーーーーーー
            //rb2d.AddForce(shotIt * ForcePoint, ForceMode2D.Impulse);

            //// 移動処理を行わないようにする
            //stop = true;
            //    fix = false;

            //    //2、敵の位置が黄色、白、赤のエリア内にあるかどうかを判断し、威力を変える。
            //    switch (area)
            //    {
            //        case currentAttackRange.Red: ForcePoint = redForce; break;
            //        case currentAttackRange.white: ForcePoint = whiteForce; break;
            //        case currentAttackRange.yellow: ForcePoint = yellowForce; break;
            //    }

            //    //3、プレイヤーとの位置によって吹っ飛ばす方向を決める
            //    if (!shoted)
            //    {
            //    Debug.Log(3);
            //        // ふっとばす
            //        shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
            //        shotIt.x = Mathf.Sign(shotrote.x);
            //        shotIt.y = Mathf.Sign(shotrote.y);

            //        //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
            //        rb2d.AddForce(shotIt * ForcePoint, ForceMode2D.Impulse);
            //    shoted = true;
            //}
        }
    }

    /// <summary>
    /// 吹っ飛ばす状態に止まるの時間
    /// </summary>
    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            actTime = 0;
            shoted = false;
        }
    }

    /// <summary>
    /// 吹っ飛ばすの状態に止まる
    /// </summary>
    private void StopBlow()
    {
        // ふっとばし停止処理
        // 攻撃入力がされていない上
        if (!playerAttackable)
        {
            stop = false;
            // 修正処理がされていないなら停止する。
            if (!fix)
            {
                transform.eulerAngles = Vector3.zero;
                fix = true;
                shoted = false;
                rb2d.velocity = Vector2.zero;
                rb2d.angularVelocity = 0;
            }
        }
    }


    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    // プレイヤーの攻撃範囲に入った場合
    //    if (col.CompareTag("Player"))
    //    {
    //        inPlayerAttackRange = true;

    //        if (col.gameObject.name == "Red")
    //        {
    //            area = currentAttackRange.Red;
    //        }
    //        else if (col.gameObject.name == "white")
    //        {
    //            area = currentAttackRange.white;
    //        }
    //        else if (col.gameObject.name == "yellow")
    //        {
    //            area = currentAttackRange.yellow;
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
            if (col.gameObject.name == "Red")
            {
                area = currentAttackRange.Red;
            }
            else if (col.gameObject.name == "white")
            {
                area = currentAttackRange.white;
            }
            else if (col.gameObject.name == "yellow")
            {
                area = currentAttackRange.yellow;
            }
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
