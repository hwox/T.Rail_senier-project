using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    enum SoundList
    {
        buttonClickSound = 0,
        stage1BGM = 1
    }

    enum AudioSourceList
    {
        lobbyScene = 0,
        mainScene = 1
    }

    public AudioSource[] audioSources;
    public AudioClip[] sounds;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void onButtonClickSound()
    {
        //sounds[]
        //audioSources[SoundList.buttonClickSound] = 
    }
}
