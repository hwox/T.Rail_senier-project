using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamera_Ctrl : MonoBehaviour {

    Camera MCam;
    Camera ControllerCam;
    public GameObject ExitStateController; // 일단 ㅇ녀기다가 갖다놨음 

    public void Awake()
    {
        MCam = Camera.main;
        ControllerCam = GetComponent<Camera>();
    }
    public void On_StateController()
    {
        MCam.enabled = false;
        ControllerCam.enabled = true;
        ExitStateController.SetActive(true);

        // 근데  이거 thiscamon은 없애도 될 거 ㅅ가은데
       // MCam.GetComponent<Mouse_Ctrl>().ThisCamOn = false;
        this.GetComponent<Camera>().GetComponent<Mouse_Ctrl>().ThisCamSetOnOff(false);

    }

    public void Exit_StateController()
    {
        MCam.enabled = true;
       // MCam.GetComponent<Mouse_Ctrl>().ThisCamOn = true;
        this.GetComponent<Camera>().GetComponent<Mouse_Ctrl>().ThisCamSetOnOff(true);
        ControllerCam.enabled = false;
        ExitStateController.SetActive(false);

    }
}
