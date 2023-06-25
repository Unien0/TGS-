using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData�ϐ�
    private float maxHp
    {
        //Player��MaxHP���Q�b�g����
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    private float hp
    {
        //Player��HP���Q�b�g����
        get { if (playerData != null) return playerData.hp; else return 0; }

    }
    private float damage
    {
        //Player�̃_���[�W�l���擾����
        get { if (playerData != null) return playerData.damage; else return 0; }
       
    }
    private float speed
    {
        //Player�̃X�s�[�h�l���擾����
        get { if (playerData != null) return playerData.speed; else return 0; }

    } 
    private float attackCD
    {
        //Player��CD���擾����
        get { if (playerData != null) return playerData.attackCD; else return 1; }

    }
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return false; }
        set { playerData.isDead = value; }//�v���C���[�����S�����ꍇ�ɍX�V
    }
    public bool attackable
    {
        //�v���C���[���U���ł��邩�ǂ����𔻒f����
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }
    }
    public bool removable
    {
        //�v���C���[���ړ��ł��邩�ǂ����𔻒f����
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

            // �U������
            if (Input.GetKeyDown(KeyCode.E))
            {
                attackable = false;
            }

            // �U������
            if (!attackable)
            {
                // �N�[���_�E���̊m�F
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
        // �ړ����� 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // �ړ�����
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
