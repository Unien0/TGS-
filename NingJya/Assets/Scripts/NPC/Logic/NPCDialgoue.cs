using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NPCDialgoue : MonoBehaviour
{
    public GameObject door1;
    public GameObject npc1;
    private bool npc;
    private FungusManager fungus;
    public Flowchart fc;

    private void Awake()
    {
        
    }

    void Update()
    {
        //Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        if (fc.GetBooleanVariable("WellOpen"))
        {
            Destroy(door1);
            npc1.SetActive(false);
        }
    }
    
}
