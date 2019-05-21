using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StateController_Ctrl : MonoBehaviourPunCallbacks {

    public Text UsableStatus; // 사용할 수 있는 스텟
    


    [PunRPC]
    public void stateChange(int category, bool isUp)
    {
        switch (category)
        {
            //speed
            case 0:
                if(true == isUp)
                    TrainGameManager.instance.Speed_stat += 1;
                else
                    TrainGameManager.instance.Speed_stat -= 1;
                break;

            //noise
            case 1:
                if (true == isUp)
                    TrainGameManager.instance.Noise_stat += 1;
                else
                    TrainGameManager.instance.Noise_stat -= 1;
                break;
            
            //defence
            case 2:
                if (true == isUp)
                    TrainGameManager.instance.Defence_stat += 1;
                else
                    TrainGameManager.instance.Defence_stat -= 1;
                break;
        }

    }


    public void SpeedUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
            GameValue.StatusMAX > TrainGameManager.instance.Speed_stat)
        {
            //TrainGameManager.instance.Speed_stat += 1;
            photonView.RPC("stateChange", RpcTarget.All, 0, true);
            NowStat();    
        }
        else
        {
        }
    }

    public void SpeedDOWN()
    {
        if (TrainGameManager.instance.Speed_stat > 0)
        {
            //TrainGameManager.instance.Speed_stat -= 1;
            //Debug.Log("Speed_state : " + TrainGameManager.instance.Speed_stat);
            photonView.RPC("stateChange", RpcTarget.All, 0, false);

            NowStat();
        }
    }

    public void NoiseUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
           GameValue.StatusMAX < TrainGameManager.instance.Noise_stat)
        {
            //TrainGameManager.instance.Noise_stat += 1;
            photonView.RPC("stateChange", RpcTarget.All, 1, true);
            NowStat();
        }
    }

    public void NoiseDOWN()
    {
        if (TrainGameManager.instance.Noise_stat > 0)
        {
            photonView.RPC("stateChange", RpcTarget.All, 1, false);
            //TrainGameManager.instance.Noise_stat -= 1;
            NowStat();
        }
    }

    public void DefenceUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
           GameValue.StatusMAX < TrainGameManager.instance.Defence_stat)
        {
            photonView.RPC("stateChange", RpcTarget.All, 2, true);
            //TrainGameManager.instance.Defence_stat += 1;
            NowStat();
        }
    }

    public void DefenceDOWN()
    {
        if (TrainGameManager.instance.Defence_stat > 0)
        {
            photonView.RPC("stateChange", RpcTarget.All, 2, false);
            //TrainGameManager.instance.Defence_stat -= 1;
            NowStat();
        }
    }


    public void NowStat()
    {
        // int 계산
     //   UsableStatus.text = 계산.ToString();
    }
}
