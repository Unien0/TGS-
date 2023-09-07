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
        BulletPlus,
        END
    }
    [SerializeField]private ItemName itemType;
    private Collider2D col2D;
    private SpriteRenderer SpR2D;
    private AudioSource SoundSource;
    [SerializeField] private AudioClip GetSound;
    private bool Fade_Out;
    private Animator anim;
    private float animSpeed;
    private GameObject PlayerObj;

    void Start()
    {
        col2D = GetComponent<Collider2D>();
        SpR2D = GetComponent<SpriteRenderer>();
        SoundSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "！Player")
        {
            switch (itemType)
            {
                case ItemName.LIFEUP:
                    if (hp != maxHp)
                    {
                        hp += 2;
                    }
                    if (hp >= maxHp)
                    {
                        hp = maxHp;
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

                    break;
                case ItemName.BulletPlus:

                    break;
            }
            SoundSource.clip = GetSound;
            SoundSource.Play();
            Fade_Out = true;
            PlayerObj = col.gameObject;
        }
    }

    private void Update()
    {
        if (Fade_Out)
        {
            this.transform.position = new Vector3 (PlayerObj.transform.position.x, PlayerObj.transform.position.y + 1, PlayerObj.transform.position.z);
            anim.SetBool("FadeOut",true);
        }
        else
        {
            animSpeed = GameManeger.AnimSpeed;
            anim.SetFloat("AnimSpeed", animSpeed);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("void"))
        {
            Destroy(gameObject);
        }
    }
}
