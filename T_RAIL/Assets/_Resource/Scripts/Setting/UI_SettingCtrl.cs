using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SettingCtrl : MonoBehaviour
{


    public GameObject Exit_Window;
    public GameObject Book_Window;

    public GameObject HandUI;
    public GameObject HPUI;
    public bool HandButtonOn { get; set; }
    public bool HPButtonOn { get; set; }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void On_ExitWindow()
    {
        TrainGameManager.instance.SoundManager.ExitWindow_Sound_Play();
        Exit_Window.SetActive(true);
    }

    public void Off_ExitWindow()
    {
        TrainGameManager.instance.SoundManager.onButtonClickSound();
        Exit_Window.SetActive(false);
    }
    public void GameExit()
    {
        Application.Quit();
    }

    public void On_BookWindow()
    {
        TrainGameManager.instance.SoundManager.onButtonClickSound();
        Book_Window.SetActive(true);
        Book_Window.GetComponent<ShowStateInNotePad>().OpenNotePad();
    }
    public void Off_BookWindow()
    {
        TrainGameManager.instance.SoundManager.onButtonClickSound();
        Book_Window.SetActive(false);
        Book_Window.GetComponent<ShowStateInNotePad>().CloseNotePad();
    }

    public void HPButtonClick()
    {
        TrainGameManager.instance.SoundManager.onButtonClickSound();
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
        TrainGameManager.instance.SoundManager.onButtonClickSound();
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
