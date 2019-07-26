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
    int[] WhatInStorage; // 뭐가 자꾸 늘어나네 얘는 그 인덱스에 있는게 몇번째 아이템인ㄴ지
    Text[] StorageCount;

    // Dictionary<int, int> Storage_test = new Dictionary<int, int>();
    // 앞에는 index 뒤에는 count

    int StorageIndex = 0;

    // Use this for initialization
    void Start()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
        Storage = new GameObject[6];
        StorageCount = new Text[6];
        WhatInStorage = new int[6];
        ItemCount = new int[(int)GameValue.itemCategory.woodboard];

        for (int i = 0; i < 6; i++)
        {
            Storage[i] = gameObject.transform.GetChild(i).GetChild(0).gameObject;
            StorageCount[i] = gameObject.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
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

            if (StorageIndex <= 6)
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

            else
            {
                Debug.Log("보관함 6개 넘어서 못넣어 이제");
            }
        }
    }

    ////////////////////////////////////////////////////////////////////

    void DrawMaterialStorageImage()
    {
        //int StorageIndex = 0;
        bool Enable = false;
        int index = 0;

        for (int i = 0; i < 6; i++)
        {

            if (WhatInStorage[i].Equals(ForMakeItem[ForMakeItem.Count - 1]))
            {

                // count-1값이 제일 마지막에 들어온 값일거니끼ㅏ
                Enable = true;
                index = i;
                break;
            }

        }
        ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1] += 1;

        if (Enable)
        {
            // 이거는 겹치는게 있다는거 
            // 그러니까 count 값 하나 증가시키고 
            // text
            StorageCount[index].text = ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1].ToString();
        }

        else if (!Enable)
        {
            // 겹치는거없어
            // 이미지 띄우기
            WhatInStorage[StorageIndex] = ForMakeItem[ForMakeItem.Count - 1];
            Storage[StorageIndex].GetComponent<Image>().sprite = allitem.ItemImage[ForMakeItem[ForMakeItem.Count - 1] - 1];
            StorageCount[index].text = ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1].ToString();
            StorageIndex += 1;
        }


        for (int j = StorageIndex; j < 6; j++)
        {
            Storage[StorageIndex].GetComponent<Image>().sprite = allitem.NullImage;

        }
        Debug.Log("증가한거"+ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1]);
        Debug.Log("마짐막꺼"+ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1]);
    }

    public bool IsBoxMakeEnable()
    {
        // 나무 두개 8 못 하나 1  망치 하나 6
        bool wood1 = false;
        bool wood2 = false;
        bool nail = false;
        bool hammer = false;

        for (int i = 0; i < ForMakeItem.Count; i++)
        {
            if (ForMakeItem[i] == 8)
            {
                if (!wood1)
                {

                    wood1 = true;
                }
                else if (!wood2)
                {

                    wood2 = true;

                }
            }

            if (ForMakeItem[i] == 1)
            {
                if (!nail)
                {

                    nail = true;
                }
            }
            if (ForMakeItem[i] == 6)
            {
                if (!hammer)
                {

                    hammer = true;
                }
            }


            if(hammer && wood1 && wood2 && nail)
            {
                return true;
            }
        }

        return false;
    }
            // 다 찾았으면 그것도 다 지워야 되네
            // 뺄거잖아
            // 어차피 뺄 필요없나?
    public bool IsSofaMakeEnable()
    {
        bool wood = false;
        bool nail1 = false;
        bool nail2 = false;
        bool hammer = false;
        // 나무 하나 못 두개 망치 하나
        for (int i = 0; i < ForMakeItem.Count; i++)
        {
            if (ForMakeItem[i] == 8)
            {
                if (!wood)
                {

                    wood = true;
                }
            }

            if (ForMakeItem[i] == 1)
            {
                if (!nail1)
                {

                    nail1 = true;
                }
                else if (!nail2)
                {

                    nail2 = true;
                }
            }
            if (ForMakeItem[i] == 6)
            {
                if (!hammer)
                {

                    hammer = true;
                }
            }


            if (hammer && wood && nail1 && nail2)
            {
                return true;
            }
        }
        return false;
    }

}
