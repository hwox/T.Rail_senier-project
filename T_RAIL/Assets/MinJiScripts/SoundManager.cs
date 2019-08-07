using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviourPunCallbacks
{

    public AudioSource BGM_Source;
    public AudioSource Effect1_Source;
    public AudioSource Effect2_Source;
    public AudioSource Effect3_Source;
    public AudioSource UI1_Source;
    public AudioSource UI2_Source;
    public AudioSource foot_Source;
    public AudioSource TrainDriving_Source;

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
    public AudioClip passenger_die_Sound; //승객 죽을때 
    public AudioClip train_door_open_Sound; // 역에서 그냥 틀고 시작 기차 문열리는소리
    public AudioClip ExitWindow_Sound; // 끌거냐고 묻는 UI
    public AudioClip ladder_Sound; //사다리 오르고 내리는 소리 
    public AudioClip InvenOpen_Sound;

    public AudioClip foot_Sound; //플레이어 발자국소리
    public AudioClip SitMachineGun_Sound; // 기관총에 앉았을 때
    public AudioClip TrainDriving_Sound; // 기차 움직일 때 내는 소리
    public AudioClip ChickenDie_Sound; // 닭 죽을 때
    public AudioClip HPIncrease_Sound; // 자판기에서 하트 먹을 때
    public AudioClip EggEat_Sound; // 달걀 먹었을 때

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

    //  TrainGameManager.instance.SoundManager.Box_Sofa_instance_Sound_Play();

    public void Box_Sofa_instance_Sound_Play()
    {
        Effect2_Source.clip = Box_Sofa_instance_Sound;
        Effect2_Source.Play();
    }

    public void coin_Sound_Play()
    {
        Effect1_Source.clip = coin_Sound;
        Effect1_Source.Play();
    }

    public void buy_item_Sound_Play()
    {
        Effect2_Source.clip = buy_item_Sound;
        Effect2_Source.Play();
    }

    public void ExitWindow_Sound_Play()
    {
        Effect2_Source.clip = ExitWindow_Sound;
        Effect2_Source.Play();
    }

    public void Train_Break_Sound_Play()
    {
        Effect1_Source.clip = train_Break_Sound;
        Effect1_Source.Play();
    }

    public void Train_Treat_Sound_Play()
    {
        Effect1_Source.clip = train_Treat_Sound;
        Effect1_Source.Play();
    }

    public void Hunger_Treat_Sound_Play()
    {
        Effect1_Source.clip = hunger_Treat_Sound;
        Effect1_Source.Play();
    }

    public void Disease_Treat_Sound_Play()
    {
        Effect1_Source.clip = disease_Treat_Sound;
        Effect1_Source.Play();
    }

    public void Train_door_open_Sound_Play()
    {
        Effect1_Source.clip = train_door_open_Sound;
        Effect1_Source.Play();
    }

    public void Ladder_Sound_Play()
    {
        UI2_Source.clip = ladder_Sound;
        UI2_Source.Play();
    }
    public void OpenInven_Sound_Play()
    {
        Effect2_Source.clip = InvenOpen_Sound;
        Effect2_Source.Play();
    }

    public void Passenger_Die_Sound_Play()
    {
        Effect2_Source.clip = passenger_die_Sound;
        Effect2_Source.Play();
    }

    public void SitMachineGun_Sound_Play()
    {
        Effect2_Source.clip = SitMachineGun_Sound;
        Effect2_Source.Play();
    }
    public void Player_foot_Sound_Play()
    {
        foot_Source.clip = foot_Sound;
        foot_Source.Play();
    }

    public void TrainDriving_Sound_Play()
    {
        TrainDriving_Source.clip = TrainDriving_Sound;
        TrainDriving_Source.Play();
    }
    public void TrainDriving_Sound_Stop()
    {
        TrainDriving_Source.Stop();
    }

    public void HPIncrease_Sound_Play()
    {
        Effect1_Source.clip = HPIncrease_Sound;
        Effect1_Source.Play();
    }

    public void ChickenDie_Sound_Play()
    {
        Effect2_Source.clip = ChickenDie_Sound;
        Effect2_Source.Play();
    }
    public void EggEat_Sound_Play()
    {
        Effect1_Source.clip = EggEat_Sound;
        Effect1_Source.Play();
    }
}
