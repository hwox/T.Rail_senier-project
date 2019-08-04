using System.Collections;
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
    [Header("Lobby Sound")]
    public AudioClip buttonClickSound;

    //stage1 초원
    [Header("Stage1 Sound")]
    public AudioClip stage1BGM;
    public AudioClip enemy_Sound;
    public AudioClip Train_Sound;
    public AudioClip enemy_attack_Sound;


    //stage2 사막
    [Header("Stage2 Sound")]
    public AudioClip stage2BGM;


    //stage3 설원
    [Header("Stage3 Sound")]
    public AudioClip stage3BGM;


    [Header("Common Sound")]
    public AudioClip Machine_Gun_Sound;
    public AudioClip InGameButtonSound; 
    public AudioClip Box_Sofa_instance_Sound; //박스 소파 생성 시
    public AudioClip train_Break_Sound; //기차 뚫릴 먹을시
    public AudioClip train_Treat_Sound; //기차 수리시
    public AudioClip hunger_Treat_Sound; //음식 먹을시
    public AudioClip disease_Treat_Sound; //질병 치료시
    public AudioClip coin_Sound; //승객 먹을때, 닭잡을때
    public AudioClip buy_item_Sound; //자판기 소리

    public AudioClip ExitWindow_Sound; // 끌거냐고 묻는 UI


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
        BGM_Source.clip = stage1BGM;
        BGM_Source.Play();
    }

    public void TrainStage2_BGMSoundPlay()
    {
        BGM_Source.clip = stage2BGM;
        BGM_Source.Play();
    }

    public void TrainStage3_BGMSoundPlay()
    {
        BGM_Source.clip = stage3BGM;
        BGM_Source.Play();
    }

    public void Machine_Gun_Sound_Play()
    {
        Effect1_Source.clip = Machine_Gun_Sound;
        Effect1_Source.Play();
    }

    public void enemy_Sound_Play()
    {
        Effect2_Source.clip = enemy_Sound;
        Effect2_Source.Play();
    }

    public void Train_Sound_Play()
    {
        Effect3_Source.clip = Train_Sound;
        Effect3_Source.Play();
    }

    public void enemy_attack_Sound_Play()
    {
        Effect2_Source.clip = enemy_attack_Sound;
        Effect2_Source.Play();
    }

    public void ExitWindow_Sound_Play()
    {
        Effect2_Source.clip = ExitWindow_Sound;
        Effect2_Source.Play();
    }
}
