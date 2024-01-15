using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //共有データ
    public PlayerData_SO playerData;
    #region PlayerDataの変数
    private int playerHp
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
    public bool IsAttack
    {
        //プレイヤーが攻撃できるかどうかを判断する
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
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
    public bool removable
    {
        //移動可能かどうかを判断する
        get { if (enemyData != null) return enemyData.removable; else return false; }
        set { enemyData.removable = value; }
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

    public enum enemyActSet
    {
        move,
        notMove,
        cannonball,
        knockback,
        clockup,
        Dummy,
        Bug,
        Earthworm,
        Carp,
        end
    }
    [SerializeField]private bool DontSporn;
    public float actTime;// 行動までの待機時間    
    [SerializeField] private float objctDistance;
    [SerializeField]private float hp;
    public float knockbackPoint;
    private Vector2 ReSponePoint;

    //個体の状態（Bool）
    private bool inPlayerAttackRange = false;
    private bool isEnemyClash = false;
    private bool shotOk;
    // 0~3の間の数値を入力する
    [SerializeField] private int ShotTempo;
    private bool isShot;
    public bool shoted;//吹っ飛ばすの状態
    private bool hit;
    private bool ishit;
    private double CoolDownTime;
    private double time = -1;
    private bool stop = false;// 移動処理の停止
    private bool fix = false;
    [SerializeField] private bool exchange;
    private bool isEnd;
    public bool conductIt;
    public bool Ready;
    private bool isBlow;
    public bool isAtk;

    private Rigidbody2D rb2d;
    private SpriteRenderer SpR;
    private Collider2D col2d;
    private Animator anim;
    public enemyActSet enemyAct;

    private GameObject PlayerObject;
    public GameObject conductObject;
    [SerializeField] private GameObject HitEfect;
    [SerializeField] private GameObject DEADEfect;
    [SerializeField] private GameObject EnemyBullet;
    [SerializeField] private GameObject ShotRote;
    private float gapPos;
    private float gapfixPos;

    private Vector2 moveint;
    private Vector2 PosCheck;
    private Vector2 moveit;// moveintの結果を正負のみの値にする
    public Vector2 knockbackRote;
    [SerializeField] private Vector2 shotIt;


    private AudioSource Audio;
    [SerializeField] private AudioClip isBlowSE;
    [SerializeField] private AudioClip HITSE;
    private bool IsSound;
    [SerializeField] private AudioClip DeadSE1;
    [SerializeField] private AudioClip DeadSE2;

    private bool ComboFix;
    public int MakeComboCount;
    private float animSpeed;

    [SerializeField] private GameObject ComboEfect;
    private bool AlphaExchange;

    private bool AssaultMode;
    private bool HideMobe;
    [SerializeField]
    private bool Enter;
    [SerializeField]
    private float HideTime;

    [SerializeField] private GameObject[] DestoyObj;

    private void Awake()
    {
        hp = enemyHp;
        rb2d = GetComponent<Rigidbody2D>();
        SpR = GetComponent<SpriteRenderer>();
        col2d = GetComponent<Collider2D>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
        Audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        knockbackPoint = ForcePoint;
    }

    void Start()
    {
        ReSponePoint = transform.position;
        hp = enemyHp;
        if (enemyAct == enemyActSet.knockback)
        { knockbackPoint = knockbackPoint * 2; }
        CoolDownTime = FindObjectOfType<GameManeger>().OneTempo * 2;
        if (time == -1)
        {
            time = 10;
        }
    }

    void Update()
    {
        // 現在のテンポ数に応じたアニメーション速度に変更する
        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);

        // 停止指示が出されていない場合
        if (!isEnd)
        {
            // 経過時間を算出する
            time += Time.deltaTime;

            // ノックバック状態になった場合
            if (isBlow)
            {
                // ノックバック処理を行う
                BlowAway();
                isBlow = false;
            }

            //ノックバック状態じゃない場合
            if (!shoted)
            {
                // 敵の種類に応じて処理を変更する
                switch (enemyAct)
                {
                    case enemyActSet.move:
                        // 移動処理を呼び出す
                        Move();
                        break;

                    case enemyActSet.notMove:
                        // 停止状態にさせる
                        rb2d.velocity = Vector3.zero;
                        break;

                    case enemyActSet.cannonball:
                        // 停止状態にさせた上
                        rb2d.velocity = Vector3.zero;
                        
                        // 射撃処理を許可し
                        shotOk = true;
                        // プレイヤーのいる方角を取得し続ける
                        // ラジアン数を算出し
                        gapPos = Mathf.Atan2((PlayerObject.transform.position.x - this.transform.position.x), 
                                              (PlayerObject.transform.position.y - this.transform.position.y));
                        // 度数に変換する
                        gapfixPos = gapPos * Mathf.Rad2Deg;
                        // マイナスの計算結果が出た場合、プラスの結果として算出する
                        if (gapfixPos < 0)
                        {
                            gapfixPos += 360;
                        }
                        // その方角に向けて発射口を回転させる
                        ShotRote.transform.rotation = Quaternion.Euler(0, 0,-1* gapfixPos);
                        break;

                    case enemyActSet.knockback:
                        // 移動処理を呼び出す
                        Move();                        
                        break;

                    case enemyActSet.Dummy:
                        // 停止状態にさせる
                        rb2d.velocity = Vector2.zero;
                        break;

                    case enemyActSet.Earthworm:
                        // 移動処理と潜伏処理を呼び出す
                        Move();
                        Hide();
                        break;

                    case enemyActSet.Carp:
                        // 潜伏処理と突撃処理を呼び出す
                        Assault();
                        Hide();
                        break;

                }
            }

            if (hit)
            {                
                Instantiate(HitEfect, this.transform.position, this.transform.rotation);
                Audio.clip = HITSE;
                Audio.Play();
                shoted = true;
                hit = false;
                HitBlow();
                Camera.ShakeOrder = true;
                ShakeManeger.ShakeLevel = 1;
            }

            // 攻撃範囲に敵がいて
            if (inPlayerAttackRange)
            {
                // クールダウンが回復している状態で、かつ
                if (CoolDownTime <= time)
                {
                    // 攻撃入力を受け付けた時
                    if (IsAttack)
                    {
                        // ジャストアタックのタイミングなら吹っ飛ばし処理を呼び出す
                        if (blowable)
                        {
                            // ダメージエフェクトを発生させる
                            Instantiate(HitEfect, this.transform.position, this.transform.rotation);
                            // ダメージSEを再生させる
                            Audio.clip = HITSE;
                            Audio.Play();

                            // 射撃機能を停止させる
                            shoted = true;
                            // 体力を1減らす
                            hp = -1;
                            // ノックバック状態にする
                            isBlow = true;

                            // ConductManeger.csに誘導処理の計算を行わせる
                            FindObjectOfType<ConductManeger>().conduct = true;
                            // ConductManeger.csに自身のデータ(吹っ飛ばし対象オブジェクト)を送る
                            FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                            // ノックバック待機状態にする
                            conductIt = true;

                            // 経過時間の加算を止める
                            time = 0;
                            // Camera.csの画面シェイク機能を処理させる
                            Camera.ShakeOrder = true;

                            // 画面シェイクの時間・度合いを送信する
                            GameManeger.shakeTime = 0.125f;
                            ShakeManeger.ShakeLevel = 2;

                            // ダメージ時のスコア加算処理を呼び出す
                            KillCombo();
                        }
                        else
                        {
                            shoted = false;
                        }
                    }

                }
            }

            //吹っ飛ばすしたら、止まる時間を計算する
            if (shoted)
            {
                ToStop();
                if (!AlphaExchange)
                {
                    SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a - Time.deltaTime * 10);
                    if (SpR.color.a <= 0)
                    {
                        AlphaExchange = true;
                    }
                }
                else
                {
                    SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a + Time.deltaTime * 10);
                    if (SpR.color.a >= 1)
                    {
                        AlphaExchange = false;
                    }
                }
            }
            else
            {
                SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, 1);
            }

            if (shotOk)
            {
                if (enemyAct == enemyActSet.cannonball)
                {
                    #region
                    if (GameManeger.Tempo == ShotTempo)
                    {
                        if (!isShot)
                        {
                            PlayerObject = FindObjectOfType<Player>().gameObject;
                            moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                            objctDistance = Mathf.Sqrt(moveint.x * moveint.x + moveint.y * moveint.y);
                            if (objctDistance <= 10)
                            {
                                Instantiate(EnemyBullet, this.transform.position, ShotRote.transform.rotation);
                            }
                            isShot = true;                            
                        }
                    }
                    else
                    {
                        isShot = false;
                    }
                    #endregion
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
            PosCheck = new Vector2(Mathf.Abs(moveint.x), Mathf.Abs(moveint.y));   
            objctDistance = Mathf.Sqrt(moveint.x * moveint.x + moveint.y * moveint.y);
            if (objctDistance <= 10)
            {
                moveit.x = Mathf.Sign(moveint.x);
                moveit.y = Mathf.Sign(moveint.y);

                if (!isAtk)
                {
                    rb2d.velocity = moveit * enemySpeed;
                }
                else
                {
                    rb2d.velocity = Vector2.zero;
                }
            }
        }
        else
        {
            isAtk = false;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;

            if (this.transform.position.x - Mathf.FloorToInt(this.transform.position.x) > 0.5f)
            {
                moveit.x = Mathf.Ceil(this.transform.position.x);
            }
            else
            {
                moveit.x = Mathf.Floor(this.transform.position.x);
            }

            if (this.transform.position.y - Mathf.FloorToInt(this.transform.position.y) > 0.5f)
            {
                moveit.y = Mathf.Ceil(this.transform.position.y);
            }
            else
            {
                moveit.y = Mathf.Floor(this.transform.position.y);
            }
            this.transform.position = moveit;

        }
    }

    private void Assault()
    {
        if (AssaultMode)
        {
            if (objctDistance <= 10)
            {
                rb2d.velocity = transform.up * enemySpeed;
            }
        }
        else
        {
            if (HideMobe)
            {
                gapPos = Mathf.Atan2((PlayerObject.transform.position.x - this.transform.position.x), (PlayerObject.transform.position.y - this.transform.position.y));
                gapfixPos = gapPos * Mathf.Rad2Deg;
                if (gapfixPos < 0)
                {
                    gapfixPos += 360;
                }
                this.transform.rotation = Quaternion.Euler(0, 0, -1 * gapfixPos);
            }
        }

        switch (enemyAct)
        {
            case enemyActSet.Carp:
                if (GameManeger.Tempo == 1)
                {
                    AssaultMode = true;
                }
                else
                {

                    AssaultMode = false;
                }
                break;
        }
    }

    private void Hide()
    {
        switch (enemyAct)
        {
            case enemyActSet.Earthworm:
                if ((GameManeger.Tempo == 3) || (GameManeger.Tempo == 0))
                {
                    HideMobe = true;
                }
                else
                {
                    HideMobe = false;
                }
                break;
        }

        /*if (Enter)
        {
            HideTime += Time.deltaTime;
            if (HideTime > 0.25f)
            {
                HideMobe = true;
                rb2d.velocity = Vector3.zero;
            }
        }
        else
        {
            HideMobe = false;
            HideTime = 0;
        }*/

        if (HideMobe)
        {
            col2d.enabled = false;
            SpR.enabled = false;
            anim.SetBool("Hide", true);
            Debug.Log("Hide");
        }
        else
        {
            col2d.enabled = true;
            SpR.enabled = true;
            anim.SetBool("Hide", false);
        }
    }

    /// <summary>
    /// 吹っ飛ばす処理
    /// </summary>
    private void BlowAway()
    {
        // プレイヤーの攻撃範囲にいる場合
        if (inPlayerAttackRange)
        {
            // ConductManeger.csから計算結果が返された時
            if (Ready)
            {                
                // かつ誘導先オブジェクトが設定されている場合
                if (conductObject != null)
                {
                    //誘導先オブジェクトの座標と自身の座標をもとに、吹っ飛ばされる方向を求める
                    knockbackRote = new Vector2(conductObject.transform.position.x - this.transform.position.x, 
                                                conductObject.transform.position.y - this.transform.position.y);

                    // Rotetion.xの値が0.5 ~ -0.5以内なら、その値をそのまま代入する
                    // それ以外の場合は0として算出する
                    if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                    {
                        shotIt.x = Mathf.Sign(knockbackRote.x); 
                    }
                    else
                    {   
                        shotIt.x = 0;
                    }

                    // Rotetion.yの値が0.5 ~ -0.5以内なら、その値をそのまま代入する
                    // それ以外の場合は0として算出する
                    if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                    {
                        shotIt.y = Mathf.Sign(knockbackRote.y); 
                    }
                    else
                    {   
                        shotIt.y = 0; 
                    }

                     // 誘導先オブジェクトの方向に向かって、このオブジェクトを弾き飛ばします
                     rb2d.AddForce(shotIt * knockbackPoint);
                }
                // 誘導先オブジェクトが設定されていない場合
                else
                {
                    // 自身の座標とPlayerの座標ををもとに、真後ろの方向を算出する
                    knockbackRote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, 
                                                this.transform.position.y - PlayerObject.transform.position.y);

                    // Rotetion.xの値が0.5 ~ -0.5以内なら、その値をそのまま代入する
                    // それ以外の場合は0として算出する
                    if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(knockbackRote.x); }
                    else
                    { shotIt.x = 0; }
                    // Rotetion.yの値が0.5 ~ -0.5以内なら、その値をそのまま代入する
                    // それ以外の場合は0として算出する
                    if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(knockbackRote.y); }
                    else
                    { shotIt.y = 0; }

                    // 計算結果をもとに、その方向に向かってこのオブジェクトを弾き飛ばします
                    rb2d.AddForce(shotIt * knockbackPoint);
                }
            }
        }
    }

    void HitBlow()
    {
        if (Ready)
        {            
            if (conductObject != null)
            {
                //方向
                knockbackRote = new Vector2(this.transform.position.x - conductObject.transform.position.x, this.transform.position.y - conductObject.transform.position.y);

                if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                { shotIt.x = Mathf.Sign(knockbackRote.x); }
                else
                { shotIt.x = 0; }
                if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                { shotIt.y = Mathf.Sign(knockbackRote.y); }
                //現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                rb2d.AddForce(shotIt * knockbackPoint);
            }
            else
            {
                //方向
                knockbackRote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                { shotIt.x = Mathf.Sign(knockbackRote.x); }
                else
                { shotIt.x = 0; }
                if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                { shotIt.y = Mathf.Sign(knockbackRote.y); }
                else
                { shotIt.y = 0; }
                //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                rb2d.AddForce(shotIt * knockbackPoint);
            }
        }
    }

    /// <summary>
    /// 吹っ飛ばす状態に止まるの時間
    /// </summary>
    private void ToStop()
    {
        anim.SetBool("DEAD", true);
        shotOk = false;
        actTime += Time.deltaTime;
        if (actTime >= blowTime)
        {
            transform.eulerAngles = Vector3.zero;
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;


            if (GameManeger.TempoExChange)
            {
                if (hp <= 0)
                {                    
                    if (!IsSound)
                    {
                        IsSound = true;
                        Audio.clip = DeadSE2;
                        Audio.Play();
                        // エフェクト作成
                        Instantiate(DEADEfect, this.transform.position, this.transform.rotation);
                        GameObject instance = Instantiate(ComboEfect, this.transform.position, this.transform.rotation);
                        instance.GetComponent<EfectDestory>().ComboPoint = MakeComboCount;
                    }

                    foreach (GameObject child in DestoyObj)
                    {
                        if (child != null)
                        {
                            GameObject.Destroy(child.gameObject);
                        }
                        else
                        {
                            break;
                        }
                    }

                    SpR.enabled = false;
                    col2d.enabled = false;



                    if (enemyAct == enemyActSet.Dummy)
                    {
                        if (!DontSporn)
                        {
                            if (actTime > blowTime + 1)
                            {
                                // 初期化する
                                SpR.enabled = true;
                                col2d.enabled = true;
                                hp = 1;
                                transform.position = ReSponePoint;
                                anim.SetBool("DEAD", false);
                                AlphaExchange = false;
                                IsSound = false;
                                shoted = false;
                                Ready = false;
                                //GameManeger.ComboMax--;
                                inPlayerAttackRange = false;
                            }
                        }
                    }
                    else if (actTime > blowTime + 1)
                    {
                        isEnd = true;
                        GameManeger.KillEnemy ++;
                    }
                }
                else
                {
                    actTime = 0;
                    shoted = false;
                }
            }
        }
    }

    void KillCombo ()
    {
        if (!ComboFix)
        {
            ComboFix = true;
            GameManeger.ComboCheck = true;
            MakeComboCount = GameManeger.ComboCount + 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.GetComponent<Enemy>().isAtk)
            {
                isAtk = true;
            }
            if (col.gameObject.GetComponent<Enemy>().shoted)
            {
                if (!ishit)
                {
                    ishit = true;
                    hit = true;
                    GameManeger.hitEnemy++;
                    hp = 0;
                    KillCombo();
                    GameManeger.shakeTime = 0.125f;
                    ShakeManeger.ShakeLevel = 1;
                }
            }
        }
        if ((col.gameObject.CompareTag("HitObj")) )
        {
            if (col.gameObject.GetComponent<Objects>().shoted)
            {
                if (!ishit)
                {
                    ishit = true;
                    hit = true;
                    GameManeger.hitEnemy++;
                    hp = 0;
                    KillCombo();
                    GameManeger.shakeTime = 0.125f;
                    ShakeManeger.ShakeLevel = 1;
                }
            }
        }
        if (col.gameObject.name == "！Player")
        {
            isAtk = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnemyBullet"))
        {
            if (col.gameObject.GetComponent<EnemyBullet>().isHIT == true)
            {
                if (!ishit)
                {
                    ishit = true;
                    hit = true;
                    GameManeger.hitEnemy++;
                    hp = 0;
                    KillCombo();
                    GameManeger.shakeTime = 0.125f;
                    ShakeManeger.ShakeLevel = 1;
                }
            }
        }
        if ((col.gameObject.CompareTag("HitObj")))
        {
            if (col.GetComponent<Objects>().shoted)
            {
                if (!ishit)
                {
                    ishit = true;
                    hit = true;
                    GameManeger.hitEnemy++;
                    hp = 0;
                    KillCombo();
                    GameManeger.shakeTime = 0.125f;
                    ShakeManeger.ShakeLevel = 1;
                }
            }
        }
        if ((col.gameObject.CompareTag("PlayerBullet")))
        {
            if (!ishit)
            {
                col2d.enabled = false;
                ishit = true;
                hit = true;
                GameManeger.hitEnemy++;
                hp = 0;
                KillCombo();
                GameManeger.shakeTime = 0.125f;
                ShakeManeger.ShakeLevel = 1;
            }
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
        if (col.gameObject.name == "pond")
        {
            if (enemyAct == enemyActSet.Carp)
            {
                Enter = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {            
            inPlayerAttackRange = false;
        }
        if (col.gameObject.name == "pond")
        {
            if (enemyAct == enemyActSet.Carp)
            {
                Enter = false;
            }
        }
    }
}
