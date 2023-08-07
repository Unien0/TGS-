using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectDestory : MonoBehaviour
{
    [SerializeField] private float DeadTime;
    private float time = 0;

    public enum EfectType
    {
        Move,
        Any,
        End
    }
    [SerializeField] private EfectType EfectName;
    void Start()
    {
        switch (EfectName)
        {
            case EfectType.Move:
                switch (FindObjectOfType<Player>().roteMax)
                {
                    #region
                    case 0:
                        this.transform.rotation = Quaternion.Euler(90, 90, -90);
                        break;
                    case 45:
                        this.transform.rotation = Quaternion.Euler(45, 90, -90);
                        break;
                    case 90:
                        this.transform.rotation = Quaternion.Euler(0, 90, -90);
                        break;
                    case 135:
                        this.transform.rotation = Quaternion.Euler(-45, 90, -90);
                        break;
                    case 180:
                        this.transform.rotation = Quaternion.Euler(-90, 90, -90);
                        break;
                    case 225:
                        this.transform.rotation = Quaternion.Euler(-135, 90, -90);
                        break;
                    case 270:
                        this.transform.rotation = Quaternion.Euler(180, 90, -90);
                        break;
                    case 315:
                        this.transform.rotation = Quaternion.Euler(135, 90, -90);
                        break;
                        #endregion
                }

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= DeadTime)
        {
            Destroy(gameObject);
        }
    }
}
