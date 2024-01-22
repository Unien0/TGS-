using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessObject : MonoBehaviour
{
    private Collider2D col2D;
    [SerializeField] private GameObject[] AccessEfect;
    [SerializeField] private Objects cs;
    private void Start()
    {
        col2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            col2D.enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            col2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            col2D.enabled = false;
        }

        if (col.gameObject.name == "ÅIPlayer")
        {
            foreach (var Act in AccessEfect)
            {
                Act.GetComponent<Animator>().SetBool("Access", true);
            }
            cs.Activate = true;
            cs.ActivateObj = this.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            col2D.enabled = true;
        }
    }
}
