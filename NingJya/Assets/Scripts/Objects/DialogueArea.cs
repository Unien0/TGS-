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
    public GameObject door;
    public GameObject npc1;
    //public GameObject npc2;
    private Collider2D col2D;
    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
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
            Destroy(door);
            npc1.SetActive(false);
        }
        if (canChat && !endChat)
        {
            
            //�Θb����������
            if (flowchart.HasBlock(chatName))
            {
                flowchart.ExecuteBlock(chatName);
                endChat = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
        if (col.CompareTag("Player"))
        {
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
