using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStateInNotePad : MonoBehaviour
{

    string[] TrainIndexText = new string[] {"첫 번째 칸", "두 번째 칸", "세 번째 칸", "네 번째 칸","다섯번째 칸", "여섯번째 칸", "일곱번째 칸", "여덟번째 칸",
        "아홉번째 칸", "열번째 칸", "열 한번째 칸", "열 두번째 칸", "열 세번째 칸" };

    public GameObject[] PassengerState;
    public GameObject[] ItemBoxState;
    public GameObject Line; // 선 구분하는 line들
    public GameObject PageX; // 기차가 안붙어있어서 UI 를 확인할수 없음
    public Text WhereTrainText;// 몇번째칸인지 써져있는 text

    int TrainInformationIndex;

    int[] TrainObject;

   // bool NotOpen; // 아직 그만큼 기차가 안 붙어있을 경우

    // Use this for initialization
    void Start()
    {
        TrainObject = new int[4];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextTrainInformation()
    {
        // 얘를 뭘로 바꿀거냐면 기차 지금 어디까지 붙어있는지
        if (TrainInformationIndex < 12)
        {
            TrainInformationIndex += 1;
            ChangeTrainIndex();
        }
        else
        {
            NotOpenTrain();
        }
    }
    public void PrevTrainInformation()
    {
        if (TrainInformationIndex > 0)
        {
            TrainInformationIndex -= 1;
            ChangeTrainIndex();
        }
        else
        {
            NotOpenTrain();
        }
    }

    void ChangeTrainIndex()
    {
        Line.SetActive(true);
        PageX.SetActive(falses);
        WhereTrainText.text = TrainIndexText[TrainInformationIndex];

        for (int i = 0; i < 4; i++)
        {

        }

    }

    void NotOpenTrain()
    {
        Line.SetActive(false);
        PageX.SetActive(true);
    }
}
