using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public PlayerData_SO playerData;
    public int maxHp
    {
        //PlayerのMaxHPをゲットする
        get { if (playerData != null) return playerData.maxHp; else return 0; }
        set { playerData.maxHp = value; }
    }
    public int hp
    {
        //PlayerのHPをゲットする
        get { if (playerData != null) return playerData.hp; else return 0; }
        set { playerData.hp = value; }
    }

    private enum ItemName
    {
        LIFEUP,
        SCOREUP,
        END
    }
    [SerializeField]private ItemName itemType;
    private Collider2D col2D;
    private SpriteRenderer SpR2D;
    private AudioSource SoundSource;
    [SerializeField] private AudioClip GetSound;

    void Start()
    {
        col2D = GetComponent<Collider2D>();
        SpR2D = GetComponent<SpriteRenderer>();
        SoundSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "！Player")
        {
            col2D.enabled = false;
            SpR2D.enabled = false;
            switch (itemType)
            {
                case ItemName.LIFEUP:
                    if (hp <= maxHp)
                    {
                        hp += 2;
                    }
                    /*
                    else if(hp == maxHp && maxHp <12)
                    {
                        maxHp += 2;
                        hp += 2;
                    }
                    else if(hp ==maxHp && maxHp == 12)
                    {
                        hp = maxHp;
                    }*/
                    break;
                case ItemName.SCOREUP:
                    GameManeger.GetCoin++;
                    break;
            }
            SoundSource.clip = GetSound;
            SoundSource.Play();
        }
    }
}
