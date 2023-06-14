using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public static float Tempo;

    // ƒtƒŒ[ƒ€”‚ð60‚É‚µ‚½‚¢
    void FixedUpdate()
    {
        Tempo = Tempo + 1;

        // 2•b(120)—§‚Â‚½‚Ñ‚É‰Šú‰»
        if (Tempo >= 120)
        {
            Tempo = 0;
        }
    }
}
