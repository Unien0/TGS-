using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //���L�f�[�^
    public PlayerData_SO playerData;
    #region PlayerData�̕ϐ�
    private int playerHp
    {
        //Player��HP���Q�b�g����
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }//�_���[�W��^�����ꍇ��Player�̌��ʂ��C������
    }
    private float playerDamage
    {
        //Player�̃_���[�W�l���擾����
        get { if (playerData != null) return playerData.damage; else return 0; }

    }
    private float ForcePoint
    {
        //���̗�
        get { if (playerData != null) return playerData.force; else return 0; }
    }

    public bool playerAttackable
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.attackable; else return false; }
    }
    public bool IsAttack
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyData�̕ϐ�
    private float enemyHp
    {
        //enemy��hp���Q�b�g����
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemySpeed
    {
        //enemy��hp���Q�b�g����
        get { if (enemyData != null) return enemyData.Speed; else return 0; }
    }
    private float enemyDamage
    {
        //enemy�̃_���[�W���Q�b�g����
        get { if (enemyData != null) return enemyData.damage; else return 0; }

    }
    private float blowTime
    {
        //������΂��̏�Ԃɒ����̎���
        get { if (enemyData != null) return enemyData.blowTime; else return 0; }

    }
    private float ExistenceTime
    {
        //���̂̕ۑ�����
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }    
    private bool attackable
    {
        //�U���̉ۂ𔻒f����
        get { if (enemyData != null) return enemyData.attackable; else return false; }
    }
    public bool removable
    {
        //�ړ��\���ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.removable; else return false; }
        set { enemyData.removable = value; }
    }
    private bool blowable
    {
        //�ł���ׂ邩�ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    private bool beingBlow
    {
        //��΂���Ă��邩�ǂ����𔻒f����
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
    public float actTime;// �s���܂ł̑ҋ@����    
    [SerializeField] private float objctDistance;
    [SerializeField]private float hp;
    public float knockbackPoint;
    private Vector2 ReSponePoint;

    //�̂̏�ԁiBool�j
    private bool inPlayerAttackRange = false;
    private bool isEnemyClash = false;
    private bool shotOk;
    // 0~3�̊Ԃ̐��l����͂���
    [SerializeField] private int ShotTempo;
    private bool isShot;
    public bool shoted;//������΂��̏��
    private bool hit;
    private bool ishit;
    private double CoolDownTime;
    private double time = -1;
    private bool stop = false;// �ړ������̒�~
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
    private Vector2 moveit;// moveint�̌��ʂ𐳕��݂̂̒l�ɂ���
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
        // ���݂̃e���|���ɉ������A�j���[�V�������x�ɕύX����
        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);

        // ��~�w�����o����Ă��Ȃ��ꍇ
        if (!isEnd)
        {
            // �o�ߎ��Ԃ��Z�o����
            time += Time.deltaTime;

            // �m�b�N�o�b�N��ԂɂȂ����ꍇ
            if (isBlow)
            {
                // �m�b�N�o�b�N�������s��
                BlowAway();
                isBlow = false;
            }

            //�m�b�N�o�b�N��Ԃ���Ȃ��ꍇ
            if (!shoted)
            {
                // �G�̎�ނɉ����ď�����ύX����
                switch (enemyAct)
                {
                    case enemyActSet.move:
                        // �ړ��������Ăяo��
                        Move();
                        break;

                    case enemyActSet.notMove:
                        // ��~��Ԃɂ�����
                        rb2d.velocity = Vector3.zero;
                        break;

                    case enemyActSet.cannonball:
                        // ��~��Ԃɂ�������
                        rb2d.velocity = Vector3.zero;
                        
                        // �ˌ�����������
                        shotOk = true;
                        // �v���C���[�̂�����p���擾��������
                        // ���W�A�������Z�o��
                        gapPos = Mathf.Atan2((PlayerObject.transform.position.x - this.transform.position.x), 
                                              (PlayerObject.transform.position.y - this.transform.position.y));
                        // �x���ɕϊ�����
                        gapfixPos = gapPos * Mathf.Rad2Deg;
                        // �}�C�i�X�̌v�Z���ʂ��o���ꍇ�A�v���X�̌��ʂƂ��ĎZ�o����
                        if (gapfixPos < 0)
                        {
                            gapfixPos += 360;
                        }
                        // ���̕��p�Ɍ����Ĕ��ˌ�����]������
                        ShotRote.transform.rotation = Quaternion.Euler(0, 0,-1* gapfixPos);
                        break;

                    case enemyActSet.knockback:
                        // �ړ��������Ăяo��
                        Move();                        
                        break;

                    case enemyActSet.Dummy:
                        // ��~��Ԃɂ�����
                        rb2d.velocity = Vector2.zero;
                        break;

                    case enemyActSet.Earthworm:
                        // �ړ������Ɛ����������Ăяo��
                        Move();
                        Hide();
                        break;

                    case enemyActSet.Carp:
                        // ���������Ɠˌ��������Ăяo��
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

            // �U���͈͂ɓG������
            if (inPlayerAttackRange)
            {
                // �N�[���_�E�����񕜂��Ă����ԂŁA����
                if (CoolDownTime <= time)
                {
                    // �U�����͂��󂯕t������
                    if (IsAttack)
                    {
                        // �W���X�g�A�^�b�N�̃^�C�~���O�Ȃ琁����΂��������Ăяo��
                        if (blowable)
                        {
                            // �_���[�W�G�t�F�N�g�𔭐�������
                            Instantiate(HitEfect, this.transform.position, this.transform.rotation);
                            // �_���[�WSE���Đ�������
                            Audio.clip = HITSE;
                            Audio.Play();

                            // �ˌ��@�\���~������
                            shoted = true;
                            // �̗͂�1���炷
                            hp = -1;
                            // �m�b�N�o�b�N��Ԃɂ���
                            isBlow = true;

                            // ConductManeger.cs�ɗU�������̌v�Z���s�킹��
                            FindObjectOfType<ConductManeger>().conduct = true;
                            // ConductManeger.cs�Ɏ��g�̃f�[�^(������΂��ΏۃI�u�W�F�N�g)�𑗂�
                            FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                            // �m�b�N�o�b�N�ҋ@��Ԃɂ���
                            conductIt = true;

                            // �o�ߎ��Ԃ̉��Z���~�߂�
                            time = 0;
                            // Camera.cs�̉�ʃV�F�C�N�@�\������������
                            Camera.ShakeOrder = true;

                            // ��ʃV�F�C�N�̎��ԁE�x�����𑗐M����
                            GameManeger.shakeTime = 0.125f;
                            ShakeManeger.ShakeLevel = 2;

                            // �_���[�W���̃X�R�A���Z�������Ăяo��
                            KillCombo();
                        }
                        else
                        {
                            shoted = false;
                        }
                    }

                }
            }

            //������΂�������A�~�܂鎞�Ԃ��v�Z����
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
    /// �ړ��Ɋւ���
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
    /// ������΂�����
    /// </summary>
    private void BlowAway()
    {
        // �v���C���[�̍U���͈͂ɂ���ꍇ
        if (inPlayerAttackRange)
        {
            // ConductManeger.cs����v�Z���ʂ��Ԃ��ꂽ��
            if (Ready)
            {                
                // ���U����I�u�W�F�N�g���ݒ肳��Ă���ꍇ
                if (conductObject != null)
                {
                    //�U����I�u�W�F�N�g�̍��W�Ǝ��g�̍��W�����ƂɁA������΂������������߂�
                    knockbackRote = new Vector2(conductObject.transform.position.x - this.transform.position.x, 
                                                conductObject.transform.position.y - this.transform.position.y);

                    // Rotetion.x�̒l��0.5 ~ -0.5�ȓ��Ȃ�A���̒l�����̂܂ܑ������
                    // ����ȊO�̏ꍇ��0�Ƃ��ĎZ�o����
                    if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                    {
                        shotIt.x = Mathf.Sign(knockbackRote.x); 
                    }
                    else
                    {   
                        shotIt.x = 0;
                    }

                    // Rotetion.y�̒l��0.5 ~ -0.5�ȓ��Ȃ�A���̒l�����̂܂ܑ������
                    // ����ȊO�̏ꍇ��0�Ƃ��ĎZ�o����
                    if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                    {
                        shotIt.y = Mathf.Sign(knockbackRote.y); 
                    }
                    else
                    {   
                        shotIt.y = 0; 
                    }

                     // �U����I�u�W�F�N�g�̕����Ɍ������āA���̃I�u�W�F�N�g��e����΂��܂�
                     rb2d.AddForce(shotIt * knockbackPoint);
                }
                // �U����I�u�W�F�N�g���ݒ肳��Ă��Ȃ��ꍇ
                else
                {
                    // ���g�̍��W��Player�̍��W�������ƂɁA�^���̕������Z�o����
                    knockbackRote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, 
                                                this.transform.position.y - PlayerObject.transform.position.y);

                    // Rotetion.x�̒l��0.5 ~ -0.5�ȓ��Ȃ�A���̒l�����̂܂ܑ������
                    // ����ȊO�̏ꍇ��0�Ƃ��ĎZ�o����
                    if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(knockbackRote.x); }
                    else
                    { shotIt.x = 0; }
                    // Rotetion.y�̒l��0.5 ~ -0.5�ȓ��Ȃ�A���̒l�����̂܂ܑ������
                    // ����ȊO�̏ꍇ��0�Ƃ��ĎZ�o����
                    if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(knockbackRote.y); }
                    else
                    { shotIt.y = 0; }

                    // �v�Z���ʂ����ƂɁA���̕����Ɍ������Ă��̃I�u�W�F�N�g��e����΂��܂�
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
                //����
                knockbackRote = new Vector2(this.transform.position.x - conductObject.transform.position.x, this.transform.position.y - conductObject.transform.position.y);

                if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                { shotIt.x = Mathf.Sign(knockbackRote.x); }
                else
                { shotIt.x = 0; }
                if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                { shotIt.y = Mathf.Sign(knockbackRote.y); }
                //���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                rb2d.AddForce(shotIt * knockbackPoint);
            }
            else
            {
                //����
                knockbackRote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                if (knockbackRote.x <= -0.5f || knockbackRote.x >= 0.5f)
                { shotIt.x = Mathf.Sign(knockbackRote.x); }
                else
                { shotIt.x = 0; }
                if (knockbackRote.y <= -0.5f || knockbackRote.y >= 0.5f)
                { shotIt.y = Mathf.Sign(knockbackRote.y); }
                else
                { shotIt.y = 0; }
                //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                rb2d.AddForce(shotIt * knockbackPoint);
            }
        }
    }

    /// <summary>
    /// ������΂���ԂɎ~�܂�̎���
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
                        // �G�t�F�N�g�쐬
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
                                // ����������
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
        if (col.gameObject.name == "�IPlayer")
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
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
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
