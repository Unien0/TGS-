using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public static float Tempo;

    // �t���[������60�ɂ�����
    void FixedUpdate()
    {
        Tempo = Tempo + 1;

        // 2�b(120)�����тɏ�����
        if (Tempo >= 120)
        {
            Tempo = 0;
        }
    }
}
