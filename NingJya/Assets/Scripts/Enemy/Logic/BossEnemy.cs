using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{    public PlayerData_SO playerData;
    #region
    public bool IsAttack
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.isAttack; else return false; }
        set { playerData.isAttack = value; }
    }
    #endregion
    public EnemyData_SO enemyData;
    #region
    private bool blowable
    {
        //�ł���ׂ邩�ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    #endregion
    private Rigidbody2D rb2d;
    private SpriteRenderer SpR;
    private Collider2D Col2D;
    private AudioSource Audio;
    [SerializeField] private GameObject ShotRote;
    private float gapPos;
    private float gapfixPos;
    [SerializeField] private GameObject[] DestoyObj;
    private enum BossNumber
    {
        No1,
        No2,
        end
    }
    [SerializeField] private BossNumber BossType;
    [SerializeField] int BossHP;

    #region �m�b�N�o�b�N�֌W
    private bool inPlayerAttackRange = false;
    private GameObject PlayerObject;
    private Vector2 shotrote;
    private double CoolDownTime;
    private double time = -1;
    #endregion
    [SerializeField] private Vector2 shotIt;
    [SerializeField] private float ForcePoint;
    public bool ActPermission;
    private int ActCounter;
    private int LoopCounter = 1;
    private bool fix;
    private int RandumCount;
    private Vector2 MoveInt;
    public bool ExChange;
    [SerializeField] GameObject[] EnemyBullet;
    [SerializeField] private Sprite[] BossSprite;
    [SerializeField] private AudioClip isBlowSE;
    [SerializeField] private GameObject Hit_Efect;
    [SerializeField] private GameObject DEAD_EFECT;
    private float MutekiTime = 5;
    private bool AlphaExchange;

    private Animator anim;
    private float animSpeed;

    private bool isDead;
    [SerializeField]
    private int MakeEfectCount;
    private Vector2 MakePoint;
    [SerializeField]private GameObject isDeadFlash;
    private bool DeadFix;

    [SerializeField] private GameObject SpawnFlash;
    private bool doStart;
    private float waittime;
    private bool isSpawn;


    private int Ramdom1st;
    private int Ramdom2nd;
    private bool Shot3pt;
    private bool move;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SpR = GetComponent<SpriteRenderer>();
        Col2D = GetComponent<Collider2D>();
        Audio = GetComponent<AudioSource>();
        PlayerObject = FindObjectOfType<Player>().gameObject;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //animSpeed = GameManeger.AnimSpeed;
        //anim.SetFloat("AnimSpeed", animSpeed);

        if (FindObjectOfType<BossStartFlag>().ActStart)
        {
            if (!doStart)
            {  
                rb2d.velocity = Vector3.zero;
                if (!isSpawn)
                {
                    Instantiate(SpawnFlash, MakePoint, this.transform.rotation);
                    isSpawn = true;
                    SpR.enabled = false;
                }
                else
                {
                    waittime += Time.deltaTime;
                    if (waittime >= 1.5f)
                    {
                        doStart = true;
                        SpR.enabled = true;
                    }
                    else if(waittime >= 1.2f) 
                    {
                        
                    }
                    else if (waittime >= 0.5f)
                    {
                        MakePoint.x = Random.Range(this.transform.position.x - 1, this.transform.position.x + 1);
                        MakePoint.y = Random.Range(this.transform.position.y - 1, this.transform.position.y + 1);
                        // ���[�v���� (�_���[�W�G�t�F�N�g�̐���)
                        Instantiate(DEAD_EFECT, MakePoint, this.transform.rotation);
                    }
                }
            }
            else
            {                
                CoolDownTime = FindObjectOfType<GameManeger>().OneTempo * 2;
                time += Time.deltaTime;
                if (time == -1)
                {
                    time = 10;
                }
                switch (BossType)
                {
                    case BossNumber.No1:
                        FirstBoss();
                        break;
                    case BossNumber.No2:
                        SecondBoss();
                        break;
                }
                Damage();

                gapPos = Mathf.Atan2((PlayerObject.transform.position.x - this.transform.position.x), (PlayerObject.transform.position.y - this.transform.position.y));
                gapfixPos = gapPos * Mathf.Rad2Deg;
                if (gapfixPos < 0)
                {
                    gapfixPos += 360;
                }

                // ���S����
                if (isDead)
                {
                    // ��x��������������e������
                    if (!DeadFix)
                    {
                        // �{�X���~��Ԃɂ���
                        ActPermission = false;
                        // ��ʂ�h�炷
                        GameManeger.shakeTime = 0.25f;
                        ShakeManeger.ShakeLevel = 4;
                        // ����уA�j���[�V�����𔭐�������
                        isDeadFlash.SetActive(true);
                        // �X�R�A�𑫂�
                        GameManeger.KillBOSS += 5000;
                        // �����������������Ƃ��L�^����
                        DeadFix = true;
                    }

                    // �ʒu�Œ�
                    rb2d.velocity = Vector3.zero;

                    // �q�I�u�W�F�N�g�̍폜
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
                    if (BossType == BossNumber.No2)
                    {
                        anim.SetBool("Hide", false);

                    }

                    // ����уA�j���[�V�������I�������
                    if (isDeadFlash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FlashEND"))
                    {
                        // �G�t�F�N�g��10�񐶐�����܂Ń��[�v����
                        if (MakeEfectCount < 10)
                        {
                            MakePoint.x = Random.Range(this.transform.position.x - 1, this.transform.position.x + 1);
                            MakePoint.y = Random.Range(this.transform.position.y - 1, this.transform.position.y + 1);
                            // ���[�v���� (�_���[�W�G�t�F�N�g�̐���)
                            Instantiate(DEAD_EFECT, MakePoint, this.transform.rotation);
                            MakeEfectCount += 1;
                        }
                        else
                        {
                            Instantiate(DEAD_EFECT, this.transform.position, this.transform.rotation);
                            // �{�X���\���ɂ���
                            SpR.enabled = false;
                            Col2D.enabled = false;
                            //anim.enabled = false;
                            // �����J����
                            FindObjectOfType<BossStartFlag>().ActEnd = true;
                            // ���S�������I������
                            isDead = false;
                        }
                    }
                }
                else
                {
                    ShotRote.transform.rotation = Quaternion.Euler(0, 0, -1 * gapfixPos);
                }
            }            
        }
    }

    void Damage()
    {
        MutekiTime += Time.deltaTime;
        if (MutekiTime > 1)
        {
            AlphaExchange = false;
            SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, 1);
            if (IsAttack)
            {
                if (inPlayerAttackRange)
                {
                    // �W���X�g�A�^�b�N�̃^�C�~���O�Ȃ�
                    if (blowable)
                    {
                        // �N�[���_�E�����񕜂������ǂ���
                        if (CoolDownTime <= time)
                        {
                            FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
                            //����
                            shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                            if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                            { shotIt.x = Mathf.Sign(shotrote.x); }
                            else
                            { shotIt.x = 0; }
                            if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                            { shotIt.y = Mathf.Sign(shotrote.y); }
                            else
                            { shotIt.y = 0; }
                            // ���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                            rb2d.AddForce(shotIt * ForcePoint);

                            Audio.clip = isBlowSE;
                            Audio.Play();
                            Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                            BossHP = BossHP - 1;
                            time = 0;
                            MutekiTime = 0;
                            Camera.ShakeOrder = true;
                            GameManeger.KillBOSS += 1000;

                            if (BossHP <= 0)
                            {
                                isDead = true;
                            }
                            else
                            {
                                GameManeger.shakeTime = 0.125f;
                                ShakeManeger.ShakeLevel = 2;
                            }

                        }
                    }
                }
            }

        }
        else
        {
            if (!AlphaExchange)
            {
                SpR.color = new Color(SpR.color.r, SpR.color.g, SpR.color.b, SpR.color.a - Time.deltaTime * 8);
                if (SpR.color.a <= 0)
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

    void FirstBoss()
    {
        // �s�������~��Ă���̂�
        if (ActPermission)
        {
            // �A�N�V�����^�C�~���O�ɓ�������
            if (ExChange)
            {
                fix = false;
                ExChange = false;

                // �A�N�V�����J�E���g�ɉ����čs����ω�������
                switch (ActCounter)
                {
                    case 1:
                        rb2d.velocity = MoveInt * 4;
                        SpR.sprite = BossSprite[0];
                        break;
                    case 2:
                        rb2d.velocity = MoveInt * 4;
                        SpR.sprite = BossSprite[1];

                        Ramdom1st = Random.Range(0, 3);
                        Ramdom2nd = Random.Range(0, 3);
                        break;
                    case 3:
                        rb2d.velocity = Vector2.zero;
                        if(LoopCounter == 6)
                        {
                            Instantiate(EnemyBullet[3], this.transform.position, ShotRote.transform.rotation);
                        }
                        else if (LoopCounter == 3)
                        {
                            // �s���Ȃ�
                        }
                        else
                        {
                            Instantiate(EnemyBullet[Ramdom1st], this.transform.position, ShotRote.transform.rotation);
                            if (Ramdom1st == 0)
                            {
                                Ramdom2nd = Random.Range(1, 3);
                            }
                            if (Ramdom1st == 2)
                            {
                                Ramdom2nd = Random.Range(0, 2);
                            }
                        }
                        SpR.sprite = BossSprite[2];
                        break;
                    case 4:
                        rb2d.velocity = Vector2.zero;
                        if (LoopCounter == 6)
                        {
                            // �s���Ȃ�
                            LoopCounter = 0;
                        }
                        else�@if (LoopCounter == 3)
                        {
                            Instantiate(EnemyBullet[3], this.transform.position, ShotRote.transform.rotation);
                        }
                        else
                        {
                            Instantiate(EnemyBullet[Ramdom2nd], this.transform.position, ShotRote.transform.rotation);
                        }
                        SpR.sprite = BossSprite[1];
                        break;
                }
            }
            else
            {
                // �������E�l�̍Đݒ�
                if (!fix)
                {
                    // �A�N�V�����J�E���g���㏸������
                    ActCounter++;
                    if (ActCounter == 5)
                    { ActCounter = 1;
                      LoopCounter++;}
                    fix = true;

                    RandumCount = Random.Range(1, 5);

                    switch (RandumCount)
                    {
                        case 1:
                            MoveInt = new Vector2(1, 0);
                            break;
                        case 2:
                            MoveInt = new Vector2(-1, 0);
                            break;
                        case 3:
                            MoveInt = new Vector2(0, 1);
                            break;
                        case 4:
                            MoveInt = new Vector2(0, -1);
                            break;
                    }
                }
            }
        }
    }

    void SecondBoss()
    {

        // �s�������~��Ă���̂�
        if (ActPermission)
        {
            animSpeed = GameManeger.AnimSpeed;
            anim.SetFloat("AnimSpeed", animSpeed);
            // �A�N�V�����^�C�~���O�ɓ�������
            if (ExChange)
            {
                fix = false;
                ExChange = false;
                Debug.Log("ActCounter = " + ActCounter);
                switch (ActCounter)
                {
                    case 1:
                        anim.SetBool("Hide", true);
                        if (!move)
                        {
                            move = true;
                            switch (RandumCount)
                            {
                                case 1:
                                    transform.position = new Vector2(-21.47f, -57.95f);

                                    break;
                                case 2:
                                    transform.position = new Vector2(-16.47f, -57.95f);
                                    break;
                                case 3:
                                    transform.position = new Vector2(-11.47f, -57.95f);
                                    break;
                            }
                        }
                        break;
                    case 2:
                        move = false; 
                        anim.SetBool("Hide", false);
                        switch (RandumCount)
                        {
                            case 1:
                                Instantiate(EnemyBullet[Ramdom2nd], this.transform.position, ShotRote.transform.rotation);
                                anim.SetBool("Shot", true);
                                break;
                            case 2:
                                // �ߐڍU��
                                anim.SetBool("ATK", true);
                                break;
                            case 3:
                                Instantiate(EnemyBullet[Ramdom2nd], this.transform.position, ShotRote.transform.rotation);
                                anim.SetBool("Shot", true);
                                break;
                        }
                        rb2d.velocity = Vector2.zero;

                        break;
                    case 3:
                        rb2d.velocity = Vector2.zero;
                        break;
                    case 4:
                        rb2d.velocity = Vector2.zero;
                        break;
                }
            }
            else
            {
                // �������E�l�̍Đݒ�
                if (!fix)
                {
                    anim.SetBool("ATK", false);
                    anim.SetBool("Shot", false);

                    if (ActCounter == 1)
                    {
                        anim.SetBool("Hide", true);
                    }
                    // �A�N�V�����J�E���g���㏸������
                    ActCounter++;
                    if (ActCounter == 5)
                    {
                        ActCounter = 1;
                        LoopCounter++;
                    }
                    fix = true;

                    // �ړ����ς���
                    RandumCount = Random.Range(1, 4);

                }
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
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            inPlayerAttackRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // �s����Ԃ���
        if (ActPermission)
        {
            // �G�l�~�[�o���b�g���Ԃ�������
            if (col.gameObject.CompareTag("EnemyBullet"))
            {
                // ���ˏ�Ԃ��m�F����
                if (col.gameObject.GetComponent<EnemyBullet>().isHIT == true)
                {
                    // �N�[���_�E�����񕜂������ǂ���
                    if (CoolDownTime <= time)
                    {
                        //����
                        shotrote = new Vector2(this.transform.position.x - col.gameObject.transform.position.x, this.transform.position.y - col.gameObject.transform.position.y);
                        if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                        { shotIt.x = Mathf.Sign(shotrote.x); }
                        else
                        { shotIt.x = 0; }
                        if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                        { shotIt.y = Mathf.Sign(shotrote.y); }
                        else
                        { shotIt.y = 0; }
                        // ���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                        rb2d.AddForce(shotIt * ForcePoint);

                        Audio.clip = isBlowSE;
                        Audio.Play();
                        Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                        BossHP = BossHP - 1;
                        time = 0;
                        MutekiTime = 0;
                        Camera.ShakeOrder = true;
                        GameManeger.KillBOSS += 1000;
                        if (BossHP <= 0)
                        {
                            isDead = true;
                        }
                    }
                }
            }

            // �v���C���[�o���b�g������������
            if (col.gameObject.CompareTag("PlayerBullet"))
            {
                // �N�[���_�E�����񕜂������ǂ���
                if (CoolDownTime <= time)
                {
                    //����
                    shotrote = new Vector2(this.transform.position.x - col.gameObject.transform.position.x, this.transform.position.y - col.gameObject.transform.position.y);
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    // ���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * ForcePoint);

                    Audio.clip = isBlowSE;
                    Audio.Play();
                    Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                    BossHP = BossHP - 1;
                    time = 0;
                    MutekiTime = 0;
                    Camera.ShakeOrder = true;
                    GameManeger.KillBOSS += 1000;
                    if (BossHP <= 0)
                    {
                        isDead = true;
                    }

                }
            }
        }
    }
}
