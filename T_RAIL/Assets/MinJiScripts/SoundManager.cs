using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviourPunCallbacks {

    enum AudioClipList
    {
        buttonClickSound = 0,
        stage1BGM = 1
    }

    enum AudioSourceList
    {
        lobbyScene = 0,
        mainScene = 1
    }

    public static SoundManager instance = null;


    public AudioSource[] audioSources;
    public AudioClip[] sounds;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }


    public void onButtonClickSound()
    {
        audioSources[(int)AudioSourceList.lobbyScene].PlayOneShot(sounds[(int)AudioClipList.buttonClickSound]);
        //audioSources[(int)AudioSourceList.lobbyScene].clip = sounds[(int)AudioClipList.buttonClickSound];
        //audioSources[(int)AudioSourceList.lobbyScene].Play();
    }


    public void TrainStage1_BGMSoundPlay()
    {
        audioSources[(int)AudioSourceList.mainScene].clip = (sounds[(int)AudioClipList.stage1BGM]);
        audioSources[(int)AudioSourceList.mainScene].Play();
    }
}
