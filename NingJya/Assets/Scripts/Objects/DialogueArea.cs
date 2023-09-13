using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueArea : MonoBehaviour
{
    public string chatName;
    private bool chated = false;
    private bool canChat = false;
    private bool chatCD;
    private float time;
    private Flowchart flowchart;
    public GameObject iconDisplay;
    private Collider2D col2D;
    [SerializeField] private GameObject DEAD_EFECT;

    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    void Update()
    {
        //if (!flowchart.GetBooleanVariable("ChatCD"))
        //{
        //    chatCD = true;
        //    time += Time.deltaTime;
        //    if (time >=1)
        //    {
        //        chatCD = false;
        //    }
        //}

        if (chated && Input.GetKeyDown("joystick button 0") && canChat&& !flowchart.GetBooleanVariable("ChatCD"))
        {
            //対話実装したか
            if (flowchart.HasBlock(chatName))
            {
                flowchart.ExecuteBlock(chatName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.gameObject.name == "！Player" && !chated)
        {
            chated = true;
            
            flowchart.ExecuteBlock(chatName);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "！Player")
        {
            canChat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "！Player")
        {
            canChat = false;
        }
    }

}
