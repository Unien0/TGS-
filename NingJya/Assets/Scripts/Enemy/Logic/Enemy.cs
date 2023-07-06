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
    private bool isEnemyClash = false;
    private GameObject ClashEnemyObj;
    private Vector2 clashRote;
    private Vector2 clashshotIt;
    private GameObject PlayerObject;
    private Vector2 moveint;
    [SerializeField]private float objctDistance;
    // moveintの結果を正負のみの値にする
    private Vector2 moveit;
    // 行動までの待機時間
    private float actTime;
    // 移動処理の停止
    private bool stop = false;
    private bool fix = false;
    public Vector2 shotrote;
    [SerializeField]private Vector2 shotIt;
    private bool shoted;//吹っ飛ばすの状態
    private bool hit;

    private enum enemyActSet
    {
        move,
        shot,
        cannonball,
        end
    }
    enemyActSet enemyAct;
    private bool shotOk;
    [SerializeField] private GameObject cannonball;
    [SerializeField] private GameObject sotBullet;
    private bool isShot;

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
    private  float ForcePoint = 800;

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
        if (hit)
        {
            shoted = true;
            hit = false;
           HitBlow();
        }
        
        
        //Eキーを押した時、playerも攻撃できる上
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1") )
        {
            if (inPlayerAttackRange)
            {
                // ジャストアタックのタイミングなら
                if (blowable)
                {
                    shoted = true;
                    BlowAway();
                }
                else
                {
                    shoted = false;
                }
            }
        }

        //吹っ飛ばすしたら、止まる時間を計算する
        if (shoted)
        {
            ToStop();
        }        
        //StopBlow();

        switch (enemyAct)
        {
            case enemyActSet.move:
                break;
            case enemyActSet.shot:
                shotOk = true;
                break;
            case enemyActSet.cannonball:
                shotOk = true;
                rb2d.velocity = Vector3.zero;
                break;
        }

        if (shotOk)
        {
            if (enemyAct == enemyActSet.cannonball)
            {
                if (GameManeger.Tempo == 0)
                {
                    if (!isShot)
                    {
                        isShot = true;

                    }
                }
                else
                {
                    isShot = false;
                }
            }
            if (enemyAct == enemyActSet.shot)
            {
                if(GameManeger.Tempo == 1)
                {
                    if (!isShot)
                    {
                        isShot = true;
                        
                    }
                }
                else
                {
                    isShot = false;
                }
            }
        }
    }

    /// <summary>
    /// 移動に関する
    /// </summary>
    private void Move()
    {
        if (removable)
        {
            PlayerObject = FindObjectOfType<Player>().gameObject;
            //if (!isEnemyClash)
            {
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                objctDistance = Mathf.Sqrt(moveint.x * moveint.x + moveint.y * moveint.y);
                if (objctDistance <= 10)
                {
                    moveit.x = Mathf.Sign(moveint.x);
                    moveit.y = Mathf.Sign(moveint.y);
                    rb2d.velocity = moveit * enemySpeed;
                }

            }
        }
        else
        {
            //if (!isEnemyClash)
            {
                rb2d.velocity = Vector2.zero;
                rb2d.angularVelocity = 0;
            }
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
            if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
            {shotIt.x = Mathf.Sign(shotrote.x);}
            else
            {shotIt.x = 0;}
            if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
            {shotIt.y = Mathf.Sign(shotrote.y);}
            else
            {shotIt.y = 0;}
            //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
            rb2d.AddForce(shotIt * ForcePoint);
        }
    }

    void HitBlow()
    {
        //方向
        shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
        if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
        { shotIt.x = Mathf.Sign(shotrote.x); }
        else
        { shotIt.x = 0; }
        if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
        { shotIt.y = Mathf.Sign(shotrote.y); }
        else
        { shotIt.y = 0; }
        //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
        rb2d.AddForce(shotIt * ForcePoint);
    }

    /// <summary>
    /// 吹っ飛ばす状態に止まるの時間
    /// </summary>
    private void ToStop()
    {
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;

            if (actTime >= blowTime + 0.5f)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.GetComponent<Enemy>().shoted)
            {
                hit = true;
            }
        }
        if (col.gameObject.CompareTag("HitObj"))
        {
            Debug.Log("B");
            hit = true;
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = true;
            PlayerObject = FindObjectOfType<Player>().gameObject;
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
