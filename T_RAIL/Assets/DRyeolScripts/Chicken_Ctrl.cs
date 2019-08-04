using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Chicken_Ctrl : MonoBehaviourPunCallbacks
{

    Animator anim;
    NavMeshAgent nav;
   
    public GameObject DestPerson;
    GameObject SpwanManager;
    Vector3 prePos;
    public bool live = true;
    //state 0= 걷기 ,1= 공격 ,2= 공격당함
    int NextDestNum;
    public int HP=5;

    public GameObject particle;

    public void BeatenTrue()
    {
        Invoke("beatenTrueInvoke", 0.1f); 
           
    }

    void beatenTrueInvoke()
    {
        GameObject temp = Instantiate(particle, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        temp.transform.parent = this.transform;

        anim.SetBool("Is Beaten", true);
        nav.speed = 2.5f;
        StopCoroutine("GotoDest");
        StopCoroutine("FindNextDest");
        StartCoroutine("GotoDestPreson");
        StartCoroutine("BeatenFalse");
        HP -= 1;
        if (HP <= 0)
            photonView.RPC("chickenDeath_RPC", RpcTarget.All);
    }


    [PunRPC]
    void chickenDeath_RPC()
    {
        StartCoroutine("Death");
    }

    private void Awake()
    {
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
        this.transform.parent = TrainGameManager.instance.gameObject.transform.GetChild(6);// (int)prefab_list.passenger);
        TrainGameManager.instance.ChickenManager.Add(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        SpwanManager = GameObject.Find("ChickenManager");
        // Debug.Log(SpwanManager.gameObject.transform.position.z);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("GotoDest");
            StartCoroutine("FindNextDest");
        }
    }

    IEnumerator FindNextDest()// 무작위 위치 찾기
    {
        NextDestNum = Random.Range(0, 10);
        // Debug.Log(NextDestNum);
        yield return 5;
    }

    IEnumerator GotoDest()//닭이 무작위로 정해진 위치로 향해 가기
    {
        GameObject[] Cdest = (GameObject[])SpwanManager.gameObject.GetComponent<ChickenManager>().dest.Clone();
        nav.speed = 1.0f;
        while (true)
        {
            nav.SetDestination(Cdest[NextDestNum].transform.position);
            if (Vector3.Distance(transform.position, prePos) > 0.01f)
            {
                // anim.SetBool("Is Beaten", false);
            }
            else
            {

                StartCoroutine("FindNextDest");
                // anim.SetBool("IsSearch", true);

            }
            prePos = transform.position;
            yield return new WaitForSeconds(0.01f);
        }


    }


    IEnumerator GotoDestPreson()// 목표 사람으로 따라가기
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, DestPerson.transform.position) > 2.8f)
            {
                nav.speed = 2.5f;
                nav.SetDestination(DestPerson.transform.position);
            }
            else
            {
                nav.speed =0f;
            }
            yield return new WaitForSeconds(0.01f);

        }
        
    }

    IEnumerator BeatenFalse()//공격받고 다시 맞는 애니메이션 false하기
    {

        yield return new WaitForSeconds(0.2f);

        anim.SetBool("Is Beaten", false);

    }



    public void die()
    {
        photonView.RPC("chickenDie_RPC", RpcTarget.All);
    }

    [PunRPC]
    void chickenDie_RPC()
    {
        StopCoroutine("GotoDestPreson");
        StopCoroutine("BeatenFalse");
        this.gameObject.SetActive(false);
    }



    IEnumerator Death()//죽기
    {
        live = false;
        
        StopCoroutine("GotoDestPreson");
        StopCoroutine("BeatenFalse");
        anim.SetBool("Is Beaten", false);
        Debug.Log("죽음");
        anim.SetBool("Is Death", true);
        yield return new WaitForSeconds(2.7f);
        StartCoroutine("CoinParticle");
        TrainGameManager.instance.SoundManager.coin_Sound_Play();

        GameObject egg = TrainGameManager.instance.GetObject(7);
        egg.SetActive(true);
        egg.transform.position = this.gameObject.transform.position;
        this.gameObject.SetActive(false);

    }

    IEnumerator CoinParticle()
    {
        Debug.Log("들어옴");
        GameObject Cp = TrainGameManager.instance.GetObject(8);
        Cp.SetActive(true);
        Cp.transform.position = this.gameObject.transform.position;
        TrainGameManager.instance.GetComponent<PhotonView>().RPC("getCoin_RPC", RpcTarget.All,10);
        //TrainGameManager.instance.CoinNum += 10;
        yield return new WaitForSeconds(2.5f);
        Cp.SetActive(false);
    }
}
