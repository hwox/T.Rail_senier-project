using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class VendingMachine : MonoBehaviour {

    public GameObject[] button;

    public GameObject cam;
    public GameObject itemCtrl;
    //public Camera cam;

    public GameObject item1;

    public Player_Actor customer;
    public GameObject myPlayer;

    GameObject targetbutton;

    //public bool VendingMachine_on = false;
     int[] item_price = new int[9] { 2, 2, 2, 2, 1, 4, 1, 2,2 };
    // Use this for initialization
    void Start () {
        cam = GameObject.Find("VendingMachineCam");
        itemCtrl = GameObject.Find("Item_Ctrl");


        




    }

    // Update is called once per frame
    void Update()
    {

        if (TrainGameManager.instance.VendingMachineOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
               
                targetbutton = GetClickobject();
                for (int i = 0; i < button.Length; i++)
                {
                    
                    if (targetbutton == button[i])
                    {
                      
                        StartCoroutine(ClickButton(button[i]));


                      
                        if (TrainGameManager.instance.CoinNum >= item_price[i])
                        {
                            StartCoroutine(itemdrop(i));
                            if (i+1==9)
                            {
                                TrainGameManager.instance.SoundManager.HPIncrease_Sound_Play();
                                myPlayer.GetComponent<PhotonView>().RPC("setPlayerHP", RpcTarget.All, myPlayer.GetComponent<PhotonView>().ViewID, -10);
                                //customer.HP += 5;
                                TrainGameManager.instance.CoinNum -= item_price[8];
                            }
                            else
                            {
                                //TrainGameManager.instance.CoinNum = TrainGameManager.instance.CoinNum - item_price[i];
                                TrainGameManager.instance.GetComponent<PhotonView>().RPC("getCoin_RPC", RpcTarget.All, -item_price[i]);
                                itemCtrl.GetComponent<AllItem_Ctrl>().VendingMachine_ItemGet(i + 1);
                            }
                     
                           
                        }
                        else
                        {
                            //Debug.Log("돈 부족");
                            itemCtrl.GetComponent<AllItem_Ctrl>().VendingMachine_ItemGet(627);
                        }
                    }
                }
            }
        }
    
	}

    GameObject GetClickobject()
    {
        RaycastHit hit;
        GameObject Cbutton = null;
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Cbutton = hit.collider.gameObject;
            //Debug.Log(Cbutton.name);
        }
            return Cbutton;
    }


    IEnumerator ClickButton(GameObject button)
    {

        button.GetComponent<Animator>().SetBool("click", true);
        //Debug.Log("클릭");
        yield return new WaitForSeconds(0.2f);

        button.GetComponent<Animator>().SetBool("click", false);

    }
     IEnumerator itemdrop(int i)
     {

        GameObject temp = TrainGameManager.instance.GetObject(i+9);
        temp.SetActive(true);
        temp.transform.position = item1.transform.position;

        TrainGameManager.instance.SoundManager.buy_item_Sound_Play();

        temp.GetComponent<Rigidbody>().AddForce(Vector3.back *3 , ForceMode.Impulse);
        yield return new WaitForSeconds(3.0f);
        temp.SetActive(false);
    }
}
