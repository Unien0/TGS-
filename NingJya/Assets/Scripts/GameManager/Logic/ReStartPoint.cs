using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // プレイヤーがポイントに侵入したら
        if(col.gameObject.name == "！Player")
        {
            // 親機(ReStartManeger)にこのオブジェクトの情報を送信する
            ReStartManeger.ReSpornPoint = transform.position;
            ReStartManeger.CanReSporn = true;
        }
    }
}
