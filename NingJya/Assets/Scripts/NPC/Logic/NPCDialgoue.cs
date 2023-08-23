using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class NPCDialgoue : MonoBehaviour
{
    public string chatName;
    public GameObject npc1;
    public Flowchart fc;
    private bool make;
    [SerializeField] private GameObject DEAD_EFECT;

    private void Awake()
    {
        fc = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    void Update()
    {
        if (fc.GetBooleanVariable("WellOpen"))
        {
            if (!make)
            {
                make = true;
                Instantiate(DEAD_EFECT, npc1.transform.position, npc1.transform.rotation);
            }
            npc1.SetActive(false);
        }
        if (fc.GetBooleanVariable("ToInGame"))
        {
            SceneManager.LoadScene("RemakeScene");
        }
    }
}
