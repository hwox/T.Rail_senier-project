﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviourPunCallbacks {

    public AudioSource BGM_Source;
    public AudioSource Effect1_Source;
    public AudioSource Effect2_Source;
    public AudioSource Effect3_Source;
    public AudioSource UI1_Source;
    public AudioSource UI2_Source;


    //lobby
    public AudioClip buttonClickSound;

    //stage1
    public AudioClip stage1BGM;
    public AudioClip Machine_Gun_Sound;
    public AudioClip InGameButtonSound;
    public AudioClip enemy_Sound;
    public AudioClip Train_Sound;
    public AudioClip enemy_attack_Sound;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void onButtonClickSound()
    {
        UI1_Source.PlayOneShot(InGameButtonSound);
    }

    public void TrainStage1_BGMSoundPlay()
    {
        //BGM_Source.PlayOneShot(stage1BGM);
        BGM_Source.clip = stage1BGM;
        BGM_Source.Play();
    }

    public void Machine_Gun_Sound_Play()
    {
        Effect1_Source.clip = Machine_Gun_Sound;
        Debug.Log("플레이는있는데 왜 소리가 안나세요?");
        Effect1_Source.Play();
        //Effect1_Source.PlayOneShot(Machine_Gun_Sound);
    }

    public void enemy_Sound_Play()
    {
        Effect2_Source.clip = enemy_Sound;
        Effect2_Source.Play();
        //Effect2_Source.PlayOneShot(enemy_Sound);
    }

    public void Train_Sound_Play()
    {
        Effect3_Source.clip = Train_Sound;
        Effect3_Source.Play();
        //Effect3_Source.PlayOneShot(Train_Sound);
    }

    public void enemy_attack_Sound_Play()
    {
        Effect2_Source.clip = enemy_attack_Sound;
        Effect2_Source.Play();
        //Effect3_Source.PlayOneShot(Train_Sound);
    }
}
