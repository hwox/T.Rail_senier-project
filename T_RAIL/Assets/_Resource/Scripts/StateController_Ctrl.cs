using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StateController_Ctrl : MonoBehaviour {

    public Text UsableStatus; // 사용할 수 있는 스텟
    
    public void SpeedUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
            GameValue.StatusMAX > TrainGameManager.instance.Speed_stat)
        {
            TrainGameManager.instance.Speed_stat += 1;
          
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
            TrainGameManager.instance.Speed_stat -= 1;
            Debug.Log("Speed_state : " + TrainGameManager.instance.Speed_stat);
            NowStat();
        }
    }

    public void NoiseUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
           GameValue.StatusMAX < TrainGameManager.instance.Noise_stat)
        {
            TrainGameManager.instance.Noise_stat += 1;
            NowStat();
        }
    }

    public void NoiseDOWN()
    {
        if (TrainGameManager.instance.Noise_stat > 0)
        {
            TrainGameManager.instance.Noise_stat -= 1;
            NowStat();
        }
    }

    public void DefenceUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
           GameValue.StatusMAX < TrainGameManager.instance.Defence_stat)
        {
            TrainGameManager.instance.Defence_stat += 1;
            NowStat();
        }
    }

    public void DefenceDOWN()
    {
        if (TrainGameManager.instance.Defence_stat > 0)
        {
            TrainGameManager.instance.Defence_stat -= 1;
            NowStat();
        }
    }


    public void NowStat()
    {
        // int 계산
     //   UsableStatus.text = 계산.ToString();
    }
}
