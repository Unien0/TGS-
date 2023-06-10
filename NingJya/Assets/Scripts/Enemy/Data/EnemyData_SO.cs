using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Data/Enemy Data")]
public class EnemyData_SO : ScriptableObject
{
    [Header("基本属性")]
    //HP
    public float Hp;
    //移動速度
    public float Speed;
    //ダメージ
    public float damage;

    [Header("敵の特別数値")]
    //ぶっ飛ばすの時間
    public float blowTime;
    //ぶっ飛ばすの距離
    public float blowDistance;
    //死亡後の存在時間
    public float existenceTime;

    [Header("キャラクター状態")]
    //死んだのか
    public bool isDead;
    //攻撃できる
    public bool attackable;
    //移動できる
    public bool removable;
    //ぶっ飛ばすできる
    public bool blowable;
    //ぶっ飛ばす中
    public bool beingBlow;

}
