using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AllItem_Ctrl : MonoBehaviourPunCallbacks
{

    //public enum itemCategory
    //{
    //    nail = 1, // 못
    //    ironpan = 2, // 판(철판)
    //    food_tomato = 3, // 음식
    //    food_bean = 4,
    //    food_chicken = 5,
    //    hammer = 6, // 도끼
    //    medipack = 7,
    //    woodboard = 8,
    //    //spanner = 7, // 스패너
    //}

    public Sprite[] ItemImage;
    public Sprite NullImage;
    public Sprite StandardImage; //기본 아이템창 백이미지

    int boxNumber; // 박스 총 몇개인지

    public List<InBoxItem> boxItem = new List<InBoxItem>();

    // #hand UI
    public GameObject ItemInhand; // 손 item 

    public Image DragCursorSprite; // 마우스 드래그할 때 이미지
    public int NowDragItemInfo; // 현재 드래그중인 아이템 정보

    public GameObject LeftHand_PocketObject; // 왼쪽 손 오브젝트
    public GameObject RightHand_PocketObject; // 오른쪽 손 오브젝트

    public int LeftHand_Pocket;
    public int RightHand_Pocket;

    public bool ItemBoxToHand;
    public bool ItemHandToBox;
    public bool ItemHTBEnable; // HandToBox하려니까 box에 collision 때문에
    public bool ItemHandToMaterialStorage;

    public int NowChoiceBox; // HandToBox -> 몇번째 박스인지

    public int LeftFlag = 0; // 왼손에 닿았는가ㅎ
    public int RightFlag = 0; // 오른손에 닿았는가 ㅎ

    public int UseInLeftHand = 0; // 재료상자용 
    public int UseInRightHand = 0;// 재료상자용

    public ChatGui chatGui;

    void Start()
    {//
     //   LeftUIImage = ItemInhand.transform.GetChild(0).GetChild(0).gameObject;
     //  RightUIImage = ItemInhand.transform.GetChild(1).GetChild(0).gameObject;
    }

    public void AddedItemBox(InBoxItem inbox)
    {
        boxItem.Add(inbox);
        boxNumber += 1;
    }
    public void DeletedItemBox(int index)
    {
        boxItem.RemoveAt(index);
        boxNumber -= 1;
    }

    public void Change_DragMouse(int _number)
    {

        DragCursorSprite.sprite = ItemImage[_number - 1];
        NowDragItemInfo = _number;
    }
    public void OnOff_DragMouse(bool _onoff)
    {
        if (_onoff)
        {
            DragCursorSprite.gameObject.SetActive(true);
        }
        else if (!_onoff)
        {
            DragCursorSprite.gameObject.SetActive(false);
            ItemBoxToHand = false;
            ItemHandToBox = false;
            NowDragItemInfo = 0;
        }
        TrainGameManager.instance.SoundManager.onButtonClickSound();
    }

    public void Position_DragMouse()
    {
        DragCursorSprite.transform.position = Input.mousePosition;
    }

    public void SetLeftHandItem()
    {
        TrainGameManager.instance.LeftHandItem = NowDragItemInfo;
        LeftHand_Pocket = NowDragItemInfo;
        LeftHand_PocketObject.GetComponent<Image>().sprite = ItemImage[LeftHand_Pocket - 1];
    }
    public void UseLeftHandItem()
    {
        TrainGameManager.instance.LeftHandItem = 0;
        LeftHand_Pocket = 0;
        LeftHand_PocketObject.GetComponent<Image>().sprite = NullImage;
        UseInLeftHand = 0;
        UseInRightHand = 0;
    }
    public void SetRightHandItem()
    {
        TrainGameManager.instance.RightHandItem = NowDragItemInfo;
        RightHand_Pocket = NowDragItemInfo;
        RightHand_PocketObject.GetComponent<Image>().sprite = ItemImage[RightHand_Pocket - 1];
    }
    public void UseRightHandItem()
    {
        TrainGameManager.instance.RightHandItem = 0;
        RightHand_Pocket = 0;
        RightHand_PocketObject.GetComponent<Image>().sprite = NullImage;
        UseInLeftHand = 0;
        UseInRightHand = 0;
    }

    public void ForItemToBoxIndex(int index)
    {
        NowChoiceBox = index;
    }

    public bool Usable_MediPack()
    {
        // 구급상자를 사용할 수 있는지
        for (int i = 0; i < boxItem.Count; i++)
        {
            for (int j = 0; j < GameValue.ITEMLIMIT; j++)
            {
                if (boxItem[i].HaveItemInfo[j] == (int)GameValue.itemCategory.medipack)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Use_MediPack()
    {
        // 구급상자를 사용
        for (int i = 0; i < boxItem.Count; i++)
        {
            for (int j = 0; j < GameValue.ITEMLIMIT; j++)
            {
                if (boxItem[i].HaveItemInfo[j] == (int)GameValue.itemCategory.medipack)
                {
                    boxItem[i].DeleteItem(j); // 아이템 지우기
                    break;
                }
            }
        }
    }

    public bool Usable_Food()
    {
        // 음식을 사용할 수 있는지
        for (int i = 0; i < boxItem.Count; i++)
        {
            for (int j = 0; j < GameValue.ITEMLIMIT; j++)
            {
                if (boxItem[i].HaveItemInfo[j] >= (int)GameValue.itemCategory.food_tomato && boxItem[i].HaveItemInfo[j] <= (int)GameValue.itemCategory.food_chicken)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Use_Food()
    {
        // 음식을 사용

        // 음식은 3,4,5 니까 박스에 있으며녀 사용
        for (int i = 0; i < boxItem.Count; i++)
        {
            for (int j = 0; j < GameValue.ITEMLIMIT; j++)
            {
                if (boxItem[i].HaveItemInfo[j] >= (int)GameValue.itemCategory.food_tomato && boxItem[i].HaveItemInfo[j] <= (int)GameValue.itemCategory.food_chicken)
                {
                    boxItem[i].DeleteItem(j); // 아이템 지우기
                    break;
                }
            }
        }
    }

    public void ItemGet_FoodTomato_Button()
    {
        photonView.RPC("ItemGet_FoodTomato", RpcTarget.All);
    }

    public void ItemGet_FoodBean_Button()
    {
        photonView.RPC("ItemGet_FoodBean", RpcTarget.All);
    }

    public void ItemGet_FoodChicken_Button()
    {
        photonView.RPC("ItemGet_FoodChicken", RpcTarget.All);
    }

    public void ItemGet_Nail_Button()
    {
        photonView.RPC("ItemGet_Nail", RpcTarget.All);
    }

    public void ItemGet_Hammer_Button()
    {
        photonView.RPC("ItemGet_Hammer", RpcTarget.All);

    }

    public void ItemGet_Ironpan_Button()
    {
        photonView.RPC("ItemGet_Ironpan", RpcTarget.All);
    }
    public void ItemGet_WoodBoard_Button()
    {
        photonView.RPC("ItemGet_WoodBoard", RpcTarget.All);
    }
    public void ItemGet_MediPack_Button()
    {
        photonView.RPC("ItemGet_MediPack", RpcTarget.All);
    }
    [PunRPC]
    public void ItemGet_Nail()
    {
        // 1 
        int boxCount = 0;

        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.nail);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }
    [PunRPC]
    public void ItemGet_Ironpan()
    {
        // 2
        int boxCount = 0;
        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.ironpan);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }
    [PunRPC]
    public void ItemGet_FoodTomato()
    {
        //3
        int boxCount = 0;

        // 모든 박스를 검사 중 해당 박스가 full이 아닐 경우
        // if(!boxitem[i].isboxfull())
        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.food_tomato);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }
    [PunRPC]
    public void ItemGet_FoodBean()
    {
        // 4
        int boxCount = 0;

        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.food_bean);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }

    }
    [PunRPC]
    public void ItemGet_FoodChicken()
    {
        // 5
        int boxCount = 0;
        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.food_chicken);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }
    [PunRPC]
    public void ItemGet_Hammer()
    {
        // 6
        int boxCount = 0;
        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.hammer);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }
    [PunRPC]
    public void ItemGet_MediPack()
    {
        //7
        int boxCount = 0;
        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.medipack);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }
    [PunRPC]
    public void ItemGet_WoodBoard()
    {
        //8
        int boxCount = 0;

        for (int i = 0; i < boxItem.Count; i++)
        {
            if (!boxItem[i].IsBoxFull())
            {
                // 앞의 순서대로 박스가 full이 아니면 여기에 들어가기
                boxItem[i].AddItem((int)GameValue.itemCategory.woodboard);
                break;
            }
            else //박스 꽉참
            {
                boxCount++;
            }
        }

        if (boxCount == boxItem.Count)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("박스의 인벤토리가 가득 차 아이템을 획득하지 못했습니다.");
        }
    }

    [PunRPC]
    public void VendingMachine_ItemGet(int Vnum)
    {
        switch (Vnum)
        {
            case 1:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_FoodTomato", RpcTarget.All);

                break;
            case 2:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_FoodBean", RpcTarget.All);
                break;
            case 3:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_FoodChicken", RpcTarget.All);
                break;
            case 4:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_Hammer", RpcTarget.All);
                break;
            case 5:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_Nail", RpcTarget.All);
                break;
            case 6:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_MediPack", RpcTarget.All);
                break;
            case 7:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_WoodBoard", RpcTarget.All);
                break;
            case 8:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                photonView.RPC("ItemGet_Ironpan", RpcTarget.All);
                break;
            case 627:
                photonView.RPC("vending_item", RpcTarget.All, Vnum);
                break;
        }

    }
    [PunRPC]
    public void vending_item(int itemcase)
    {


        if (itemcase == 627)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "돈이 충분하지 않습니다.");
            TrainGameManager.instance.Notice_Someting("돈이 충분하지 않습니다.");
        }
    

        if (boxItem.Count == 0)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "기차 내부에 박스가 없어서 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("기차 내부에 박스가 없어서 아이템을 획득하지 못했습니다.");
        }
        else
        {
            switch (itemcase)
            {
                case 1:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [토마토 스프]를 획득했습니다.");
                    break;
                case 2:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [콩 스프]을 획득했습니다.");
                    break;
                case 3:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [치킨 스프]를 획득했습니다.");
                    break;
                case 4:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [망치]를 획득했습니다.");
                    break;
                case 5:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [못]을 획득했습니다.");
                    break;
                case 6:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [구급상자]를 획득했습니다.");
                    break;
                case 7:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [나무판자]를 획득했습니다.");
                    break;
                case 8:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [철판]을 획득했습니다.");
                    break;
            }
        }
    }
    public void ItemGet_Random(int viewID)
    {
        int ItemNumber = Random.Range(0, (int)GameValue.itemCategory.ironpan + 1);

        switch (ItemNumber)
        {
            case 1:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_FoodTomato", RpcTarget.All);

                break;
            case 2:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_FoodBean", RpcTarget.All);
                break;
            case 3:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_FoodChicken", RpcTarget.All);
                break;
            case 4:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_Hammer", RpcTarget.All);
                break;
            case 5:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_Nail", RpcTarget.All);
                break;
            case 6:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_MediPack", RpcTarget.All);
                break;
            case 7:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_WoodBoard", RpcTarget.All);
                break;
            case 8:
                photonView.RPC("setEggActiveFalse", RpcTarget.All, viewID, ItemNumber);
                photonView.RPC("ItemGet_Ironpan", RpcTarget.All);
                break;
        }
    }
    [PunRPC]
    public void setEggActiveFalse(int viewID, int itemCase)
    {
        GameObject _other = PhotonView.Find(viewID).gameObject;
        _other.gameObject.SetActive(false);


        if (boxItem.Count == 0)
        {
            chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "기차 내부에 박스가 없어서 아이템을 획득하지 못했습니다.");
            TrainGameManager.instance.Notice_Someting("기차 내부에 박스가 없어서 아이템을 획득하지 못했습니다.");
        }
        else
        {
            switch (itemCase)
            {
                case 1:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [토마토 스프]를 획득했습니다.");
                    break;
                case 2:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [콩 스프]을 획득했습니다.");
                    break;
                case 3:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [치킨 스프]를 획득했습니다.");
                    break;
                case 4:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [망치]를 획득했습니다.");
                    break;
                case 5:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [못]을 획득했습니다.");
                    break;
                case 6:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [구급상자]를 획득했습니다.");
                    break;
                case 7:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [나무판자]를 획득했습니다.");
                    break;
                case 8:
                    chatGui.chatClient.PublishMessage(chatGui.selectedChannelName, "아이템 [철판]을 획득했습니다.");
                    break;
            }
        }
    }
}
