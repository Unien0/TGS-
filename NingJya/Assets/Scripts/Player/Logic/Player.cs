using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    private float maxHp
    {
        //playerDataの中にmaxHpをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    private float hp
    {
        //playerDataの中にhpをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.hp; else return 0; }

    }
    private float damage
    {
        //playerDataの中にdamageをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.damage; else return 0; }
       
    }
    private float speed
    {
        //playerDataの中にspeedをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.speed; else return 0; }

    }
   
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return false; }
        set { playerData.isDead = value; }//setは自分の変数がplayerDataに送る
    }
    public bool attackable
    {
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }//setは自分の変数がplayerDataに送る
    }
    public bool removable
    {
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }//setは自分の変数がplayerDataに送る
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

            // 峌寕張棟
            if (Input.GetKeyDown(KeyCode.E))
            {
                ATK = true;
            }

            // 峌寕張棟
            if (ATK)
            {
                // 僋乕儖僟僂儞偺妋擣
                time += Time.deltaTime;
                if (time >= 1)
                {
                    time = 0;
                    ATK = false;
                }
            }
        }        
    }

    void move()
    {
        // 堏摦擖椡 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 堏摦張棟
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
