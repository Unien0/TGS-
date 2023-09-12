using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueArea : MonoBehaviour
{
    public string chatName;
    private bool chated = false;
    private bool canChat = false;

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
        if (chated && Input.GetKeyDown("joystick button 0") && canChat)
        {

            //�Θb����������
            if (flowchart.HasBlock(chatName))
            {
                flowchart.ExecuteBlock(chatName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // �v���C���[�̍U���͈͂ɓ����Ă��āA
        if (col.gameObject.name == "�IPlayer" && !chated)
        {
            chated = true;
            
            flowchart.ExecuteBlock(chatName);
        }
        else if (col.gameObject.name == "�IPlayer")
        {
            canChat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "�IPlayer")
        {
            canChat = false;
        }
    }

}
