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
