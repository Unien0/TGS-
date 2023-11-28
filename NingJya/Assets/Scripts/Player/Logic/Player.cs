using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    public int maxHp
    {
        //PlayerのMaxHPをゲットする
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    public int hp
    {
        //PlayerのHPをゲットする
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }
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
    public int maxBullet
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.maxBullet; else return 1; }
    }
    public int nowBullet
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.nowBullet; else return 1; }
        set { playerData.nowBullet = value; }
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
    public bool taking
    {
        //プレイヤーが移動できるかどうかを判断する
        get { if (playerData != null) return playerData.taking; else return false; }
        set { playerData.taking = value; }
    }
    public bool Mutekki
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.mutekki; else return false; }
        set { playerData.mutekki = value; }
    }
    public bool TimeInspect
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.levelling; else return false; }
        set { playerData.levelling = value; }
    }
    public bool IsAttack
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
    }
    #endregion 

    [SerializeField] private GameManeger gameManegerCS;

    private float inputX;                   // 横軸の移動入力を確認する変数
    private float inputY;                   // 縦軸の移動入力を確認する変数
    private Vector2 TemporaryInput;         // 一時的に移動入力を保存する変数
    private float inputFixH;                // TemporaryInput.xの値を初期化するまでの時間
    private float inputFixV;                // TemporaryInput.yの値を初期化するまでの時間
    private Vector2 moveInput;              // 1テンポ毎事の移動入力を保存する変数
    public float moveDirection;             // 移動方向の保存
    private bool maked;                     // 移動エフェクトの生成を確認する変数
    // 移動エフェクトを保存する変数
    [SerializeField] private GameObject MoveEfect;
    private bool ActFix;

    private float MutekiTime;               // ダメージ時後の無敵時間を管理する変数
    public bool Hit;                        // ダメージを食らった時の確認用変数

    // 攻撃範囲オブジェクトを保存する変数
    [SerializeField] private GameObject AttackArea;
    private bool InputATK;                  // 攻撃入力の確認用変数
    public float roteMax;                   // 攻撃範囲の回転数を保存する変数

    private float animSpeed;                // アニメーション速度の値を保存する変数

    private Rigidbody2D rb2d;               // Rigidbody2Dコンポーネントを保存する変数
    private Animator anim;                  // Animatorコンポーネントを保存する変数
    private SpriteRenderer SpR;             // SpriteRendererコンポーネントを保存する変数
    public Collider2D PlayerCol2D;          // Collider2Dコンポーネントを保存する変数
    private PlayerInputActions controls;    // PlayerInputActionsを使用するための変数
    private Vector2 moveCon;                // コントローラーのスティック操作を認識するための変数
    private bool AlphaExchange;

    private AudioSource Audio;
    [SerializeField] private AudioClip DamageSE;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject[] NearEnemys;
    private Vector2 Gap;
    [SerializeField] private GameObject[] TargetEnemys;
    private float RotePass;
    private float TargetRote;

    private void Awake()
    {
        // 全敵の情報を取得
        NearEnemys = GameObject.FindGameObjectsWithTag("Enemy");

        // Rigidbody2Dコンポーネントを保存する
        rb2d = GetComponent<Rigidbody2D>();
        // Animatorコンポーネントを保存する
        anim = GetComponent<Animator>();
        // SpriteRendererコンポーネントを保存する
        SpR = GetComponentInChildren<SpriteRenderer>();
        // Collider2Dコンポーネントを保存する
        PlayerCol2D = GetComponent<Collider2D>();

        // マウスカーソルを非表示にする
        Cursor.visible = false;

        // コントローラーのスティック操作に応じて処理を行うように初期化する
        controls = new PlayerInputActions();
        // 操作入力に応じてmoveConに値を保存するように初期化する
        controls.GamePlay.Move.performed += ctx => moveCon = ctx.ReadValue<Vector2>();
        controls.GamePlay.Move.canceled += ctx => moveCon = Vector2.zero;
        // 攻撃入力に応じて攻撃処理を行うように初期化する
        controls.GamePlay.Attack.started += ctx => Attack();
    }

    private void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    void Start()
    {
        // 第一ステージなら
        if (SceneManager.GetActiveScene().name == "1stStage_Remake")
        {
            nowBullet = maxBullet;
            hp = maxHp;
        }
        anim.SetBool("DEAD",false);
        Audio = GetComponent<AudioSource>();
        isDead = false;
    }
    
    void Update()
    {
        // チュートリアルステージなら
        if (SceneManager.GetActiveScene().name == "0_Tutorial")
        {
            hp = maxHp;
        }

        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);
        }
       
        if (!isDead)
        {
            if (!taking)
            {
                move();
            }
            else
            {
                rb2d.velocity = Vector2.zero;
            }
            
            SwitchAnimation();
        }

        if (Hit)
        {
            if (!Mutekki)
            {
                hp--;
                Mutekki = true;
                MutekiTime = 0;
                Audio.clip = DamageSE;
                Audio.Play();
                if (hp <= 0)
                {
                    isDead = true;
                    rb2d.velocity = Vector2.zero;
                    PlayerCol2D.enabled = false;
                    GameOver.GAMEOVER = true;
                    anim.SetBool("DEAD", true);
                }
            }
            MutekiTime += Time.deltaTime;

            if (MutekiTime >= 3)
            {
                Hit = false;
                Mutekki = false;
                AlphaExchange = false;
                SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b,1);
            }
            else
            {
                if (!AlphaExchange)
                {
                    SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a - Time.deltaTime * 8);
                    if(SpR.color.a <= 0)
                    {
                        AlphaExchange = true;
                    }
                }
                else
                {
                    SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a + Time.deltaTime * 8);
                    if (SpR.color.a >= 1)
                    {
                        AlphaExchange = false;
                    }
                }

            }
        }

    }

    private void LateUpdate()
    {
        if (!isDead)
        {
            PlayerInput();
        }
    }

    void move()
    {
        // 移動入力 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0)
        {
            inputFixH += Time.deltaTime;
            if (inputFixH > gameManegerCS.OneTempo)
            {
                TemporaryInput.x = 0;
            }
        }
        else
        {
            // 入力を記録する
            inputFixH = 0;
            TemporaryInput.x = horizontal;
        }
        if(vertical == 0)
        {
            inputFixV += Time.deltaTime;
            if (inputFixV > gameManegerCS.OneTempo)
            {
                TemporaryInput.y = 0;
            }
        }
        else
        {   
            // 入力を記録する
            inputFixV = 0;
            TemporaryInput.y = vertical;
        }
        
        if (removable && !taking)
        {
            if (!maked)
            {
                if ((moveInput.x != 0) || (moveInput.y != 0))
                {
                    maked = true;
                    Instantiate(MoveEfect, this.transform.position, MoveEfect.transform.rotation);
                }
                TemporaryInput = Vector2.zero;
            }

            // 移動処理
            rb2d.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);


            ActFix = false;
        }
        else
        {
            if (!ActFix)
            {
                ActFix = true;

                // 再度アクション処理ができるように初期化する
                IsAttack = false;                             // 攻撃処理の初期化
                maked = false;                                // 移動エフェクト生成フラグの初期化
                rb2d.velocity = new Vector2(0, 0);            // キャラの移動を停止させる
                attackable = false;                           // 攻撃許可の取り消し
                anim.SetBool("Attack", false);                // 攻撃アニメーションの停止
                anim.SetBool("Attack", false);                // 攻撃アニメーションの停止
            }
            else
            {
                #region 移動入力の処理 　
                if ((TemporaryInput.x != 0) || (TemporaryInput.y != 0))
                {   // 入力に応じて、攻撃範囲を回転する
                    #region roteMax変更
                    // 入力に応じて、回転を変更する
                    if ((TemporaryInput.x >= 0.425f) && (TemporaryInput.x <= 0.8f) && (TemporaryInput.y >= 0.01f))
                    {
                        moveInput = new Vector2(1, 1);
                        moveDirection = 315;
                    }

                    if ((TemporaryInput.x == 0) && (TemporaryInput.y >= 0.01))
                    {
                        moveInput = new Vector2(0, 1);
                        moveDirection = 0;
                    }

                    if ((TemporaryInput.x <= -0.425f) && (TemporaryInput.x >= -0.8f) && (TemporaryInput.y >= 0.01f))
                    {
                        moveInput = new Vector2(-1, 1);
                        moveDirection = 45;
                    }

                    if ((TemporaryInput.x <= -0.01f) && (TemporaryInput.y == 0))
                    {
                        moveInput = new Vector2(-1, 0);
                        moveDirection = 90;
                    }

                    if ((TemporaryInput.x <= -0.425f) && (TemporaryInput.x >= -0.8f) && (TemporaryInput.y <= -0.01f))
                    {
                        moveInput = new Vector2(-1, -1);
                        moveDirection = 135;
                    }

                    if ((TemporaryInput.x == 0) && (TemporaryInput.y <= -0.01f))
                    {
                        moveInput = new Vector2(0, -1);
                        moveDirection = 180;
                    }

                    if ((TemporaryInput.x >= 0.425f) && (TemporaryInput.x <= 0.8f) && (TemporaryInput.y <= -0.01f))
                    {
                        moveInput = new Vector2(1, -1);
                        moveDirection = 225;
                    }

                    if ((TemporaryInput.x >= 0.01f) && (TemporaryInput.y == 0))
                    {
                        moveInput = new Vector2(1, 0);
                        moveDirection = 270;
                    }
                    #endregion

                    // 入力に応じてアニメーションを変更する
                    #region アニメーション変更 
                    // 横軸入力
                    if (moveInput.x >= 0.1f)
                    {
                        SpR.flipX = true;
                        anim.SetBool("Side", true);
                    }
                    else if (moveInput.x <= -0.1f)
                    {
                        SpR.flipX = false;
                        anim.SetBool("Side", true);
                    }
                    else
                    {
                        anim.SetBool("Side", false);
                    }
                    // 縦軸入力
                    if (moveInput.y >= 0.1f)
                    {
                        anim.SetBool("Top", true);
                        anim.SetBool("Bottom", false);

                    }
                    else if (moveInput.y <= -0.1f)
                    {
                        anim.SetBool("Top", false);
                        anim.SetBool("Bottom", true);
                    }
                    else
                    {
                        anim.SetBool("Top", false);
                        anim.SetBool("Bottom", false);
                    }
                    #endregion
                }
                else
                { moveInput = Vector2.zero; }
                #endregion
            }
        }
    }

    private void Attack()
    {
        float hr_R = Input.GetAxis("Horizontal_Right");
        float vr_R = Input.GetAxis("Vertical_Right");

        if ((hr_R != 0) || (vr_R != 0))
        {   // 入力に応じて、攻撃範囲を回転、攻撃アニメーションを再生する
            #region roteMax変更
            // 上入力
            if ((hr_R == 0) && (vr_R >= 0.01))
            {
                roteMax = 0;
                anim.SetBool("Right_R", false);
                anim.SetBool("Left_R", false);
                anim.SetBool("Up_R", true);
                anim.SetBool("Down_R", false);
            }
            // 左入力
            if ((hr_R <= -0.01f) && (vr_R == 0))
            {
                roteMax = 90;
                // フリップの状態に合わせてSetBoolを変更する
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }

            }
            // 右入力
            if ((hr_R >= 0.01f) && (vr_R == 0))
            {
                roteMax = 270;
                // フリップの状態に合わせてSetBoolを変更する
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", false);
                }
            }
            // 下入力
            if ((hr_R == 0) && (vr_R <= -0.01f))
            {
                roteMax = 180;
                anim.SetBool("Right_R", false);
                anim.SetBool("Left_R", false);
                anim.SetBool("Up_R", false);
                anim.SetBool("Down_R",true);
            }
            // 右上入力
            if ((hr_R >= 0.01f) && (vr_R >= 0.01f))
            {
                roteMax = 315;
                // フリップの状態に合わせてSetBoolを変更する
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }

            }
            // 左上入力
            if ((hr_R <= -0.01f) && (vr_R >= 0.01f))
            {
                roteMax = 45;
                // フリップの状態に合わせてSetBoolを変更する
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }
                else
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", true);
                    anim.SetBool("Down_R", false);
                }
            }
            // 左下入力
            if ((hr_R <= -0.1f) && (vr_R <= -0.01f))
            {
                roteMax = 135;
                // フリップの状態に合わせてSetBoolを変更する
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
                else
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
            }
            // 右下
            if ((hr_R >= 0.01f) && (vr_R <= -0.01f))
            {
                roteMax = 225;
                // フリップの状態に合わせてSetBoolを変更する
                if (SpR.flipX == true)
                {
                    anim.SetBool("Right_R", false);
                    anim.SetBool("Left_R", true);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
                else
                {
                    anim.SetBool("Right_R", true);
                    anim.SetBool("Left_R", false);
                    anim.SetBool("Up_R", false);
                    anim.SetBool("Down_R", true);
                }
            }
            #endregion
            // 攻撃範囲を回転させる
            AttackArea.transform.rotation = Quaternion.Euler(0, 0, roteMax);
            InputATK = true;
        }
        else
        {
            InputATK = false;
        }

        // 
        if (removable)
        {
            if ((Input.GetKeyDown(KeyCode.Q)) || (Input.GetAxis("L_R_Trigger") > 0) || (Input.GetAxis("L_R_Trigger") < 0))
            {
                if (nowBullet > 0)
                {
                    if (InputATK)
                    {
                        // 情報登録
                        // 全敵の情報から
                        foreach (var ctobj in NearEnemys)
                        {
                            // 一定範囲に居るオブジェクトを確認する
                            Gap = new Vector2(ctobj.transform.position.x - this.transform.position.x, ctobj.transform.position.y - this.transform.position.y);
                            float vec = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y);
                            // 一定範囲内にいる敵が
                            if (vec < 9)
                            {
                                // ノックバック or 死亡状態じゃないなら
                                if(ctobj.GetComponent<Collider2D>().enabled == true)
                                {
                                    // 攻撃対象に追加する
                                    Array.Resize(ref TargetEnemys, TargetEnemys.Length + 1);
                                    TargetEnemys[TargetEnemys.Length - 1] = ctobj.gameObject;
                                }

                            }
                        }

                        if (TargetEnemys.Length != 0)
                        {
                            // 攻撃対象にバレットを発射する
                            foreach (var Atobj in TargetEnemys)
                            {
                                RotePass = Mathf.Atan2((Atobj.transform.position.x - this.transform.position.x), (Atobj.transform.position.y - this.transform.position.y));
                                TargetRote = RotePass * Mathf.Rad2Deg;
                                AttackArea.transform.rotation = Quaternion.Euler(0, 0, TargetRote * -1);
                                Instantiate(Bullet, this.transform.position, AttackArea.transform.rotation);
                            }
                            nowBullet--;
                            Array.Resize(ref TargetEnemys, 0);
                        }                  
                    }
                }
            }
            else if (!attackable && removable)
            {
                if (InputATK)
                {
                    attackable = true;
                    anim.SetBool("Attack", true);
                    IsAttack = true;
                }
            }
        }
    }

    private void PlayerInput()
    {
        inputY = Input.GetAxisRaw("Vertical");
        if (inputY <= 0f && TimeInspect)
        {
            anim.SetBool("TimeInspect", true);
        }
        else
        {
            anim.SetBool("TimeInspect", false);
        }
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("InputY", inputY);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (!Hit)
            {
                if (col.gameObject.GetComponent<Enemy>().removable)
                {
                    if (col.gameObject.GetComponent<Enemy>().enemyAct != Enemy.enemyActSet.Dummy)
                    {
                        Hit = true;
                    }
                }
            }
        }
        if (col.gameObject.CompareTag("EnemyBullet"))
        {
            Hit = true;
        }
        if (col.gameObject.name == "END")
        {
            rb2d.velocity = Vector2.zero;
            PlayerCol2D.enabled = false;
            GameOver.GAMECLEAR = true;
        }
           
    }

}
