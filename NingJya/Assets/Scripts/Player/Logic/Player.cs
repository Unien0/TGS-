using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData����
    private float maxHp
    {
        //playerData���Ф�maxHp�򥲥åȤ��ơ��Է֤Ή����ˤʤ�;�⤷̽���ʤ����0�ˤʤ�
        get { if (playerData != null) return playerData.maxHp; else return 0; }

    }
    private float hp
    {
        //playerData���Ф�hp�򥲥åȤ��ơ��Է֤Ή����ˤʤ�;�⤷̽���ʤ����0�ˤʤ�
        get { if (playerData != null) return playerData.hp; else return 0; }

    }
    private float damage
    {
        //playerData���Ф�damage�򥲥åȤ��ơ��Է֤Ή����ˤʤ�;�⤷̽���ʤ����0�ˤʤ�
        get { if (playerData != null) return playerData.damage; else return 0; }
       
    }
    private float speed
    {
        //playerData���Ф�speed�򥲥åȤ��ơ��Է֤Ή����ˤʤ�;�⤷̽���ʤ����0�ˤʤ�
        get { if (playerData != null) return playerData.speed; else return 0; }

    }
   
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return false; }
        set { playerData.isDead = value; }//set���Է֤Ή�����playerData���ͤ�
    }
    public bool attackable
    {
        get { if (playerData != null) return playerData.attackable; else return false; }
        set { playerData.attackable = value; }//set���Է֤Ή�����playerData���ͤ�
    }
    public bool removable
    {
        get { if (playerData != null) return playerData.removable; else return false; }
        set { playerData.removable = value; }//set���Է֤Ή�����playerData���ͤ�
    }

    #endregion 

    public Vector2 shotrote;
    private GameObject Enemyobj;
    public bool ATK;
    private float time;

    private void Awake()
    {
            
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (isDead)
        {
            canATK();
        }
        

        
    }



    void canATK()
    {
        if (Input.GetKeyDown(KeyCode.E))
            {
            time += Time.deltaTime;
            shotrote = new Vector2(Enemyobj.transform.position.x - this.transform.position.x, Enemyobj.transform.position.y - this.transform.position.y);

            //if (time >= 1)
            //{
            //    time = 0;
            //    ATK = false;
            //}
        //if (ATK)
        //{
            
        //    }
        }
    }




    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemyobj = col.gameObject;
        }
    }
}
