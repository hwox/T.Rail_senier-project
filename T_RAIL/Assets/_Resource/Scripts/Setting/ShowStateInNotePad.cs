using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStateInNotePad : MonoBehaviour
{

    string[] TrainIndexText = new string[] {"첫 번째 칸", "두 번째 칸", "세 번째 칸", "네 번째 칸","다섯번째 칸", "여섯번째 칸", "일곱번째 칸", "여덟번째 칸",
        "아홉번째 칸", "열번째 칸", "열 한번째 칸", "열 두번째 칸", "열 세번째 칸" };

    public Sprite[] HumanImage;
    public Sprite[] Heart;

    public GameObject[] PassengerState;
    public GameObject[] ItemBoxState;
    public GameObject Line; // 선 구분하는 line들
    public GameObject PageX; // 기차가 안붙어있어서 UI 를 확인할수 없음
    public Text WhereTrainText;// 몇번째칸인지 써져있는 text
    Train_Ctrl TrainCtrl; // 기차 오브젝트 뭐 있는지 가져오려고 
    int TrainInformationIndex;

    int[] TrainObject; // 1-> 소파, 2->박스, 0-> 아무것도 안만들어진상태

    // bool NotOpen; // 아직 그만큼 기차가 안 붙어있을 경우

    // Use this for initialization

    bool UsingNotePad;



    void Start()
    {
        TrainObject = new int[4];
        TrainCtrl = TrainGameManager.instance.TrainCtrl;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void NextTrainInformation()
    {
        if (TrainInformationIndex < 12)
        {
            TrainInformationIndex += 1;
            WhereTrainText.text = TrainIndexText[TrainInformationIndex];
            ChangeTrainIndex();
        }

    }
    public void PrevTrainInformation()
    {
        if (TrainInformationIndex > 0)
        {

            TrainInformationIndex -= 1;
            WhereTrainText.text = TrainIndexText[TrainInformationIndex];
            ChangeTrainIndex();
        }

    }
    public void OpenNotePad()
    {
        UsingNotePad = true;
        StartCoroutine(StateInformationRenewal());
    }
    public void CloseNotePad()
    {
        UsingNotePad = false;
        StopCoroutine(StateInformationRenewal());
    }

    void ChangeTrainIndex()
    {
        // 여기 true는 index가 총 기차 길이를 초과하지 않았을 때
        // Traininformationindex는 0부터 시작
        // trainindex는 1부터 시작(train의 count로 index에 넣고있음)
        if (TrainInformationIndex < TrainGameManager.instance.trainindex)
        {
            Line.SetActive(true);
            PageX.SetActive(false);
           // WhereTrainText.text = TrainIndexText[TrainInformationIndex];

            Train_Object train = TrainCtrl.trainscript[TrainInformationIndex];


            for (int i = 0; i < 4; i++)
            {
                if (train.InTrainObjectUsed[i])
                {
                    TrainObject[i] = train.ThisTrainNowObjects[i];

                    switch (TrainObject[i])
                    {
                        case 1:
                            PassengerState[i].SetActive(true);
                            PassengerStateSetting(i);
                            //소파
                            break;
                        case 2:
                            //박스
                            ItemBoxState[i].SetActive(true);
                            ItemBoxInformationSetting(i);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // 안쓰고 있음 
                    TrainObject[i] = 0;
                }
            }
        }
        else
        {
            NotOpenTrain();
        }
    }

    void NotOpenTrain()
    {
        AllStateOff();
        Line.SetActive(false);
        PageX.SetActive(true);
    }

    void ItemBoxInformationSetting(int index)
    {
        int[] item = new int[6];
        AllItem_Ctrl allitem = TrainGameManager.instance.allitemCtrl;
        for (int i = 0; i < 6; i++)
        {
            item[i] = allitem.boxItem[index].HaveItemInfo[i];

            if (item[i] != 0)
            {
                ItemBoxState[i].transform.GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[item[i]];
            }
            else
            {
                ItemBoxState[i].transform.GetChild(i).GetComponent<Image>().sprite = allitem.NullImage;
            }

        }
    }
    void PassengerStateSetting(int index)
    {
        int hungry = TrainGameManager.instance.SofaSitPassengerCtrl.passengers[index].GetThisPassengerHungry();
        int disease = TrainGameManager.instance.SofaSitPassengerCtrl.passengers[index].GetThisPassengerHungry();

        PassengerState[index].transform.GetChild(0).GetComponent<Slider>().value = (float)(hungry / 100);
        PassengerState[index].transform.GetChild(1).GetComponent<Image>().sprite = DiseaseHeartSprite(disease);
        PassengerState[index].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = disease.ToString();
        PassengerState[index].transform.GetChild(2).GetComponent<Image>().sprite = GetHumanProfileSprite(index);

    }
    Sprite DiseaseHeartSprite(int count)
    {
        if (count >= 100)
        {
            return Heart[4];
        }
        if (count >= 75 && count < 100)
        {
            return Heart[3];
        }
        else if (count >= 50 && count < 75)
        {
            return Heart[2];
        }
        else if (count >= 25 && count < 50)
        {
            return Heart[1];
        }
        else
        {
            return Heart[0];
        }

    }
    Sprite GetHumanProfileSprite(int index)
    {
        // 그래도 똑같은 위치에 있는 사람 이미지는 항사 ㅇ같아야하니까
        // 몇번째 인덱스, 몇번째 기차에 있는지에 따라서 
        int ProfileIndex = TrainInformationIndex + index;
        ProfileIndex = ProfileIndex * 4 % 10;

        return HumanImage[ProfileIndex];

    }
    void AllStateOff()
    {
        for (int i = 0; i < 4; i++)
        {
            ItemBoxState[i].SetActive(false);
            PassengerState[i].SetActive(false);
        }
    }
    // 코루틴으로 state계속 불리는거 

    IEnumerator StateInformationRenewal()
    {
        // 얘는 먼저 실행시킨적이 없는데 스스로 혼자 실행되면서 null ref 오류가 뜸
        // 왜? 그리고 trainctrl도 null 뜨는데
        yield return new WaitForSeconds(5.0f);
        while (UsingNotePad)
        {
            for (int i = 0; i < 4; i++)
            {
                switch (TrainObject[i])
                {
                    case 1:
                        PassengerStateSetting(i);
                        break;
                    case 2:
                        ItemBoxInformationSetting(i);
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }

    }
}
