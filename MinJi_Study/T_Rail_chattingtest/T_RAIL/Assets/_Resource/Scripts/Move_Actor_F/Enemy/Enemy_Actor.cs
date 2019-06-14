using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Actor : Move_Actor {

    // Use this for initialization


    public Enemy_Actor()
    {
        base.Actor_Property = (int)Actor.Monster;
        HP = GameValue.enemy1_FullHp;
    }

}
