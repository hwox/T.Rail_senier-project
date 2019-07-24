using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MaterialForCreate : MonoBehaviour
{


    // 소파나 박스 눌렀을 때 뜨는 재료창
    List<int> ForMakeItem = new List<int>();  // 옮겨놓은 아이템
    int[] ItemCount; // 똑같은 아이템 몇개인지
    // 나중에 손으로 가져갈 거 대비한 crack
    public bool ItemCrack;
    AllItem_Ctrl allitem;

    GameObject[] Storage;



    // Use this for initialization
    void Start()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
        Storage = new GameObject[6];
        ItemCount = new int[(int)GameValue.itemCategory.woodboard];

        for (int i = 0; i < 6; i++)
        {
            Storage[i] = gameObject.transform.GetChild(i).GetChild(0).gameObject;
        }
    }



    ////////////////////////////////////////////////////////////////////

    public void DragMouse_Up()
    {

    }

    public void DragMouse_Down(int _number)
    {

    }

    public void DragMouse()
    {

    }

    public void DragMouse_End()
    {

    }

    ////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DragItem"))
        {

            //ForMakeItem.Add(other.GetComponent<PlayerHand_Item>().NowHave);
            if (allitem.UseInLeftHand == 1)
            {
                ForMakeItem.Add(allitem.LeftHand_Pocket);
                allitem.UseLeftHandItem();
            }
            if (allitem.UseInRightHand == 1)
            {
                ForMakeItem.Add(allitem.RightHand_Pocket);
                allitem.UseRightHandItem();
            }

            DrawMaterialStorageImage();
        }
    }

    ////////////////////////////////////////////////////////////////////

    void DrawMaterialStorageImage()
    {
        int StorageIndex = 0;

        Storage[StorageIndex].GetComponent<Image>().sprite = allitem.ItemImage[ForMakeItem[0] - 1];
        ItemCount[ForMakeItem[0] - 1] += 1;

        StorageIndex += 1;

        for (int i = 1; i < ForMakeItem.Count; i++)
        {
            // 정렬을 사용하려고 했으나 그러면 맨 먼저 들어왔던 이미지가 걔보다 enum값이 클 경우
            // 부자연스럽게 뒤로 밀릴 수 있어서 그렇게 안하기로함
            if (ItemCount[ForMakeItem[i] - 1].Equals(0))
            {

                //0 이면 이전에 등록이 안됐다는 소리
                Storage[StorageIndex].GetComponent<Image>().sprite = allitem.ItemImage[ForMakeItem[0] - 1];
                StorageIndex += 1;
            }

            ItemCount[ForMakeItem[0] - 1] += 1;
        }

        for (int i = StorageIndex; i < 6; i++)
        {
            Storage[StorageIndex].GetComponent<Image>().sprite = allitem.NullImage;
        }
    }
}
