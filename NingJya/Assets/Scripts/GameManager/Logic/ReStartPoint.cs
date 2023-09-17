using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartPoint : MonoBehaviour
{
    // 何番目のセーブポイントであるかを確認する(1から数え始める)
    [SerializeField] private float SaveOrderNumber;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // プレイヤーがポイントに侵入したら
        if(col.gameObject.name == "！Player")
        {
            // 親機(ReStartManeger)にこのオブジェクトの情報を送信する
            ReStartManeger.ReSpornPoint = transform.position;
            ReStartManeger.SaveOrder = SaveOrderNumber;
        }
    }
}
