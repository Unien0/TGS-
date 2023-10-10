using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductManeger : MonoBehaviour
{

    public GameObject[] Enemys;     // �S�G�I�u�W�F�N�g�̏�����ۑ�����ϐ�

    private Vector2 Gap;            // �ڕW�I�u�W�F�N�g�ƑΏۃI�u�W�F�N�g�̋�������ۑ�����ϐ�
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
        // ���I�u�W�F�N�g�ւ̐�����΂������̗U�������߂�ꂽ��
        if (conduct)
        {
            // �S�I�u�W�F�N�g�Ƃ̋������̌v�Z���s��
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
        // ���I�u�W�F�N�g�Ő�����΂��������s��ꂽ��
        // ������΂��Ώۂ̃^�O�����擾����
        tagName = CTobject.tag;

        // �S�G�I�u�W�F�N�g����ł��߂������ɂ���G���擾����
        foreach (var ctobj in Enemys)
        {
            // �ӂ��Ƃ΂��Ώۂ̃I�u�W�F�N�g����Ȃ��ꍇ
            if (ctobj.gameObject.name != CTobject.gameObject.name)
            {
                // ���̃I�u�W�F�N�g�Ƃ̋����������߂�
                Gap = new Vector2(ctobj.transform.position.x - CTobject.transform.position.x, ctobj.transform.position.y - CTobject.transform.position.y);
                float vec = Mathf.Sqrt(Gap.x * Gap.x + Gap.y * Gap.y); ;

                // �ӂ��Ƃ΂��Ώۂ̃I�u�W�F�N�g��PlayerBullet�̏ꍇ
                if (tagName == "PlayerBullet")
                {
                    // ��������10���Ȃ��Əꍇ
                    if (vec < 10)
                    {
                        // �ΏۃI�u�W�F�N�g�̋�����W�Ƃӂ��Ƃ΂��Ώۂ̍��W�����ƂɁA���p�����߂�
                        gapPos = Mathf.Atan2((PlayerObj.transform.position.x - ctobj.transform.position.x),
                                            (PlayerObj.transform.position.y - ctobj.transform.position.y));

                        gapfixPos = gapPos * Mathf.Rad2Deg;
                        if (gapfixPos < 10)
                        {
                            gapfixPos += 360;
                        }
                        // ���߂����p�Ɍ�������Rotetion.z��ύX����
                        PlayerRoteObject.transform.rotation = Quaternion.Euler(0, 0, -1 * gapfixPos);
                        // ���̏�����5�b������
                        ConductCount++;
                        
                        // 5�b��A���̃��[�v�������甲���o��
                        if (ConductCount >= 5)
                        {
                            ConductCount = 0;
                            break;
                        }
                    }
                    else
                    {
                        // �ΏۃI�u�W�F�N�g���Ȃ��ꍇ�A���̃��[�v���甲���o��
                        break ;
                    }
                }
                // �ӂ��Ƃ΂��Ώۂ̃I�u�W�F�N�g��Enemy�A��������EnemyBullet�̏ꍇ
                else if ((tagName == "Enemy") || (tagName == "EnemyBullet"))
                {
                    // ���݂̍Œ዗�����������݂̋����������Ȃ��ꍇ
                    if (vec < MustEnemyobjctDistance)
                    {
                        // ���A���̓G�I�u�W�F�N�g�����S��Ԃł͂Ȃ��ꍇ
                        if (ctobj.GetComponent<Collider2D>().enabled == true)
                        {
                            // ���A���̓G�I�u�W�F�N�g��������΂���ԂłȂ��ꍇ
                            if (!(ctobj.GetComponent<Enemy>().actTime > 0))
                            {
                                // �V�����ΏۃI�u�W�F�N�g�ɕW�I���X�V����
                                targetObj = ctobj;
                                // ���̐V�����ΏۃI�u�W�F�N�g�Ƃ̋��������A�Œ዗�����Ƃ��ĕۑ�����
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
