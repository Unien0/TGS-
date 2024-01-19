using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessObject : MonoBehaviour
{
    private Collider2D col2D;
    [SerializeField] private GameObject[] AccessEfect;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "ÅIPlayer")
        {
            foreach (var Act in AccessEfect)
            {
                Act.GetComponent<Animator>().SetBool("Access", true);
            }
            Objects.Activate = true;
            Objects.ActivateObj = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
