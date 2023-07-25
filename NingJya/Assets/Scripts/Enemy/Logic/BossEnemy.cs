using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private enum BossNumber
    {
        No1,
        No2,
        end
    }
    [SerializeField] private BossNumber BossType;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (BossType)
        {
            case BossNumber.No1:
                FirstBoss();
                break;
        }
    }

    void FirstBoss()
    {
    }
}
