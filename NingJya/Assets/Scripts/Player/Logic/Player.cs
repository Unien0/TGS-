using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    private float maxHp
    {
        //PlayerのMaxHPをゲットする
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    private float hp
    {
        //PlayerのHPをゲットする
        get { if (playerData != null) return playerData.hp; else return 0; }

    }
    private float damage
    {
        //Playerのダメージ値を取得する
        get { if (playerData != null) return playerData.damage; else return 0; }
       
    }
    private float speed
    {
        //Playerのスピード値を取得する
        get { if (playerData != null) return playerData.speed; else return 0; }

    } 
    private float attackCD
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.attackCD; else return 1; }

    }
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return false; }
        set { playerData.isDead = value; }//プレイヤーが死亡した場合に更新
    }
    public bool attackable
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }
    }
    public bool removable
    {
        //プレイヤーが移動できるかどうかを判断する
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }
    }
    #endregion 

    public Vector2 shotrote;
    private GameObject Enemyobj;
    public bool ATK;
    private float time;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (!isDead)
        {
            move();

            // 攻撃処理
            if (Input.GetKeyDown(KeyCode.E))
            {
                attackable = false;
            }

            // 攻撃処理
            if (!attackable)
            {
                // クールダウンの確認
                time += Time.deltaTime;
                if (time >= attackCD)
                {
                    time = 0;
                    attackable = true;
                }
            }
        }        
    }

    void move()
    {
        // 移動入力 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // 移動処理
        rb2d.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.CompareTag("Enemy"))
    //    {
    //        Enemyobj = col.gameObject;
    //    }
    //}
}
