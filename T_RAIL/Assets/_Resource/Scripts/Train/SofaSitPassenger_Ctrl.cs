using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaSitPassenger_Ctrl : MonoBehaviour {



    // 현재 소파에 앉아있는 승객을 총관리
    public List<InSofaPassenger> passengers = new List<InSofaPassenger>();

    int sofaNubmer; // 소파 총 몇개인지


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void AddedSofa(InSofaPassenger insofa)
    {
        passengers.Add(insofa);
        sofaNubmer += 1;
    }
    public void DeletedSofa(int index)
    {
        passengers.RemoveAt(index);
        sofaNubmer -= 1;
    }
}
