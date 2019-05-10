using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrain : MonoBehaviour {

    public GameObject trianPrefab;
    public int trainNum=3; // 기차 생성 개수
    public int trainGap = 12;// 기차 간격
    void newtrain(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Instantiate(trianPrefab, new Vector3(6-(i * trainGap), 1, 7.5f), trianPrefab.transform.rotation);
        }
    
    }
	// Use this for initialization
	void Start () {
        newtrain(trainNum);

    }
	
   
	// Update is called once per frame
	void Update () {
		
	}
}
