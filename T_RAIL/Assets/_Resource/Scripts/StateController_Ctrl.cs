using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StateController_Ctrl : MonoBehaviour {

    public Text UsableStatus; // 사용할 수 있는 스텟
    
    public void SpeedUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
            GameValue.StatusMAX > TrainGameManager.instance.Speed)
        {
            TrainGameManager.instance.Speed += 1;
            NowStat();    
        }
        else
        {
        }
    }

    public void SpeedDOWN()
    {
        if (TrainGameManager.instance.Speed > 0)
        {
            TrainGameManager.instance.Speed -= 1;
            NowStat();
        }
    }

    public void NoiseUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
           GameValue.StatusMAX < TrainGameManager.instance.Noise)
        {
            TrainGameManager.instance.Noise += 1;
            NowStat();
        }
    }

    public void NoiseDOWN()
    {
        if (TrainGameManager.instance.Noise > 0)
        {
            TrainGameManager.instance.Noise -= 1;
            NowStat();
        }
    }

    public void DefenceUP()
    {
        if (TrainGameManager.instance.NowsumStat < TrainGameManager.instance.AllStat &&
           GameValue.StatusMAX < TrainGameManager.instance.Defence)
        {
            TrainGameManager.instance.Defence += 1;
            NowStat();
        }
    }

    public void DefenceDOWN()
    {
        if (TrainGameManager.instance.Defence > 0)
        {
            TrainGameManager.instance.Defence -= 1;
            NowStat();
        }
    }


    public void NowStat()
    {
        // int 계산
     //   UsableStatus.text = 계산.ToString();
    }
}
