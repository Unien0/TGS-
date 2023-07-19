using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float time;
    private float roteMax;
    private float moveinput;
    private float inputX;
    private float inputY;


    public bool ATK;

    public Vector2 shotrote;

    private GameObject Enemyobj;    
    [SerializeField] private GameObject AttackArea;    
    [SerializeField]private GameObject KATANA;

    private Rigidbody2D rb2d;
    private Animator[] animators;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.T)) || Input.GetKeyDown("joystick button 5"))
        {
            SceneManager.LoadScene(0);
        }
       

        if (!isDead)
        {
            move();
            PlayerInput();
            SwitchAnimation();

            // 攻撃処理
            if ((Input.GetKeyDown(KeyCode.E))|| Input.GetKeyDown("joystick button 1"))
            {
                attackable = false;
                KATANA.GetComponent<Animator>().SetBool("ATK",true);
            }
            else
            {
                KATANA.GetComponent<Animator>().SetBool("ATK", false);
            }

            // 攻撃処理、playerCDの部分
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
        if ((horizontal != 0) || (vertical != 0))
        {
            moveinput += Time.deltaTime;

            // 入力に応じて、回転を変更する
            if ((horizontal >= 0.01f) && (vertical >= 0.01f)) roteMax = 315;
            if ((horizontal == 0) && (vertical >= 0.01)) roteMax = 0;
            if ((horizontal <= -0.01f) && (vertical >= 0.01f)) roteMax = 45;
            if ((horizontal <= -0.01f) && (vertical == 0)) roteMax = 90;
            if ((horizontal <= -0.1f) && (vertical <= -0.01f)) roteMax = 135;
            if ((horizontal == 0) && (vertical <= -0.01f)) roteMax = 180;
            if ((horizontal >= 0.01f) && (vertical <= -0.01f)) roteMax = 225;
            if ((horizontal >= 0.01f) && (vertical == 0)) roteMax = 270;
            AttackArea.transform.rotation = Quaternion.Euler(0, 0, roteMax);
        }
        else
        {
            moveinput = 0;
            rb2d.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
        if (moveinput >= 0.05f)
        {
            // 移動処理
            rb2d.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void SwitchAnimation()
    {
        foreach (var anim in animators)
        {
            anim.SetFloat("InputX", inputX);
            anim.SetFloat("InputY", inputY);
        }
    }
}
