using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SettingCtrl : MonoBehaviour
{


    public GameObject Setting_Window;
    public GameObject Book_Window;

    public GameObject HandUI;
    public GameObject HPUI;
    public bool HandButtonOn { get; set; }
    public bool HPButtonOn { get; set; }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void On_SettingWindow()
    {
        Setting_Window.SetActive(true);
    }

    public void Off_SettingWindow()
    {
        Setting_Window.SetActive(false);
    }

    public void On_BookWindow()
    {
        Book_Window.SetActive(true);
        Book_Window.GetComponent<ShowStateInNotePad>().OpenNotePad();
    }
    public void Off_BookWindow()
    {
        Book_Window.SetActive(false);
        Book_Window.GetComponent<ShowStateInNotePad>().CloseNotePad();
    }

    public void HPButtonClick()
    {
        if (!HPButtonOn)
        {
            HPUI.SetActive(true);
            HPButtonOn = true;
        }
        else if(HPButtonOn)
        {
            HPUI.SetActive(false);
            HPButtonOn = false;
        }
    }



    public void HandButtonClick()
    {
        if (!HandButtonOn)
        {
            HandUI.SetActive(true);
            HandButtonOn = true;
        }
        else if(HandButtonOn)
        {
            HandUI.SetActive(false);
            HandButtonOn = false;
        }

    }
}
