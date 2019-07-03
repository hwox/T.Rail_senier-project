using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlayer_Actor : Move_Actor
{

    public int[] Directions;
    public StationPlayer_Actor()
    {
        base.Actor_Property = (int)Actor.Player;
        speed = 15.0f;
        Directions = new int[4];

        for (int i = 0; i < Directions.Length; i++)
        {
            Directions[i] = 1;
        }
    }

    public override void Move(char key)
    {
        //walk일 때만 이동
        switch (key)
        {
            case 'a':
                position.x -= 0.1f * speed * Time.deltaTime * Directions[0];
                rotate.y = -90.0f;
                Direction = 1;
                break;
            case 's':
                position.z -= 0.1f * speed * Time.deltaTime * Directions[1];
                rotate.y = 180;
                Direction = 2;
                break;
            case 'd':
                position.x += 0.1f * speed * Time.deltaTime * Directions[2];
                rotate.y = 90.0f;
                Direction = 3;
                break;
            case 'w':
                position.z += 0.1f * speed * Time.deltaTime * Directions[3];
                rotate.y = 0;
                Direction = 4;
                break;

        }

    }

    public void Animate_State(int _key)
    {
        Actor_State = _key;

    }
}
