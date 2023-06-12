using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData����
    private float playerHp
    {
        //playerData���Ф�hp�򥲥åȤ��ơ��Է֤Ή����ˤʤ�;�⤷̽���ʤ����0�ˤʤ�
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }//play�򤢤���player��HP��p�餹
    }
    private float playerDamage
    {
        //playerData���Ф�damage�򥲥åȤ��ơ��Է֤Ή����ˤʤ�;�⤷̽���ʤ����0�ˤʤ�
        get { if (playerData != null) return playerData.damage; else return 0; }

    }
    private float yellowForce
    {
        //�ꥺ���Ϥ�ʤ�����ɫ���ꥢ�ΤȤ�
        get { if (playerData != null) return playerData.force; else return 0; }
    }
    private float whiteForce
    {
        //�ꥺ��Ϥ��Ȥ�
        get { if (playerData != null) return playerData.force * 4; else return 0; }
    }
    private float redForce
    {
        //�२�ꥢ�ΤȤ�
        get { if (playerData != null) return playerData.force * 8; else return 0; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyData����
    private float enemyHp
    {
        //enemyData�򥲥å�
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemyDamage
    {
        //enemyData�򥲥å�
        get { if (enemyData != null) return enemyData.damage; else return 0; }
        set { enemyData.damage = value; }
    }
    private float yellowExistenceTime
    {
        //enemyData�򥲥å�
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }
    //�ꥺ���Ϥ碌�Ɣ��򚢤����4���δ��ڕr�g������ޤ�
    private float whiteExistenceTime
    {
        //enemyData�򥲥å�
        get { if (enemyData != null) return enemyData.existenceTime*4; else return 0; }

    }
    //�२�ꥢ�ǔ��򚢤����8���δ��ڕr�g������ޤ�
    private float redExistenceTime
    {
        //enemyData�򥲥å�
        get { if (enemyData != null) return enemyData.existenceTime*8; else return 0; }

    }


    private bool attackable
    {
        //penemyData�򥲥å�
        get { if (enemyData != null) return enemyData.attackable; else return false; }
    }
    private bool removable
    {
        //penemyData�򥲥å�
        get { if (enemyData != null) return enemyData.removable; else return false; }
    }
    private bool blowable
    {
        //penemyData�򥲥å�
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    private bool beingBlow
    {
        //penemyData�򥲥å�
        get { if (enemyData != null) return enemyData.beingBlow; else return false; }
    }
    #endregion


    private Rigidbody2D rb2d;
    private bool ATKAREA = false;
    private GameObject PlayerObject;
    private Vector2 moveint;
    // moveint�̌��ʂ𐳕��݂̂̒l�ɂ���
    private Vector2 moveit;
    // �s���܂ł̑ҋ@����
    private float actTime;
    // �ړ������̒�~
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

        // ��b�o�ߎ�
        if (actTime >= 2.0f)
        {
            // ��~�������s���Ă��Ȃ��Ȃ�
            if (!stop)
            {
                // �ړ�����
                moveint = new Vector2(PlayerObject.transform.position.x - transform.position.x, PlayerObject.transform.position.y - transform.position.y);
                moveit.x = Mathf.Sign(moveint.x);
                moveit.y = Mathf.Sign(moveint.y);
                rb2d.velocity = moveit * 5;

                // ��~�E����������
                if (actTime >= 2.25f)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.angularVelocity = 0;
                    actTime = 0;
                }
            }
            else actTime = 0; ;
        }

        // �v���C���[�̍U���͈͂ɂ���Ƃ�
        if (ATKAREA)
        {
            // �v���C���[���U�����͂�������
            if (FindObjectOfType<Player>().ATK)
            {
                // �ړ��������s��Ȃ��悤�ɂ���
                stop = true;
                fix = false;
                if (!shoted)
                {
                    // �ӂ��Ƃ΂�
                    shoted = true;
                    shotrote = new Vector2(this.transform.position.x - PlayerObject.transform.position.x, this.transform.position.y - PlayerObject.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    rb2d.AddForce(shotrote * 10, ForceMode2D.Impulse);
                }

            }
        }

        // �ӂ��Ƃ΂���~����
        // �U�����͂�����Ă��Ȃ���
        if (FindObjectOfType<Player>().ATK == false)
        {
            stop = false;
            // �C������������Ă��Ȃ��Ȃ��~����B
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
    /// ���򴵤��w�Ф����ꥢ���ж�
    /// </summary>
    private void BlowAway()
    {
        //1������λ�ä���ɫ���ס���Υ��ꥢ�ڤˤ��뤫�ɤ������жϤ���
        //2���ץ쥤��`�Ϲ��Ĥ��������жϤ���
        //3���ץ쥤��`�Ȥ�λ�äˤ�äƴ����w�Ф������Q���
        //4���F��λ�ä˻��Ť��ƴ����w�Ф������ȱ���r�g���жϤ��ޤ�
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ������ꍇ
        if (col.CompareTag("Player"))
        {
            ATKAREA = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂��甲�����ꍇ
        if (col.CompareTag("Player"))
        {
            ATKAREA = false;
        }
    }
}
