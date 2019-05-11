using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlayer_Ctrl : MonoBehaviour {

    Animator anim;
    StationPlayer_Actor player;


    float runTime;
    // Use this for initialization

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = new StationPlayer_Actor();
    }




    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetKey();
        //anim.SetFloat("RunTime", runTime);

    }
    void GetKey()
    {

        if (Input.GetKey(KeyCode.A))
        {
            Move('a');
            anim.SetBool("IsWalk", true);
            runTime += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Move('d');
            anim.SetBool("IsWalk", true);
            runTime += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move('s');
            anim.SetBool("IsWalk", true);
            runTime += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Move('w');
            anim.SetBool("IsWalk", true);
            runTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("IsWalk", false);
            runTime = 0;
        }
    }
    public void Move(char key)
    {
        player.Move(key);
        // player.Animate_State(1); // walk로 state바꾸기
        // 아 c#에서는 enum을 어떻게 써야되는지 모르겠네
        // 좀 다른듯. 그래서 지금 있는 순서 idle,walk,jump,run,attack 유지하면서
        // 더 필요한 state들은 뒤로 추가하는걸로
    }

}
