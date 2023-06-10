using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region PlayerData変数
    private float playerHp
    {
        //playerDataの中にhpをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }//playをあたらplayerのHPを減らす
    }
    private float playerDamage
    {
        //playerDataの中にdamageをゲットして、自分の変数になる;もし探さなければ0になる
        get { if (playerData != null) return playerData.damage; else return 0; }

    }
    private float yellowForce
    {
        //リズムを合わない、黄色エリアのとき
        get { if (playerData != null) return playerData.force; else return 0; }
    }
    private float whiteForce
    {
        //リズム合うとき
        get { if (playerData != null) return playerData.force * 4; else return 0; }
    }
    private float redForce
    {
        //赤エリアのとき
        get { if (playerData != null) return playerData.force * 8; else return 0; }
    }
    #endregion 

    public EnemyData_SO enemyData;
    #region EnemyData変数
    private float enemyHp
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.Hp; else return 0; }
        
    }
    private float enemyDamage
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.damage; else return 0; }
        set { enemyData.damage = value; }
    }
    private float yellowExistenceTime
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.existenceTime; else return 0; }

    }
    //リズムを合わせて敵を殺すれば4倍の存在時間があります
    private float whiteExistenceTime
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.existenceTime*4; else return 0; }

    }
    //赤エリアで敵を殺すれば8倍の存在時間があります
    private float redExistenceTime
    {
        //enemyDataをゲット
        get { if (enemyData != null) return enemyData.existenceTime*8; else return 0; }

    }


    private bool attackable
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.attackable; else return false; }
    }
    private bool removable
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.removable; else return false; }
    }
    private bool blowable
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.blowable; else return false; }
    }
    private bool beingBlow
    {
        //penemyDataをゲット
        get { if (enemyData != null) return enemyData.beingBlow; else return false; }
    }
    #endregion


    private Rigidbody2D rb2d;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Player>().ATK)
        {
            rb2d.AddForce(FindObjectOfType<Player>().shotrote,ForceMode2D.Impulse);
        }
        else
        {
            rb2d.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 敵を吹っ飛ばすエリアの判定
    /// </summary>
    private void BlowAway()
    {
        //1、敵の位置が黄色、白、赤のエリア内にあるかどうかを判断する
        //2、プレイヤーは攻撃したかを判断する
        //3、プレイヤーとの位置によって吹っ飛ばす方向を決める
        //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
    }
}
