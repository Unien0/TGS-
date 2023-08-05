using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public PlayerData_SO playerData;

    public int MaxHP
    {
        get { if (playerData != null) return playerData.maxHp; else return 0; }
    }
    public int HP
    {
        get { if (playerData != null) return playerData.hp; else return 0; }
    }


    public GameObject heartFull;
    public GameObject heartHalf;
    public GameObject heartVoid;


    // Update is called once per frame
    void Update()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < HP / 2; i++)
        {
            Instantiate(heartFull, transform);
        }
        for (int i = 0; i < HP % 2; i++)
        {
            Instantiate(heartHalf, transform);
        }
        for (int i = 0; i < (MaxHP - HP) / 2; i++)
        {
            Instantiate(heartVoid, transform);
        }

    }
}
