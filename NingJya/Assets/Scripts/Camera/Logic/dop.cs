using Cinemachine;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dop : MonoBehaviour
{
    // プレイヤーが侵入したかどうか確認する変数
    private bool Check;
    // 侵入時処理が終わったかどうか確認する変数
    private bool Fixed; 
    
    // 敵キャラを出すイベントを行うかどうか確認する変数 
    [SerializeField]
    private bool JumpScare;
    // 出現させる敵のオブジェクトを保存する変数
    [SerializeField] 
    private GameObject[] ProceedBlocks;
    // 出現時エフェクトを保存する変数
    [SerializeField] 
    private GameObject Smoke;
    // 倒した敵の数を記録する変数
    private int BreakPoint;

    // カメラの固定位置を保存する変数
    [SerializeField] GameObject StopPoint;
    void Start()
    {

    }

    void Update()
    {
        if (Check)
        {
            if (!Fixed)
            {
                // 進行度を更新する
                Camera.Order += 1;

                // 敵のジャンプスケアがONになっている場合
                if (JumpScare)
                {
                    foreach (var Eventobj in ProceedBlocks)
                    {
                        Eventobj.SetActive(true);
                        Instantiate(Smoke, Eventobj.transform.position, Eventobj.transform.rotation);
                    }
                }
                // 複数回処理を行わないようにこのif文を非成立にさせる
                Fixed = true;
            }
            else
            {
                if (JumpScare)
                {
                    // カメラの移動を止める
                    FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().enabled = false;
                    // カメラの位置を変更する
                    FindObjectOfType<Camera>().GetComponent<Transform>().transform.position = StopPoint.transform.position;
                    // 敵の撃破数を確認する
                    foreach (var Eventobj in ProceedBlocks)
                    {
                        // 倒されていた場合
                        if (Eventobj.gameObject.GetComponent<SpriteRenderer>().enabled == false)
                        {
                            // 数を1増やす
                            BreakPoint += 1;
                        }
                        else
                        {
                            // 倒されていない場合このループを脱する
                            break;
                        }
                    }

                    // 倒した数が敵の総数と同じ場合
                    if (BreakPoint == ProceedBlocks.Length)
                    {
                        // このイベントを終了する
                        JumpScare = false;
                        // カメラの移動を再開させる
                        FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().enabled = true;
                    }
                    else
                    {
                        // 同じでない場合ループ処理をやり直す
                        // そのため、点数を0にする
                        BreakPoint = 0;
                    }
                }
                else
                {
                    // 削除する
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "！Player")
        {
            Check = true;
        }
    }
}
