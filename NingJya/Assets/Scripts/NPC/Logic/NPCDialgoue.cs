using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class NPCDialgoue : MonoBehaviour
{
    public string chatName;
    private bool npc;
    private bool endChat = false;
    private bool outEnemy = false;
    public GameObject door1;
    public GameObject npc1;
    public Flowchart fc;
    private Collider2D col2D;
    private bool make;
    [SerializeField] private GameObject DEAD_EFECT;

    private void Awake()
    {
        fc = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        col2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        //Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        if (fc.GetBooleanVariable("WellOpen"))
        {
            if (!make)
            {
                make = true;
                Instantiate(DEAD_EFECT, npc1.transform.position, npc1.transform.rotation);
            }
            Destroy(door1);
            npc1.SetActive(false);
        }
        if (fc.GetBooleanVariable("TutorialCompletion") && !endChat &&outEnemy)
        {

            //‘Î˜bŽÀ‘•‚µ‚½‚©
            //if (fc.HasBlock(chatName))
            //{
            //    fc.ExecuteBlock(chatName);
            //    endChat = true;
            //}
        }
        if (fc.GetBooleanVariable("ToInGame"))
        {
            //SceneManager.LoadScene("SampleMap");
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
