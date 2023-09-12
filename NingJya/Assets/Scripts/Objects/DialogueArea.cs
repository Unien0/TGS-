using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueArea : MonoBehaviour
{
    public string chatName;
    private bool outEnemy = false;
    private bool canChat = false;
    private bool endChat = false;

    private Flowchart flowchart;
    public GameObject iconDisplay;
    //public GameObject npc1;

    //public GameObject npc2;
    private Collider2D col2D;
    private bool make;
    private bool isSay;
    [SerializeField] private GameObject DEAD_EFECT;
    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //iconDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (outEnemy)
        //{
        //    if (isSay)
        //    {
        //        if (!make)
        //        {
        //            make = true;
        //            Instantiate(DEAD_EFECT, npc1.transform.position, npc1.transform.rotation);

        //        }
        //        else
        //        {
        //            npc1.SetActive(false);
        //            iconDisplay.SetActive(false);
        //        }
        //    }
        //}
        //if (canChat && Input.GetKeyDown("joystick button 0"))
        //{

        //    //対話実装したか
        //    if (flowchart.HasBlock(chatName))
        //    {
        //        flowchart.ExecuteBlock(chatName);
        //        isSay = true;
        //        //endChat = true;
        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // プレイヤーの攻撃範囲に入っていて、
        if (col.gameObject.name == "！Player" && !canChat)
        {
            //iconDisplay.SetActive(true);
            //npc1.SetActive(true);
            canChat = true;
            flowchart.ExecuteBlock(chatName);
        }
        
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //iconDisplay.SetActive(false);
            //outEnemy = true;
            //canChat = true;

        }
    }
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        outEnemy = true;
    //    }
    //}
}
