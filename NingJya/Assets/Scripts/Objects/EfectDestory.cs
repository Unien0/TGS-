using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EfectDestory : MonoBehaviour
{
    [SerializeField] private float DeadTime;
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
    [SerializeField] private TextMeshPro ComboText;
    private Rigidbody2D rd2d;
    private float ShotPoint;
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        ShotPoint = Random.Range(-1.5f,1.5f);

        switch (EfectName)
        {
            case EfectType.Move:
                switch (FindObjectOfType<Player>().roteMax)
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
        time += Time.deltaTime;
        if (time >= DeadTime)
        {
            Destroy(gameObject);
        }

        if (EfectName == EfectType.Combo)
        {
            ComboText.text = ComboPoint.ToString();

            if (GameManeger.ComboCheck == true)
            {
                //Destroy(gameObject);
            }
        }
    }
}
