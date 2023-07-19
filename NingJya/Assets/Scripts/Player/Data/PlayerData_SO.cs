using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Player Data")]
public class PlayerData_SO : ScriptableObject
{
    [Header("基本プロパティ")]
    //MaxHP
    public float maxHp;
    //HP
    public float hp;
    //スピード
    public float speed;
    //ダメージ
    public float damage;
    //攻撃CD
    public float attackCD;

    [Header("リズム相関")]
    //リズム時間
    public float beatLengh;

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
