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
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyDataの変数
    private float enemyHp
    {
        //enemyのhpをゲットする
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemyDamage
    {
        //enemyのダメージをゲットする
        get { if (enemyData != null) return enemyData.damage; else return 0; }

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
    private bool ATKAREA = false;
    private GameObject PlayerObject;
    private Vector2 moveint;
    // moveintの結果を正負のみの値にする
    private Vector2 moveit;
    // 行動までの待機時間
    private float actTime;
    // 移動処理の停止
    private bool stop = false;
    private bool fix = false;
    [SerializeField] private float speed;
    public Vector2 shotrote;
    private Vector2 shotIt;
    private bool shoted;

    private enum ATKAREATYPE
    {
        Null,
        Red,
        white,
        yellow,
        end
    }

    ATKAREATYPE AREA = ATKAREATYPE.Null;
    float ForcePoint = 0;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        // プレイヤーの攻撃範囲にいるとき
        if (ATKAREA)
        {
            // プレイヤーが攻撃入力をした時
            if (FindObjectOfType<Player>().ATK)
            {
                // 移動処理を行わないようにする
                stop = true;
                fix = false;
                if (!shoted)
                {
                    // ふっとばす
                    shoted = true;
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    rb2d.AddForce(shotrote * 10, ForceMode2D.Impulse);
                }

            }
        }

        // ふっとばし停止処理
        // 攻撃入力がされていない上
        if (FindObjectOfType<Player>().ATK == false)
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
        Move();

        BlowAway();
    }

    /// <summary>
    /// 移動に関する
    /// </summary>
    private void Move()
    {
        actTime = GameManeger.Tempo;

        // 二秒経過時
        if (actTime >= 2.0f)
        {
            // 停止処理が行われていないなら
            if (!stop)
            {
                // 移動する
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                moveit.x = Mathf.Sign(moveint.x);
                moveit.y = Mathf.Sign(moveint.y);
                rb2d.velocity = moveit * 5;

                // 停止・初期化する
                if (actTime >= 2.25f)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.angularVelocity = 0;
                    actTime = 0;
                }
            }
            else actTime = 0; ;
        }
    }

    /// <summary>
    /// 吹っ飛ばすに関するプログラム
    /// </summary>
    private void BlowAway()
    {
        // プレイヤーの攻撃範囲にいるとき
        if (ATKAREA)
        {
            //1、プレイヤーは攻撃したかを判断する
            if (FindObjectOfType<Player>().ATK == true)
            {
                // 移動処理を行わないようにする
                stop = true;
                fix = false;

                //2、敵の位置が黄色、白、赤のエリア内にあるかどうかを判断し、威力を変える。
                switch (AREA)
                {
                    case ATKAREATYPE.Red: ForcePoint = redForce; break;
                    case ATKAREATYPE.white: ForcePoint = whiteForce; break;
                    case ATKAREATYPE.yellow: ForcePoint = yellowForce; break;
                }

                //3、プレイヤーとの位置によって吹っ飛ばす方向を決める
                if (!shoted)
                {
                    // ふっとばす
                    shoted = true;
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);

                    //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * ForcePoint, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入った場合
        if (col.CompareTag("Player"))
        {
            ATKAREA = true;

            if (col.gameObject.name == "Red")
            {
                AREA = ATKAREATYPE.Red;
            }
            else if (col.gameObject.name == "white")
            {
                AREA = ATKAREATYPE.white;
            }
            else if (col.gameObject.name == "yellow")
            {
                AREA = ATKAREATYPE.yellow;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            if (col.gameObject.name == "Red")
            {
                AREA = ATKAREATYPE.Red;
            }
            else if (col.gameObject.name == "white")
            {
                AREA = ATKAREATYPE.white;
            }
            else if (col.gameObject.name == "yellow")
            {
                AREA = ATKAREATYPE.yellow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

    }
}
