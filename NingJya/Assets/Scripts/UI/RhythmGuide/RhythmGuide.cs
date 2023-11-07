using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGuide : MonoBehaviour
{
    private float animSpeed;                // アニメーション速度の値を保存する変数
    private Animator anim;                  // Animatorコンポーネントを保存する変数

    void Start()
    {
        // Animatorコンポーネントを保存する
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animSpeed = GameManeger.AnimSpeed;
        anim.SetFloat("AnimSpeed", animSpeed);
    }
}
