using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviourPunCallbacks {

    public AudioSource Lobby_AudioSource;
    public AudioSource Stage1_AudioSource;

    //lobby
    public AudioClip buttonClickSound;

    //stage1
    public AudioClip stage1BGM;
    public AudioClip Machine_Gun_Sound;
    public AudioClip InGameButtonSound;
    public AudioClip enemy_Sound;
    public AudioClip Train_Sound;

    //enum AudioClipList
    //{
    //    buttonClickSound = 0,
    //    stage1BGM = 1,
    //    Machine_Gun_Sound = 2,
    //    InGameButtonSound = 3,
    //    enemy_Sound = 4,
    //    Train_Sound = 5
    //}
    //
    //enum AudioSourceList
    //{
    //    lobbyScene = 0,
    //    mainScene = 1
    //}

    public static SoundManager instance = null;


    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this);
    }


    public void onButtonClickSound()
    {
        Lobby_AudioSource.PlayOneShot(buttonClickSound);
        //audioSources[(int)AudioSourceList.lobbyScene].PlayOneShot(sounds[(int)AudioClipList.buttonClickSound]);
    }


    public void TrainStage1_BGMSoundPlay()
    {
        Stage1_AudioSource.clip = stage1BGM;
        Stage1_AudioSource.Play();
        //audioSources[(int)AudioSourceList.mainScene].clip = (sounds[(int)AudioClipList.stage1BGM]);
        //audioSources[(int)AudioSourceList.mainScene].Play();
    }

    public void onButtonClickSound_ingame()
    {
        Lobby_AudioSource.PlayOneShot(buttonClickSound);
    }

    public void Machine_Gun_Sound_Play()
    {
        Stage1_AudioSource.PlayOneShot( Machine_Gun_Sound);
    }

    public void Train_Sound_Play()
    {
        Stage1_AudioSource.PlayOneShot( Train_Sound);
    }
}
