using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    private float playerHp
    {
        //playerDataの中にhpをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }//playをあたらplayerのHPを減らす
    }
    private float playerDamage
    {
        //playerDataの中にdamageをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.damage; else return 0; }

    }
    private float yellowForce
    {
        //リズムを合わない、黄色エリアのとき
        get { if (playerData != null) return playerData.force; else return 0; }
    }
    private float whiteForce
    {
        //リズム合うとき
        get { if (playerData != null) return playerData.force * 4; else return 0; }
    }
    private float redForce
    {
        //赤エリアのとき
        get { if (playerData != null) return playerData.force * 8; else return 0; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyData変数
    private float enemyHp
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemyDamage
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.damage; else return 0; }
        set { enemyData.damage = value; }
    }
    private float yellowExistenceTime
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }
    //リズムを合わせて敵を殺すれば4倍の存在時間があります
    private float whiteExistenceTime
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.existenceTime*4; else return 0; }

    }
    //赤エリアで敵を殺すれば8倍の存在時間があります
    private float redExistenceTime
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.existenceTime*8; else return 0; }

    }


    private bool attackable
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.attackable; else return false; }
    }
    private bool removable
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.removable; else return false; }
    }
    private bool blowable
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    private bool beingBlow
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.beingBlow; else return false; }
    }
    #endregion


    private Rigidbody2D rb2d;
    private bool ATKAREA = false;
    private GameObject PlayerObject;
    private Vector2 moveint;
    // moveint偺寢壥傪惓晧偺傒偺抣偵偡傞
    private Vector2 moveit;
    // 峴摦傑偱偺懸婡帪娫
    private float actTime;
    // 堏摦張棟偺掆巭
    private bool stop = false;
    private bool fix = false;
    [SerializeField] private float speed;
    public Vector2 shotrote;
    private Vector2 shotIt;
    private bool shoted;

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
        actTime += Time.deltaTime;

        // 擇昩宱夁帪
        if (actTime >= 2.0f)
        {
            // 掆巭張棟偑峴傢傟偰偄側偄側傜
            if (!stop)
            {
                // 堏摦偡傞
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                moveit.x = Mathf.Sign(moveint.x);
                moveit.y = Mathf.Sign(moveint.y);
                rb2d.velocity = moveit * 5;

                // 掆巭丒弶婜壔偡傞
                if (actTime >= 2.25f)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.angularVelocity = 0;
                    actTime = 0;
                }
            }
            else actTime = 0; ;
        }

        // 僾儗僀儎乕偺峌寕斖埻偵偄傞偲偒
        if (ATKAREA)
        {
            // 僾儗僀儎乕偑峌寕擖椡傪偟偨帪
            if (FindObjectOfType<Player>().ATK)
            {
                // 堏摦張棟傪峴傢側偄傛偆偵偡傞
                stop = true;
                fix = false;
                if (!shoted)
                {
                    // 傆偭偲偽偡
                    shoted = true;
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    rb2d.AddForce(shotrote * 10, ForceMode2D.Impulse);
                }

            }
        }

        // 傆偭偲偽偟掆巭張棟
        // 峌寕擖椡偑偝傟偰偄側偄忋
        if (FindObjectOfType<Player>().ATK == false)
        {
            stop = false;
            // 廋惓張棟偑偝傟偰偄側偄側傜掆巭偡傞丅
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

    /// <summary>
    /// 敵を吹っ飛ばすエリアの判定
    /// </summary>
    private void BlowAway()
    {
        //1、敵の位置が黄色、白、赤のエリア内にあるかどうかを判断する
        //2、プレイヤーは攻撃したかを判断する
        //3、プレイヤーとの位置によって吹っ飛ばす方向を決める
        //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 僾儗僀儎乕偺峌寕斖埻偵擖偭偨応崌
        if (col.CompareTag("Player"))
        {
            ATKAREA = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // 僾儗僀儎乕偺峌寕斖埻偐傜敳偗偨応崌
        if (col.CompareTag("Player"))
        {
            ATKAREA = false;
        }
    }
}
