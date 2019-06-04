using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySoundManager : MonoBehaviour {

    public AudioSource AudioSource;
    public AudioClip AudioClip;

    public void onButtonClick()
    {
        AudioSource.PlayOneShot(AudioClip);
    }
}
