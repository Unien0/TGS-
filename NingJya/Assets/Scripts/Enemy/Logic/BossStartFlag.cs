using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartFlag : MonoBehaviour
{
    public bool ActStart;
    public bool ActEnd;
    [SerializeField] private GameObject ENTER_SYOUZI;
    [SerializeField] private GameObject EXIT_SYOUZI;
    [SerializeField] private Collider2D EXIT_SYOUZI_Col2D;
    [SerializeField] private GameObject PlayerMovePos;
    private void Update()
    {
        if (ActEnd)
        {
            EXIT_SYOUZI_Col2D.enabled = false;
            EXIT_SYOUZI.GetComponent<Animator>().SetBool("IsEnd", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "！Player")
        {
            //プレイヤー停止
            FindObjectOfType<Player>().taking = true;
            ActStart = true;
            ENTER_SYOUZI.SetActive(true);
            collision.gameObject.transform.position = PlayerMovePos.transform.position;
        }
    }
}
