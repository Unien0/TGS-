using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public PlayerData_SO playerData;

    private float maxHp
    {
        //Player‚ÌMaxHP‚ðƒQƒbƒg‚·‚é
        get { if (playerData != null) return playerData.maxHp; else return 0; }
    }
    private float hp
    {
        //Player‚ÌHP‚ðƒQƒbƒg‚·‚é
        get { if (playerData != null) return playerData.hp; else return 0; }
    }

    public Image Bar;

    private void Awake()
    {
        
    }


    void Update()
    {
        Bar.fillAmount = hp / maxHp;
    }
}
