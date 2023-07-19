using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //共有データ
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
    private float ForcePoint
    {
        //仮の力
        get { if (playerData != null) return playerData.force; else return 0; }
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
    private float ExistenceTime
    {
        //死体の保存時間
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

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

    private enum enemyActSet
    {
        move,
        shot,
        cannonball,
        end
    }
    //今プレイヤーの攻撃範囲のどこにいる
    private enum currentAttackRange
    {
        Null,
        Red,
        white,
        yellow,
        end
    }
    
    private float actTime;// 行動までの待機時間    
    [SerializeField] private float objctDistance;
    private float hp;

    //個体の状態（Bool）
    private bool inPlayerAttackRange = false;
    private bool isEnemyClash = false;
    private bool shotOk;
    private bool isShot;
    private bool shoted;//吹っ飛ばすの状態
    private bool hit;
    private bool stop = false;// 移動処理の停止
    private bool fix = false;
    [SerializeField] private bool exchange;

    private Rigidbody2D rb2d;
    enemyActSet enemyAct;
    private currentAttackRange area;

    private GameObject ClashEnemyObj;
    private GameObject PlayerObject;
    [SerializeField] private GameObject cannonball;
    [SerializeField] private GameObject sotBullet;

    private Vector2 clashRote;
    private Vector2 clashshotIt;
    private Vector2 moveint;    
    private Vector2 moveit;// moveintの結果を正負のみの値にする
    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;

    }

    void Start()
    {
        area = currentAttackRange.Null;
        hp = enemyHp;
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
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 4"))
        {
            exchange = !exchange;
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
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                objctDistance = Mathf.Sqrt(moveint.x * moveint.x + moveint.y * moveint.y);
                if (objctDistance <= 10)
                {
                    moveit.x = Mathf.Sign(moveint.x);
                    moveit.y = Mathf.Sign(moveint.y);
                    rb2d.velocity = moveit * enemySpeed;
                }
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
            if (exchange)
            {
                // 全敵の取得
                // 誘導範囲内の敵の取得
                // 誘導範囲内で、攻撃方向にある敵を取得
                // 上の敵取得の中から一番近くの敵を取得
                // このオブジェクトと目標オブジェクトの差を求め、アドフォース
            }
            else
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
                if (hp <= 0)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    actTime = 0;
                    shoted = false;
                }
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
                hp--;
            }
        }
        if (col.gameObject.CompareTag("HitObj"))
        {
            hit = true;
            hp--;
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
