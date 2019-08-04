using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForUICollision : MonoBehaviour
{
    public int ThisBoxIndex;
    public InBoxItem InBoxItemScript;
    AllItem_Ctrl allitem;

    private void Start()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
    }
    private void OnTriggerEnter(Collider other)
    {
    
        if (other.CompareTag("DragItem"))
        {
            if (allitem.ItemHTBEnable)
            {
                ThisBoxIndex = InBoxItemScript.thisBoxIndex;

                if (!InBoxItemScript.BoxFullCheck())
                {
                    allitem.ItemHandToBox = true;
                    allitem.ForItemToBoxIndex(ThisBoxIndex);
                }
                else
                {
                    allitem.ItemHandToBox = false;
                }

            }
        }
    }
}
