using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SettingCtrl : MonoBehaviour {


    public GameObject Setting_Window;
    public GameObject Book_Window;

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
    }
    public void Off_BookWindow()
    {
        Book_Window.SetActive(false);
    }


}
