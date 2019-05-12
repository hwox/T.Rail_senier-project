using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Actor : Move_Actor {

    public bool WallConflict;
    public int[] Directions;

    public Player_Actor()
    {
        base.Actor_Property = (int)Actor.Player; // property에 player 라고 정의

        //position = new Pos(-1, 3.8f, -2.5f);
        //플레이어 생성 위치 
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                position = new Pos(-1 * i * 2, 3.8f, -2.5f);
            }
        }

        speed = 15.0f; // speed 는 km/h 로 따지나 
        Directions = new int[4];

        for(int i = 0; i < Directions.Length; i++)
        {
            Directions[i] = 1;
        }
       Where_Train = 1;
       Where_Floor = 1; // 처음에는 1층, 1번째칸에 존재하니까 

    }


    // 서버잘한다~~ 화ㅏ이팅 >_<~~

    public override void Move(char key)
    {
       // base.Move(key);

        if (Actor_State == 1)
        {
            //walk일 때만 이동
            switch (key) {
                case 'a':
                    position.x -= 0.15f*speed*Time.deltaTime*Directions[0];
                    rotate.y = -90.0f;
                    Direction = 1;
                    break;
                case 's':
                    position.z -= 0.15f * speed * Time.deltaTime * Directions[1];
                    rotate.y = 180;
                    Direction = 2;
                    break;
                case 'd':
                    position.x += 0.15f * speed * Time.deltaTime * Directions[2];
                    rotate.y = 90.0f;
                    Direction = 3;
                    break;
                case 'w':
                    position.z += 0.15f * speed * Time.deltaTime * Directions[3];
                    rotate.y = 0;
                    Direction = 4;
                    break;

            }
        }


 
    }
    
    public void To_UpStair(float _x)
    {
        // 윗층으로
        position.x = _x;
        position.y += 0.01f * Time.deltaTime * 100.0f;
        rotate.y = 180;
        Direction = 3; // 근데 이거 direction 어따쓰려고 만들어뒀더라
    }

    public void To_DownStair(float _x)
    {
        // 아랫층으로
        // 이건 floor2의 x 받아와야함
        position.x = _x;
       
        position.y -= 0.01f * Time.deltaTime * 100.0f;
        rotate.y = 180;
        Direction = 3; // 근데 이거 direction 어따쓰려고 만들어뒀더라
    }

    public void Animate_State(int _key)
    {
        Actor_State = _key;

    }



    public void On_Floor2_yPosition()
    {
        position.y = GameValue.player_2f_position_y;
    }
    public void On_Floor1_yPosition()
    {
        position.y = GameValue.player_1f_position_y;
    }


    public void Jump_ToNextTrain()
    {
        // 여기서 해야될거 position 증가 
        position.x -= 0.15f * speed * Time.deltaTime;
        rotate.y = -90.0f;
        Direction = 1;


    }


    public void Jump()
    {
        // prev, next bool 변수를 ctrl에서 받아서 prev가 true이면 jump_toprevtrain 호출하고
        // next가 true이면 jump_tonexttrain 호출

        //if (prev)
        //{
        //    Jump_ToPrevTrain();
        //}

        //else if (next)
        //{ 
        //    Jump_ToNextTrain();
        //}
        if (!WallConflict)
        {
            switch (Direction)
            {
                case 1:
                    position.x -= 0.15f * speed * Time.deltaTime * Directions[0]; 
                    rotate.y = -90.0f;
                    break;
                case 2:
                    position.z -= 0.15f * speed * Time.deltaTime * Directions[1]; 
                    rotate.y = 180;
                    break;
                case 3:
                    position.x += 0.15f * speed * Time.deltaTime * Directions[2]; 
                    rotate.y = 90.0f;
                    break;
                case 4:
                    position.z += 0.15f * speed * Time.deltaTime * Directions[3];
                    rotate.y = 0;
                    break;
            }
        }
    }

    public void WallConflictDirection()
    {
        if (!WallConflict)
        {
            switch (Direction)
            {
                case 1:
                    Directions[0] = 0;
                    WallConflict = true;
                    break;
                case 2:
                    Directions[1] = 0;
                    WallConflict = true;
                    break;
                case 3:
                    Directions[2] = 0;
                    WallConflict = true;
                    break;
                case 4:
                    Directions[3] = 0;
                    WallConflict = true;
                    break;
            }
        }
    }
    public void WallConflictDirections_Reset()
    {
        for (int i = 0; i < Directions.Length; i++)
        {
            Directions[i] = 1;
        }
        WallConflict = false;
    }

   public void UpSize()
   {
        size = new Size(2, 2, 2);
   }
   public void DownSize()
   {
       size = new Size(1, 1, 1);
   }
    public void SetStationPlayer(int n)
    {
        position = new Pos(-1 * n * 2, 1.74f, -2.5f);
       
    }
    public void SetTrainPlayer(int n)
    {
        position = new Pos(-1 * n* 2, 3.8f, -2.5f);
    }

}
