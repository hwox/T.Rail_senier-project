using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Ctrl : MonoBehaviour
{

    // enemy1 은 ㄷ기차가 달릴 때 뒤에서 따라오는 애들 

    Camera MCam;
    CamCtrl MCam_Ctrl;

    Enemy_Actor enemy;
    [SerializeField]
    Transform tr;
    Animator anim;

    public int E_damage;

    Vector3 Position_Set_Destination;
    Vector3 Position_Set_Move;
    bool Position_Set_Go = false;

    public Transform Rhino_child; // 아니 이거 fbx가 이렇게 안잡으면 제대로 안움직임

    bool Retreat; // 후퇴

    Vector3 Init_Rhino_child;
    Vector3 Init_Rhino;

    int follow_index; // 따라갈 기차의 인덱스 

    private void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        StartCoroutine(Enemy_ActRoutine());
        enemy = new Enemy_Actor();
    }
    // Use this for initialization
    void Start()
    {
        Init_Rhino = tr.position;
        Init_Rhino_child = Rhino_child.position;

        enemy.speed = 10.0f;  // enemy1은 스피드 기본고정
        enemy.Damage = E_damage;
        anim.SetBool("IsRun", true);


        MCam = Camera.main;
        MCam_Ctrl = MCam.GetComponent<CamCtrl>();


        TrainGameManager.instance.ConditionCtrl.enemy1 = this.gameObject;
        TrainGameManager.instance.ConditionCtrl.enemy1_ctrl = this.GetComponent<Enemy1_Ctrl>();
        this.gameObject.SetActive(false);



    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(GameValue.bullet_layer))
        {
            // 총알맞으면
            GameObject parti = TrainGameManager.instance.GetObject(3); // dust 
            parti.transform.position = other.gameObject.transform.position;
            parti.SetActive(true);
            parti.transform.GetChild(0).gameObject.SetActive(true);
            parti.transform.GetChild(0).GetComponent<ParticleSystem>().Play(true);
            other.gameObject.SetActive(false);
           
            
            Debug.Log("맞");
            MCam_Ctrl.Hit_EnemyAppearCam();



            enemy.HP -= 5;
        }

        if (other.gameObject.layer.Equals(GameValue.train_layer))
        {
            // 잠깐 뒤로 물러나기
            //iTween.ShakePosition(other.gameObject, iTween.Hash("time", 0.5f, "x", -2.0f));
            MCam_Ctrl.Hit_EnemyCam(true);
            Debug.Log("기차랑 충돌");
        }
    }

    void Update()
    {
        if (Position_Set_Go)
        {
            Position_Set();
        }
        //else
        //{
        //    tr.position = Vector3.Slerp(tr.position, Position_Set_Move, Time.deltaTime);
        //}

       

    }
    public void Enemy1_On()
    {
        anim.SetBool("IsRun", true);
        follow_index = TrainGameManager.instance.trainindex;
        Position_Set_Destination = new Vector3((GameValue.Train_distance * (follow_index-1) -15), tr.position.y, tr.position.z);
        Position_Set_Go = true;

        StartCoroutine(Enemy_ActRoutine());
        TrainGameManager.instance.Notice_EnemyAppear();
    }

    void Position_Set()
    {

        tr.LookAt(TrainGameManager.instance.TrainCtrl.train[follow_index - 1].transform);
        tr.position = Vector3.Slerp(tr.position, Position_Set_Destination, Time.deltaTime * 5.0f);

        // 여기서 z값도 좀 왔다갔다 바꾸고
        // Lookat

        if (tr.position.x == Position_Set_Destination.x)
        {
            Position_Set_Go = false;
        }
        

    }


    IEnumerator Enemy_ActRoutine()
    {

        // 몬스터는 기차 collider의 뒤까지만 달려오는거임 그래서 공격할때만 받게 
        // 그리고 monster 나와있는 와중에는 train add안되게 막아놔야 됨


        while (true)
        {

            if (!Position_Set_Go)
            {
                if (!Retreat)
                {
                    // 공격
                 //   if (enemy.HP < 0)
               //     {
                        Retreat = true;
                        // 피가 일정 아래로 내려가서 후퇴면
                 //   }
                }
                // Rhino_child.position -= new Vector3(0, 0, 0.3f);
                //Position_Set_Move = new Vector3(tr.position.x + 5 * Time.deltaTime, tr.position.y, tr.position.z);
            }
    
            yield return new WaitForSeconds(4.0f);
        }
    }

    private void OnDisable()
    {
        tr.position = Init_Rhino;
        Rhino_child.position = Init_Rhino_child;
        enemy.HP = GameValue.enemy1_FullHp; // 피 다시 원래대로 돌려놓기
        Retreat = true;
    }

}
