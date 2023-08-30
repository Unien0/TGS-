using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUI : MonoBehaviour
{
    public PlayerData_SO playerData;
    private int nowBullet
    {
        //Player‚ÌCD‚ðŽæ“¾‚·‚é
        get { if (playerData != null) return playerData.nowBullet; else return 1; }
        set { playerData.nowBullet = value; }
    }

    [SerializeField] private GameObject[] BulletIcon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var Iconobj in BulletIcon)
        {
            Iconobj.GetComponent<Animator>().SetFloat("AnimSpeed", GameManeger.AnimSpeed);
        }

        switch (nowBullet)
        {
            case 0:
                BulletIcon[0].gameObject.SetActive(false);
                break;
            case 1:
                BulletIcon[0].gameObject.SetActive(true);
                BulletIcon[1].gameObject.SetActive(false);
                break;
            case 2:
                BulletIcon[1].gameObject.SetActive(true);
                BulletIcon[2].gameObject.SetActive(false);
                break;
            case 3:
                BulletIcon[2].gameObject.SetActive(true);
                break;
        }
    }
}
