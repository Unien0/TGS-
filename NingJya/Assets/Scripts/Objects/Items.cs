using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public PlayerData_SO playerData;
    #region Player変数の取得
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
    public int maxBullet
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.maxBullet; else return 1; }
    }
    public int nowBullet
    {
        //PlayerのCDを取得する
        get { if (playerData != null) return playerData.nowBullet; else return 1; }
        set { playerData.nowBullet = value; }
    }
    #endregion

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

    private bool Fixed;

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
                    if (!Fixed)
                    {
                        Fixed = true;
                        if (hp != maxHp)
                        {
                            hp += 2;
                        }
                        if (hp >= maxHp)
                        {
                            hp = maxHp;
                        }
                    }          
                    break;
                case ItemName.SCOREUP:
                    if (!Fixed)
                    {
                        Fixed = true;
                    }
                    break;
                case ItemName.BulletPlus:
                    if (!Fixed)
                    {
                        Fixed = true;
                        if (nowBullet != maxBullet)
                        {
                            nowBullet = nowBullet + 1;
                        }
                        if (nowBullet >= maxBullet)
                        {
                            nowBullet = maxBullet;
                        }
                        Debug.Log(nowBullet);
                    }
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
