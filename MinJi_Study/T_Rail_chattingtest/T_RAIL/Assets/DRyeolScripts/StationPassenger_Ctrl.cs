﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPassenger_Ctrl : MonoBehaviour {


  

    Animator anim;

    // Use this for initialization
    void Start () {
        
        anim = GetComponent<Animator>();
        // anim.SetBool("IsSit", true);
        anim.SetBool("IsSit", true);

    }
	
    
    
}

