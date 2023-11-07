using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EfectDestory : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rd2d;

    [SerializeField] private float DeadTime;
    private float animSpeed;
    private float time = 0;

    public enum EfectType
    {
        Move,
        Combo,
        Any,
        End
    }
    [SerializeField] private EfectType EfectName;
    public int ComboPoint;
    [SerializeField] private GameObject No1Number;
    [SerializeField] private GameObject No2Number;
    [SerializeField] private Sprite[] Numbers;
    private float ShotPoint;
    [SerializeField] private bool NeedanimSpeed;
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();

        ShotPoint = Random.Range(-1.5f,1.5f);
        switch (EfectName)
        {
            case EfectType.Move:
                switch (FindObjectOfType<Player>().moveDirection)
                {
                    #region
                    case 0:
                        this.transform.rotation = Quaternion.Euler(90, 90, -90);
                        break;
                    case 45:
                        this.transform.rotation = Quaternion.Euler(45, 90, -90);
                        break;
                    case 90:
                        this.transform.rotation = Quaternion.Euler(0, 90, -90);
                        break;
                    case 135:
                        this.transform.rotation = Quaternion.Euler(-45, 90, -90);
                        break;
                    case 180:
                        this.transform.rotation = Quaternion.Euler(-90, 90, -90);
                        break;
                    case 225:
                        this.transform.rotation = Quaternion.Euler(-135, 90, -90);
                        break;
                    case 270:
                        this.transform.rotation = Quaternion.Euler(180, 90, -90);
                        break;
                    case 315:
                        this.transform.rotation = Quaternion.Euler(135, 90, -90);
                        break;
                        #endregion
                }
                break;
            case EfectType.Combo:
                rd2d.AddForce(new Vector2(ShotPoint, 5) * 80);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NeedanimSpeed)
        {
            animSpeed = GameManeger.AnimSpeed;
            anim.SetFloat("AnimSpeed", animSpeed);
        }
        time += Time.deltaTime;
        if (EfectName == EfectType.Combo)
        {
            if (GameManeger.TempoExChange)
            {
                anim.SetBool("FadeOut", true);
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("void"))
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (time >= DeadTime)
        {
            Destroy(gameObject);
        }

        switch (EfectName)
        {
            case EfectType.Combo:
                // コンボ数確認
                int data1 = (int)(ComboPoint % 10);              // 1の位
                int data2 = (int)((ComboPoint / 10) % 10);       // 2の位

                // スプライト変更
                for (int i = 0; i < 2; i++)
                {
                    switch (i)
                    {
                        case 0:     // 一桁目
                            No1Number.GetComponent<SpriteRenderer>().sprite = GetSprite(data1);
                            break;
                        case 1:     // 二桁目
                            if (data2 == 0)
                            {
                                No2Number.SetActive(false);
                            }
                            else
                            {
                                No2Number.SetActive(true);
                                No2Number.GetComponent<SpriteRenderer>().sprite = GetSprite(data2);
                            }
                            break;
                    }
                }

                if (rd2d.velocity.y < 0)
                {
                    rd2d.velocity = Vector2.zero;
                }
                break;
        }    
    }

    Sprite GetSprite(int number)
    {
        Sprite img = Numbers[0];
        switch (number)
        {
            case 0:
                img = Numbers[0];
                break;
            case 1:
                img = Numbers[1];
                break;
            case 2:
                img = Numbers[2];
                break;
            case 3:
                img = Numbers[3];
                break;
            case 4:
                img = Numbers[4];
                break;
            case 5:
                img = Numbers[5];
                break;
            case 6:
                img = Numbers[6];
                break;
            case 7:
                img = Numbers[7];
                break;
            case 8:
                img = Numbers[8];
                break;
            case 9:
                img = Numbers[9];
                break;
        }
        return img;
    }
}
