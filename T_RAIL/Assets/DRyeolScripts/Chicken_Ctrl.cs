using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken_Ctrl : MonoBehaviour
{

    Animator anim;
    NavMeshAgent nav;
    public GameObject[] dest;
    public GameObject DestPerson;
    Vector3 prePos;
    public int state = 0;
    //state 0= 걷기 ,1= 공격 ,2= 공격당함
    int NextDestNum;
    public int HP=5;


    public void BeatenTrue()
    {
        anim.SetBool("Is Beaten", true);
        nav.speed = 2.5f;
        StopCoroutine("GotoDest");
        StopCoroutine("FindNextDest");
        StartCoroutine("GotoDestPreson");
        StartCoroutine("BeatenFalse");
        HP-=1;
        if(HP<=0)
        {
            StopCoroutine("GotoDestPreson");
            StopCoroutine("BeatenFalse");
            this.gameObject.SetActive(false);
            
        }
    }
    IEnumerator BeatenFalse()//공격받고 다시 맞는 애니메이션 false하기
    {
      
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("Is Beaten", false);
        
    

    }

    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        StartCoroutine("GotoDest");
        StartCoroutine("FindNextDest");
    }

    // Update is called once per frame
    void Update()
    {




    }

    IEnumerator GotoDest()//닭이 무작위로 정해진 위치로 향해 가기
    {
        while (true)
        {
            nav.SetDestination(dest[NextDestNum].transform.position);
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

    IEnumerator FindNextDest()// 무작위 위치 찾기
    {

        NextDestNum = Random.Range(0, dest.Length);
        // Debug.Log(NextDestNum);
        yield return 5;
    }


    IEnumerator GotoDestPreson()// 목표 사람으로 따라가기
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, DestPerson.transform.position) > 3.0f)
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
}
