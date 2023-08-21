using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb2d;               // Rigidbody2Dの取得・格納
    public GameObject conductObject;
    public bool Ready;
    private bool shot;
    public float BulletSpeed;
    public Vector2 shotrote;
    [SerializeField] private Vector2 shotIt;

    void Start()
    {
        // RigidBody2Dの情報格納・代入
        rb2d = GetComponent<Rigidbody2D>();
        FindObjectOfType<ConductManeger>().CTobject = this.gameObject;
        FindObjectOfType<ConductManeger>().conduct = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shot)
        {
            if (Ready)
            {
                if (conductObject != null)
                {
                    //方向
                    shotrote = new Vector2(conductObject.transform.position.x - this.transform.position.x, conductObject.transform.position.y - this.transform.position.y);
                    shotIt.x = Mathf.Sign(shotrote.x);
                    shotIt.y = Mathf.Sign(shotrote.y);
                    //現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * BulletSpeed);
                    shot = true;
                }
                else
                {
                    //方向
                    shotrote = new Vector2(this.transform.position.x - FindObjectOfType<Player>().transform.position.x, this.transform.position.y - FindObjectOfType<Player>().transform.position.y);
                    if (shotrote.x <= -0.5f || shotrote.x >= 0.5f)
                    { shotIt.x = Mathf.Sign(shotrote.x); }
                    else
                    { shotIt.x = 0; }
                    if (shotrote.y <= -0.5f || shotrote.y >= 0.5f)
                    { shotIt.y = Mathf.Sign(shotrote.y); }
                    else
                    { shotIt.y = 0; }
                    //4、現在位置に基づいて吹っ飛ばすの力と保存時間を判断します
                    rb2d.AddForce(shotIt * BulletSpeed);
                    shot = true;
                }
            }
        }
    }
}
