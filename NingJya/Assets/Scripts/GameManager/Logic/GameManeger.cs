using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public EnemyData_SO enemyData;
    #region EnemyDataの変数
    private bool enemyRemovable
    {
        //移動可能かどうかを判断する
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

    // フレーム数を60にしたい
    //void FixedUpdate()
    //{
    //    Tempo = Tempo + 1;

    //    // 2秒(120)立つたびに初期化
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
        if (blow <4)
        {
            enemyRemovable = false;
            Debug.Log("guan");
        }
        else
        {
            enemyRemovable = true;
            StartCoroutine(ToRemoveable());            
            blow = 0;            
        }
        
    }
    private IEnumerator ToRemoveable()
    {
        
        Debug.Log("kai");
        yield return new WaitForSeconds(0.5f) ;
    }
}
