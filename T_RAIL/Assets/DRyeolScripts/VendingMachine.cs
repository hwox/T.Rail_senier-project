using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour {

    public GameObject[] button;
    public GameObject cam;
    //public Camera cam;
    GameObject targetbutton;

    public bool VendingMachine_on = false;

    // Use this for initialization
    void Start () {
        cam = GameObject.Find("VendingMachineCam");

       




    }

    // Update is called once per frame
    void Update()
    {

        //if (VendingMachine_on)
        {
            if (Input.GetMouseButtonDown(0))
            {
               
                targetbutton = GetClickobject();
                for (int i = 0; i < button.Length; i++)
                {
                    
                    if (targetbutton == button[i])
                    {
                        Debug.Log("마우스 클릭");
                        StartCoroutine(ClickButton(button[i]));
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
            Debug.Log(Cbutton.name);
        }
            return Cbutton;
    }


    IEnumerator ClickButton(GameObject button)
    {

        button.GetComponent<Animator>().SetBool("click", true);
        Debug.Log("클릭");
        yield return new WaitForSeconds(0.2f);

        button.GetComponent<Animator>().SetBool("click", false);

    }
}
