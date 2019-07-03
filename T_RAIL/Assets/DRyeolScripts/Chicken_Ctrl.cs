using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken_Ctrl : MonoBehaviour {

    Animator anim;
    NavMeshAgent nav;
    public GameObject[] dest;
    Vector3 prePos;


    int NextDestNum;

    // Use this for initialization
    void Start () {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        StartCoroutine("GotoDest");
        StartCoroutine("FindNextDest");
    }
	
	// Update is called once per frame
	void Update () {


       
     
     
    }

    IEnumerator GotoDest()
    {
        while (true)
        {
            nav.SetDestination(dest[NextDestNum].transform.position);
            if (Vector3.Distance(transform.position, prePos) > 0.01f)
            {
                //anim.SetBool("IsSearch", false);
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

    IEnumerator FindNextDest()
    {
        
        NextDestNum = Random.Range(0, dest.Length);
        Debug.Log(NextDestNum);
        yield return 5;
    }

}
