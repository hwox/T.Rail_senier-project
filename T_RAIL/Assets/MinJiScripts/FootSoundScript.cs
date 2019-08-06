using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSoundScript : MonoBehaviour {

    void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Station"))
        {
            TrainGameManager.instance.SoundManager.Player_foot_Sound_Play();

        }
    }
}
