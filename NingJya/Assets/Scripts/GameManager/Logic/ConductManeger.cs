using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductManeger : MonoBehaviour
{

    public GameObject[] Enemys;     // 全敵オブジェクトの情報をを保存する変数

    private Vector2 Gap;            // 目標オブジェクトと対象オブジェクトの距離差を保存する変数
    public Vector2 conductObjGap;
    public bool conduct;
    public GameObject chackObj;

    [SerializeField] private float EnemyobjctDistance;
    [SerializeField] private float MustEnemyobjctDistance;
    public GameObject CTobject;
    private int CTcount;
    public GameObject targetObj;

    [SerializeField] private GameObject PlayerObj;

    public static List<GameObject> EnemyList = new List<GameObject>();
    string tagName;

    private float gapPos;
    private float gapfixPos;
    private int ConductCount;
    [SerializeField] private GameObject  PlayerRoteObject;

    void Start()
    {
        MustEnemyobjctDistance = 20;
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        // 他オブジェクトへの吹っ飛ばし処理の誘導を求められたら
        if (conduct)
        {
            // 全オブジェクトとの距離差の計算を行う
            gapChack();
        }
        else
        {
            transform.position = Vector3.zero;
            MustEnemyobjctDistance = 20;
            targetObj = null;
        }
    }

    public void gapChack()
    {
        // 他オブジェクトで吹っ飛ばし処理を行われたら
        // 吹っ飛ばし対象のタグ名を取得する
        tagName = CTobject.tag;

        // 全敵オブジェクトから最も近い距離にいる敵を取得する
        foreach (var ctobj in Enemys)
        {
            // ふっとばし対象のオブジェクトじゃない場合
            if (ctobj.gameObject.name != CTobject.gameObject.name)
            {
                // そのオブジェクトとの距離差を求める
                Gap = new Vector2(ctobj.transform.position.x - CTobject.transform.position.x, ctobj.transform.position.y - CTobject.transform.position.y);
                float vec = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y); ;

                // ふっとばし対象のオブジェクトがPlayerBulletの場合
                if (tagName == "PlayerBullet")
                {
                    // 距離差が10いないと場合
                    if (vec < 10)
                    {
                        // 対象オブジェクトの居る座標とふっとばし対象の座標をもとに、方角を求める
                        gapPos = Mathf.Atan2((PlayerObj.transform.position.x - ctobj.transform.position.x),
                                            (PlayerObj.transform.position.y - ctobj.transform.position.y));

                        gapfixPos = gapPos * Mathf.Rad2Deg;
                        if (gapfixPos < 10)
                        {
                            gapfixPos += 360;
                        }
                        // 求めた方角に向かってRotetion.zを変更する
                        PlayerRoteObject.transform.rotation = Quaternion.Euler(0, 0, -1 * gapfixPos);
                        // この処理を5秒続ける
                        ConductCount++;
                        
                        // 5秒後、このループ処理から抜け出す
                        if (ConductCount >= 5)
                        {
                            ConductCount = 0;
                            break;
                        }
                    }
                    else
                    {
                        // 対象オブジェクトがない場合、このループから抜け出す
                        break ;
                    }
                }
                // ふっとばし対象のオブジェクトがEnemy、もしくはEnemyBulletの場合
                else if ((tagName == "Enemy") || (tagName == "EnemyBullet"))
                {
                    // 現在の最低距離差よりも現在の距離差が少ない場合
                    if (vec < MustEnemyobjctDistance)
                    {
                        // かつ、その敵オブジェクトが死亡状態ではない場合
                        if (ctobj.GetComponent<Collider2D>().enabled == true)
                        {
                            // かつ、その敵オブジェクトが吹っ飛ばし状態でない場合
                            if (!(ctobj.GetComponent<Enemy>().actTime > 0))
                            {
                                // 新しい対象オブジェクトに標的を更新する
                                targetObj = ctobj;
                                // その新しい対象オブジェクトとの距離差を、最低距離差として保存する
                                MustEnemyobjctDistance = Mathf.Abs(vec);
                            }
                        }
                    }
                }
                else
                {
                    if (vec < MustEnemyobjctDistance)
                    {
                        if (ctobj.GetComponent<Collider2D>().enabled == true)
                        {
                            if (!(ctobj.GetComponent<Enemy>().actTime > 0))
                            {
                                targetObj = ctobj;
                                MustEnemyobjctDistance = Mathf.Abs(vec);
                            }
                            else
                            {

                            }
                        }
                    }
                }

            }
        }

        if (targetObj == null)
        {
            //targetObj = PlayerObj;
        }

        switch (tagName)
        {
            case "Enemy":
                if (CTobject.GetComponent<Enemy>().conductIt == true)
                {
                    CTobject.GetComponent<Enemy>().conductObject = targetObj;
                    CTobject.GetComponent<Enemy>().Ready = true;
                    conduct = false;
                }
                break;
            case "HitObj":
                CTobject.GetComponent<Objects>().conductObject = targetObj;
                CTobject.GetComponent<Objects>().Ready = true;
                conduct = false;
                break;
            case "EnemyBullet":
                CTobject.GetComponent<EnemyBullet>().conductObject = targetObj;
                CTobject.GetComponent<EnemyBullet>().Ready = true;
                conduct = false;
                break;
        }

    }
}
