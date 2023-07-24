using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueArea : MonoBehaviour
{
    public string chatName;
    private bool outEnemy = false;
    private bool canChat = false;

    public GameObject door1;
    public GameObject door2;
    public GameObject npc1;
    //public GameObject npc2;
    private Collider2D col2D;
    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (outEnemy)
        {
            door1.SetActive(false);
            npc1.SetActive(false);
        }
        if (canChat)
        {
            //対話
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            //対話実装したか
            if (flowchart.HasBlock(chatName))
            {
                flowchart.ExecuteBlock(chatName);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.CompareTag("Player"))
        {
            door1.SetActive(true);
            npc1.SetActive(true);
            canChat = true;
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            outEnemy = true;
        }
    }
}
