using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Ctrl : MonoBehaviour
{
    Enemy_Actor enemy;
    Transform tr;
    Animator anim;


    public int E_damage;


    Vector3 Position_Set_Destination;
    Vector3 Position_Set_Move;
    bool Position_Set_Go = false;



    public Transform Rhino_child; // 아니 이거 fbx가 이렇게 안잡으면 제대로 안움직임
    Vector3 Init_Rhino_child;
    Vector3 Init_Rhino;

    int follow_index; // 따라갈 기차의 인덱스 
    bool Retreat; // 후퇴


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


        TrainGameManager.instance.ConditionCtrl.enemy1 = this.gameObject;
        TrainGameManager.instance.ConditionCtrl.enemy1_ctrl = this.GetComponent<Enemy1_Ctrl>();
        this.gameObject.SetActive(false);

       // StartCoroutine(Enemy_ActRoutine());

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

            enemy.HP -= 20;
            TrainGameManager.instance.SoundManager.enemy_Sound_Play();
        }


    }

    void Update()
    {
        if (Position_Set_Go)
        {
            Position_Set();
        }
        else
        {
            if (anim.GetBool("IsAttack"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                {
                    anim.SetBool("IsAttack", false);
                }
            }
        }


        if (!Retreat) {
            if (enemy.HP < 0)
            {
                Enemy1_Retreat();
                Retreat = true;
            }
        }
        else if (Retreat)
        {
            
            tr.Translate(0, 0, -10.0f * Time.deltaTime);
        }

    }
    public void Enemy1_On()
    {
        follow_index = TrainGameManager.instance.trainindex;
        Position_Set_Destination = new Vector3((GameValue.Train_distance * (follow_index - 1) - 20), tr.position.y, tr.position.z);
        Position_Set_Go = true;
        Retreat = false;
        StartCoroutine(Enemy_ActRoutine());
        TrainGameManager.instance.Notice_EnemyAppear();
    }

    public void Enemy1_Retreat()
    {
        // 후퇴할때
        
        anim.SetBool("IsAttack", false);
        Position_Set_Go = false;
        Invoke("EnemyActiveOff", 2.5f);
        // 일단 애니메이션 다 끄고
    }
    
    void Position_Set()
    {

        tr.LookAt(TrainGameManager.instance.TrainCtrl.train[follow_index - 1].transform);
        tr.position = Vector3.Slerp(tr.position, Position_Set_Destination, Time.deltaTime * 0.5f /** 5.0f*/);
    }


    IEnumerator Enemy_ActRoutine()
    {

        // 몬스터는 기차 collider의 뒤까지만 달려오는거임 그래서 공격할때만 받게 
        // 그리고 monster 나와있는 와중에는 train add안되게 막아놔야 됨


        if (!Position_Set_Go)
        {
            if (!Retreat)
            {
                anim.SetBool("IsAttack", true);
            }
            else if (Retreat)
            {
                // 후퇴해 
                anim.SetBool("IsRun", true);
            }
        }
        else
        {
            if (tr.position.x + 0.5f>= Position_Set_Destination.x)
            {
                Position_Set_Go = false;
            }
        }
        yield return new WaitForSeconds(3.0f);

        StartCoroutine(Enemy_ActRoutine());

    }

    void EnemyActiveOff()
    {
        
        StopCoroutine(Enemy_ActRoutine());
        tr.position = Init_Rhino;
        Rhino_child.position = Init_Rhino_child;
        enemy.HP = GameValue.enemy1_FullHp; // 피 다시 원래대로 돌려놓기
        Retreat = false; // 이거 어차피 Hp= 0 에서 하는데 또해?
        TrainGameManager.instance.EnemyAppear = false;
        CancelInvoke("EnemuActiveOff");

        this.gameObject.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    StopCoroutine(Enemy_ActRoutine());
    //    tr.position = Init_Rhino;
    //    Rhino_child.position = Init_Rhino_child;
    //    enemy.HP = GameValue.enemy1_FullHp; // 피 다시 원래대로 돌려놓기
    //    Retreat = true;
    //    TrainGameManager.instance.EnemyAppear = false;

    //}

}
