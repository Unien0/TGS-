using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class NPCDialgoue : MonoBehaviour
{
    public PlayerData_SO playerData;
    public bool taking
    {
        //ÉvÉåÉCÉÑÅ[Ç™à⁄ìÆÇ≈Ç´ÇÈÇ©Ç«Ç§Ç©ÇîªífÇ∑ÇÈ
        get { if (playerData != null) return playerData.taking; else return false; }
        set { playerData.taking = value; }
    }

    public string chatName;
    public GameObject npc1;
    public Flowchart fc;
    private bool make;
    [SerializeField] private GameObject DEAD_EFECT;
    [SerializeField] private GameObject rotate_Close;

    public GameObject[] anim;
    private float animSpeed;

    [SerializeField] private AudioSource AS;

    private void Awake()
    {
        fc = GameObject.Find("Flowchart").GetComponent<Flowchart>();
    }

    void Update()
    {
        foreach (var Iconobj in anim)
        {
            Iconobj.GetComponent<Animator>().SetFloat("AnimSpeed", GameManeger.AnimSpeed);
        }

        if (fc.GetBooleanVariable("WellOpen"))
        {
            if (!make)
            {
                make = true;
                Instantiate(DEAD_EFECT, npc1.transform.position, npc1.transform.rotation);
            }
            else
            {
                npc1.SetActive(false);
            }

        }
        if (fc.GetBooleanVariable("ToInGame"))
        {
            AS.GetComponent<AudioSource>().volume -= Time.deltaTime;
            if (rotate_Close.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Close"))
            {
                StoryScenes.NextStageNum = 1;
                SceneManager.LoadScene("StoryScene");
            }
        }
        if (fc.GetBooleanVariable("PlayerTaking"))
        {
            taking = true;
        }
        else
        {
            taking = false;
        }
    }
}
