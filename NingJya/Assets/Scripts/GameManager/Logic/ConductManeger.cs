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
        if (conduct)
        {
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
        tagName = CTobject.tag;

        foreach (var ctobj in Enemys)
        {
            if (ctobj.gameObject.name != CTobject.gameObject.name)
            {
                Gap = new Vector2(ctobj.transform.position.x - CTobject.transform.position.x, ctobj.transform.position.y - CTobject.transform.position.y);
                float vec = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y); ;

                if (tagName == "PlayerBullet")
                {
                    if (vec < 10)
                    {
                        gapPos = Mathf.Atan2((PlayerObj.transform.position.x - ctobj.transform.position.x), (PlayerObj.transform.position.y - ctobj.transform.position.y));
                        gapfixPos = gapPos * Mathf.Rad2Deg;
                        if (gapfixPos < 10)
                        {
                            gapfixPos += 360;
                        }
                        PlayerRoteObject.transform.rotation = Quaternion.Euler(0, 0, -1 * gapfixPos);
                        ConductCount++;
                        
                        if (ConductCount >= 5)
                        {
                            ConductCount = 0;
                            break;
                        }
                    }
                    else
                    {
                        break ;
                    }
                }
                else if (tagName == "EnemyBullet")
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
                        }
                    }
                }
                else if (tagName == "Enemy")
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
