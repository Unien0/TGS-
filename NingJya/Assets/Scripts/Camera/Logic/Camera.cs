using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // やること
    // プレイヤーとの距離の差に応じて、カメラ移動する
    // 画面端に近づいたら、プレイヤーを中央に据えるように移動する。
    // まずはプレイヤーの取得変数と差を求める変数を元にif文を造れ

    [SerializeField]private GameObject playerObj;
    private Vector2 PosGap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerObj = FindObjectOfType<Player>().gameObject;
        PosGap = new Vector2(playerObj.transform.position.x - this.gameObject.transform.position.x,
                            playerObj.transform.position.y - this.gameObject.transform.position.y);

        if (PosGap.x >= 2.5f)
        {
            Debug.Log("Right");
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * 5),transform.position.y,transform.position.z);
        }
        if (PosGap.x <= -2.5f)
        {
            Debug.Log("Left");
            transform.position = new Vector3(transform.position.x - (Time.deltaTime * 5), transform.position.y, transform.position.z);
        }
        if (PosGap.y >= 0.75f)
        {
            Debug.Log("Up");
            transform.position = new Vector3(transform.position.x , transform.position.y + (Time.deltaTime * 5), transform.position.z);
        }
        if (PosGap.y <= -0.75f)
        {
            Debug.Log("Down");
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * 5), transform.position.z);
        }

    }
}
