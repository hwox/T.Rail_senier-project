using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public bool attack = false;

    // Use this for initialization
    void Start () {
		
	}

    private void OnTriggerStay(Collider other)
    {
     
        if (other.gameObject.layer.Equals(GameValue.chicken_layer))
        {

            Debug.Log("공격");
            if ( attack )
            {
                if (other.gameObject.GetComponent<Chicken_Ctrl>().live)
                {
                   
                   
                    other.gameObject.GetComponent<Chicken_Ctrl>().DestPerson = this.gameObject;//닭이 따라가는 대상 선정
                    other.gameObject.GetComponent<Chicken_Ctrl>().BeatenTrue();// 닭 애니메이션 및 행동 변경 함수 호출
                    other.gameObject.GetComponent<Chicken_Ctrl>().gameObject.transform.Translate(0, 0, -1.0f);// 뒤로 밀어내기

                    gameObject.transform.parent.GetComponent<Player_Ctrl>().attack_possible = false;
                }

            }
            
        }

        attack = false;
    }

        // Update is called once per frame
    void Update () {
		
	}
}
