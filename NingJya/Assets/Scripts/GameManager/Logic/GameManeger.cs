using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public static float Tempo;

    // フレーム数を60にしたい
    void FixedUpdate()
    {
        Tempo = Tempo + 1;

        // 2秒(120)立つたびに初期化
        if (Tempo >= 120)
        {
            Tempo = 0;
        }
    }
}
