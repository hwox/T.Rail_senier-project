using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InBoxItem : MonoBehaviour
{


    // 이거 누르면 얘가 가지고 있는거 inventory로 켜기 
    public enum itemCategory
    {
        nail = 1, // 못
        ironpan = 2, // 판(철판)
        food_tomato = 3, // 음식
        food_bean = 4,
        food_chicken = 5,
        hammer = 6, // 도끼
                    //  spanner = 7, // 스패너
    }

    public Canvas MyInvenCanvas;

    [SerializeField]
    AllItem_Ctrl allitem;

    public List<int> HaveItemInfo = new List<int>();
    int thisBoxIndex;

    bool BoxFull; // 박스가 꽉찼는지 아닌지 

    void Start()
    {

        allitem = TrainGameManager.instance.allitemCtrl;

        ActiveThisBox();


        // 이 밑 for문은 테스트 하려고 랜덤으로 아이템 집어넣은 거임 
        // additem 수정되면 지워질 부분
        for (int i = 0; i < 6; i++)
        {
            int temp = Random.Range(1, 7);
            AddItem(temp);
        }
    }

    // 여기에 있는거를 add하고 delete까지 여기서

    public void ActiveThisBox()
    {
        // active라는 말이 이 상자가 사용될 때
        // 아직 이걸 오브젝트 풀링으로 할지 어떻게 할지 정하질 않아서
        // 함수 이름 그냥 activethisbox라고 해놨음 (active될 때 쓸거니까.)
        allitem.AddedItemBox(this);
        thisBoxIndex = allitem.boxItem.Count - 1;
    }
    public void InActiveThisBox()
    {
        allitem.DeletedItemBox(thisBoxIndex);
        thisBoxIndex = -99; // 일단 어떻게 사용할지 몰라서 쓰레기값 넣어주기 
    }

    public void OpenBoxInven()
    {

        // 이거 on되면 손도 같이  on되게
        MyInvenCanvas.gameObject.SetActive(true);
        //  allitem.HandItemUiOnOff(true);
        allitem.ItemInhand.SetActive(true);

        // 
        //// ShowInInventory();
    }
    public void OffBoxInven()
    {
        //  allitem.ItemInhand.SetActive(false);위에 동일한 거 true는 되는데 false는 안됨. 왜?
      
        MyInvenCanvas.gameObject.SetActive(false);
        allitem.ItemInhand.SetActive(false);
    }


    public void DestroyTrain()
    {
        //allitem.HaveItemInfo.RemoveAt();
        allitem.DeletedItemBox(thisBoxIndex);
        thisBoxIndex = -99; // 일단 어떻게 사용할지 몰라서 쓰레기값 넣어주기 
    }

    public void AddItem(int _item)
    {
        if (!BoxFull)
        {
            HaveItemInfo.Add(_item);
            ShowInInventory();
            if (HaveItemInfo.Count == 6)
            {
                BoxFull = true;
            }
        }
        else
        {
            Debug.Log("6초과 ");
        }
    }

    public void DeleteItem(int _item)
    {
        if (HaveItemInfo.Count > 0)
        {
            // 삭제

            BoxFull = false;

        }
    }

    void ShowInInventory()
    {
        // 인벤토리에 보이는거 이미지 등록
        for (int i = 0; i < HaveItemInfo.Count; i++)
        {
            switch (HaveItemInfo[i])
            {
                case (int)itemCategory.nail:
                    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[0];
                    break;
                case (int)itemCategory.ironpan:
                    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[1];
                    break;
                case (int)itemCategory.food_tomato:
                    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[2];
                    break;
                case (int)itemCategory.food_bean:
                    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[3];
                    break;
                case (int)itemCategory.food_chicken:
                    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[4];
                    break;
                case (int)itemCategory.hammer:
                    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[5];
                    break;
                    //  case (int)itemCategory.spanner:
                    //    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[7];
                    //     break;
            }
        }
    }

}
