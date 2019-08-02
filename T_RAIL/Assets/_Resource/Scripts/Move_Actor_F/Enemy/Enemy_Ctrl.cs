using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy_Ctrl : MonoBehaviourPunCallbacks
{
    Enemy_Actor enemy;
    Transform tr;
    Animator anim;


    Vector3 Position_Set_Destination;
    Vector3 Position_Set_Move;
    bool Position_Set_Go = false;

    public int KindOfEnemy; // 1_ 코뿔소 / 2_선인장 / 3_허스키 

    public Transform Rhino_child; // 아니 이거 fbx가 이렇게 안잡으면 제대로 안움직임
    Vector3 Init_Rhino_child;
    Vector3 Init_Rhino;

    public Transform Cactus_child;
    Vector3 Init_Cactus_child;
    Vector3 Init_Cactus;
    public GameObject Cactus_Punch;

    public Transform Husky_child;
    Vector3 Init_Husky_child;
    Vector3 Init_Husky;

    int follow_index; // 따라갈 기차의 인덱스 
    bool Retreat; // 후퇴

  //  public Condition_Ctrl condition_ctrl;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();

        enemy = new Enemy_Actor();

        DontDestroyOnLoad(gameObject);

    }
    // Use this for initialization
    void Start()
    {
      //  condition_ctrl = GameObject.Find("/State_Ctrl/Condition_Ctrl").GetComponent<Condition_Ctrl>();

        switch (KindOfEnemy)
        {
            case 1:
                RhinoInitSetting();
              //  condition_ctrl.enemyOnStage1 = this.gameObject;
                break;
            case 2:
                CactusInitSetting();
              //  condition_ctrl.enemyOnStage2 = this.gameObject;
                break;
            case 3:
                HuskyInitSetting();
            //    condition_ctrl.enemyOnStage3 = this.gameObject;
                break;
            default:
                TrainGameManager.instance.Error_print();
                break;
        }

       // StartCoroutine(Enemy_ActRoutine());
    //    TrainGameManager.instance.ConditionCtrl.NowEnemy = this.gameObject;
    //    TrainGameManager.instance.ConditionCtrl.enemy_ctrl = this.GetComponent<Enemy_Ctrl>();
        this.gameObject.SetActive(false);

        enemy.speed = 10.0f;  // enemy1은 스피드 기본고정
        anim.SetBool("IsRun", true);
        // StartCoroutine(Enemy_ActRoutine());
    }

    void RhinoInitSetting()
    {

        Init_Rhino = tr.position;
       // Init_Rhino = new Vector3(-200, 1.7f, -3.6f);
        Init_Rhino_child = Rhino_child.position;

        tr.position = new Vector3(-200, 1.7f, -3.6f);

        this.gameObject.SetActive(false);

    }
    void CactusInitSetting()
    {
       Init_Cactus = tr.position;
        //  Init_Cactus = new Vector3(-200, 1.7f, -3.6f);
        tr.position = new Vector3(-200, 1.7f, -3.6f);
        Init_Cactus_child = Cactus_child.position;
    }

    void HuskyInitSetting()
    {
       //Init_Husky = new Vector3(-200, 1.7f, -3.6f);
       Init_Husky = tr.position;
        tr.position = new Vector3(-200, 1.7f, -3.6f);
        Init_Husky_child = Husky_child.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(GameValue.bullet_layer))
        {
            //총알맞으면
            GameObject parti = TrainGameManager.instance.GetObject((int)GameValue.prefab_list.dustparticle); // dust 
            parti.transform.position = other.gameObject.transform.position;
            parti.SetActive(true);
            parti.transform.GetChild(0).gameObject.SetActive(true);
            parti.transform.GetChild(0).GetComponent<ParticleSystem>().Play(true);
            other.gameObject.SetActive(false);

            photonView.RPC("isAttackedByBullet", RpcTarget.All);
            TrainGameManager.instance.SoundManager.enemy_Sound_Play();
        }
    }

    [PunRPC]
    public void isAttackedByBullet()
    {
        enemy.HP -= 30 / TrainGameManager.instance.Defence_stat;
    }

    void Update()
    {
        //switch (KindOfEnemy)
        //{
        //    case 1:
        //        Rhino_Update();
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //    default:
        //        TrainGameManager.instance.Error_print();
        //        break;
        //}
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

        if (!Retreat)
        {
            if (enemy.HP < 0)
            {
                Enemy_Retreat();
                Retreat = true;
            }
        }
        else if (Retreat)
        {

            tr.Translate(0, 0, -10.0f * Time.deltaTime);
        }
    }



    public void Enemy_On()
    {
        follow_index = TrainGameManager.instance.trainindex;
        Debug.Log("follow_index" + follow_index);
        Position_Set_Destination = new Vector3((GameValue.Train_distance * (follow_index - 1) - 18), tr.position.y, tr.position.z);
        Position_Set_Go = true;
        Retreat = false;
        StartCoroutine(Enemy_ActRoutine());

        switch (KindOfEnemy)
        {
            case 1:
                TrainGameManager.instance.Notice_Someting("코뿔소 등장!");
                break;
            case 2:
                TrainGameManager.instance.Notice_Someting("선인장 등장!");
                break;
            case 3:
                TrainGameManager.instance.Notice_Someting("허스키 등장!");
                break;
            default:
                TrainGameManager.instance.Error_print();
                break;
        }
    }

    public void Enemy_Retreat()
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
            if (tr.position.x + 0.5f >= Position_Set_Destination.x)
            {
                Position_Set_Go = false;
            }
        }

        yield return new WaitForSeconds(3.0f);

        StartCoroutine(Enemy_ActRoutine());

    }

   

    public void EnemyActiveOff()
    {
        switch (KindOfEnemy)
        {
            case (int)GameValue.EnemyCategory.Rhino:
                tr.position = Init_Rhino;
                Rhino_child.position = Init_Rhino_child;
                break;
            case (int)GameValue.EnemyCategory.Cactus:
                tr.position = Init_Cactus;
                Cactus_child.position = Init_Cactus_child;
                break;
            case (int)GameValue.EnemyCategory.Husky:
                tr.position = Init_Husky;
               Husky_child.position = Init_Husky_child;
                break;
            default:
                break;
        }

        StopCoroutine(Enemy_ActRoutine());
        anim.SetBool("IsAttack", false);
        enemy.HP = GameValue.enemy_FullHp; // 피 다시 원래대로 돌려놓기
        Retreat = false; // 이거 어차피 Hp= 0 에서 하는데 또해?
        TrainGameManager.instance.EnemyAppear = false;
        CancelInvoke("EnemyActiveOff");
        TrainGameManager.instance.ConditionCtrl.TrainAddCondition_Enemy();

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
