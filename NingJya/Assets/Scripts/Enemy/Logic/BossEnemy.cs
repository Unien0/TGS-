using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
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

    #endregion
    [SerializeField] private Vector2 shotIt;
    [SerializeField] private float ForcePoint;
    public bool ActPermission;
    private int ActCounter;
    private bool fix;
    private int RandumCount;
    private Vector2 MoveInt;
    public bool ExChange;
    [SerializeField] GameObject EnemyBullet;
    [SerializeField] private Sprite[] BossSprite;
    [SerializeField] private AudioClip isBlowSE;
    [SerializeField] private GameObject Hit_Efect;
    [SerializeField] private GameObject DEAD_EFECT;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SpR = GetComponent<SpriteRenderer>();
        Col2D = GetComponent<Collider2D>();
        Audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (FindObjectOfType<BossStartFlag>().ActStart)
        {
            Damage();
            switch (BossType)
            {
                case BossNumber.No1:
                    if (FindObjectOfType<BossStartFlag>().ActStart == true)
                    { FirstBoss(); }
                    break;
            }
        }
    }

    void Damage()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 1")))
        {
            if (inPlayerAttackRange)
            {
                // �W���X�g�A�^�b�N�̃^�C�~���O�Ȃ�
                if (blowable)
                {
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
                    //4�A���݈ʒu�Ɋ�Â��Đ�����΂��̗͂ƕۑ����Ԃ𔻒f���܂�
                    rb2d.AddForce(shotIt * ForcePoint);
                    FindObjectOfType<Player>().KATANA.GetComponent<Animator>().SetBool("ATK", true);
                    Audio.clip = isBlowSE;
                    Audio.Play();                   
                    Instantiate(Hit_Efect, this.transform.position, this.transform.rotation);
                    BossHP = BossHP - 1;
                    if (BossHP <= 0)
                    {
                        FindObjectOfType<BossStartFlag>().ActEnd = true;
                        ActPermission = false;
                        SpR.enabled = false;
                        Col2D.enabled = false;
                        rb2d.velocity = Vector3.zero;
                        GameManeger.KillBOSS++;
                        Instantiate(DEAD_EFECT, this.transform.position, this.transform.rotation);
                    }
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
                        break;
                    case 3:
                        rb2d.velocity = Vector2.zero;
                        Instantiate(EnemyBullet,this.transform);
                        SpR.sprite = BossSprite[2];
                        break;
                    case 4:
                        rb2d.velocity = Vector2.zero;
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
                    {ActCounter = 1;}
                    fix = true;

                    RandumCount = Random.Range(1, 5);
                    Debug.Log(RandumCount);
                    switch (RandumCount)
                    {
                        case 1:
                            MoveInt = new Vector2(1, 0);
                            break;
                        case 2:
                            MoveInt = new Vector2(-1, 0);
                            break;
                        case 3:
                            MoveInt = new Vector2(0,1);
                            break;
                        case 4:
                            MoveInt = new Vector2(0, -1);
                            break;
                    }
                }
            }
        }

    }
}
