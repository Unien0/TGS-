using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public EnemyData_SO enemyData;
    #region EnemyData�̕ϐ�
    private bool enemyRemovable
    {
        //�ړ��\���ǂ����𔻒f����
        get { if (enemyData != null) return enemyData.removable; else return false; }
        set { enemyData.removable = value; }
    }
    #endregion

    public static float Tempo;
    private int blow;
    private float time;


    private void Start()
    {
        
    }
    private void Update()
    {
        Metronome();
    }

    // �t���[������60�ɂ�����
    //void FixedUpdate()
    //{
    //    Tempo = Tempo + 1;

    //    // 2�b(120)�����тɏ�����
    //    if (Tempo >= 120)
    //    {
    //        Tempo = 0;
    //    }
    //}

    private void Metronome()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            blow++;
            Debug.Log(blow);
            time = 0;
        }
        if (blow >=2)
        {
            StartCoroutine(ToRemoveable());
        }
        
    }
    private IEnumerator ToRemoveable()
    {
        enemyRemovable = true;
        blow = 0;
        //Debug.Log("kai");
        yield return new WaitForSeconds(0.5f) ;
        //Debug.Log("guan");
        enemyRemovable = false;
    }
}
