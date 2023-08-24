using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Player Data")]
public class PlayerData_SO : ScriptableObject
{
    [Header("基本プロパティ")]
    //MaxHP
    public int maxHp;
    //HP
    public int hp;
    //スピード
    public float speed;
    //ダメージ
    public int damage;
    //攻撃CD
    public float attackCD;
    // 手裏剣最大数
    public int maxBullet;
    // 所持手裏剣数
    public int nowBullet;

    [Header("リズム相関")]
    //リズム時間
    public float beatLengh;

    public bool levelling;

    [Header("吹っ飛ばすに関するデータ")]
    //吹っ飛ばすの力
    public float force;


    [Header("ロールステータス")]
    //死亡したかどうか
    public bool isDead;
    //攻撃できるかどうか
    public bool attackable;
    //移動可能かどうか
    public bool removable;
    // 無敵時間
    public bool mutekki;
}
