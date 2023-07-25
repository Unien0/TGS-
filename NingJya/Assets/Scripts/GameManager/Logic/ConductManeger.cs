using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductManeger : MonoBehaviour
{
    public GameObject[] Enemys;
    private int INCount = 0;
    public bool noneCol;
    int Flow;
    Vector2 Gap;
    Vector2 GapSign;
    public Vector2 conductObjGap;
    public bool conduct;
    public GameObject chackObj;

    [SerializeField] private float EnemyobjctDistance;
    [SerializeField] private float MustEnemyobjctDistance;
    public GameObject CTobject;
    private int CTcount;
    public GameObject targetObj;

    public static List<GameObject> EnemyList = new List<GameObject>();
    string tagName;

    void Start()
    {
        MustEnemyobjctDistance = 5;
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

            #region
            //do
            //{
            //    Vector2 Gap = new Vector2(Enemys[CTcount - 1].transform.position.x - CTobject.transform.position.x, Enemys[CTcount - 1].transform.position.y - CTobject.transform.position.y);
            //    EnemyobjctDistance = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y);

            //    if (EnemyobjctDistance < MustEnemyobjctDistance)
            //    {
            //        targetObj = Enemys[CTcount - 1].gameObject;
            //        MustEnemyobjctDistance = EnemyobjctDistance;
            //    }
            //    CTcount--;
            //} while (CTcount < 0);

            /*if (CTcount < 0)
            {
                MustEnemyobjctDistance = 5;
                conduct = false;
                reday = true;
            }*/
            #endregion
        }
        else
        {
            MustEnemyobjctDistance = 5;
            targetObj = null;
        }
    }

    public void gapChack()
    {
        foreach (var ctobj in Enemys)
        {
            if (ctobj.gameObject.name != CTobject.gameObject.name)
            {
                Gap = new Vector2(ctobj.transform.position.x - CTobject.transform.position.x, ctobj.transform.position.y - CTobject.transform.position.y);
                float vec = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y); ;

                if (vec < MustEnemyobjctDistance)
                {
                    if (ctobj.GetComponent<Collider2D>().enabled == true)
                    {
                        targetObj = ctobj;
                        MustEnemyobjctDistance = Mathf.Abs(vec);
                    }
                }
            }
        }

        tagName = CTobject.tag;

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
            case"HitObj":
                CTobject.GetComponent<Objects>().conductObject = targetObj;
                CTobject.GetComponent<Objects>().Ready = true;
                conduct = false;
                break; 
        }

    }
}
