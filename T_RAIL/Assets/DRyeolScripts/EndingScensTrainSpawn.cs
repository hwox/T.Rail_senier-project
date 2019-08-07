using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScensTrainSpawn : MonoBehaviour {


    public GameObject trianPrefab;
    public int trainNum = 3; // 기차 생성 개수
    public float trainGap = 13f;// 기차 간격

    public GameObject ExitGameWindow;

    void newtrain(int n)
    {
        for (int i = 0; i < n; i++)
        {
            GameObject train = TrainGameManager.instance.GetObject(18);
            train.SetActive(true);
            train.transform.position = new Vector3(-57 - (i * trainGap), 2.87f, -9.7f);

          
            train.transform.parent = this.transform;
        }

    }
    // Use this for initialization
    void Start () {
        trainNum = TrainGameManager.instance.trainindex;
        newtrain(trainNum);
        StartCoroutine(ExitGame());

    }
    void Update(){

        transform.Translate(0.3f, 0, 0);

    }

    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            if (!ExitGameWindow.active)
            {
                ExitGameWindow.SetActive(true);
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

}
