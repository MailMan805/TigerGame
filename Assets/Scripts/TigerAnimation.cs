using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerAnimation : MonoBehaviour
{
    TigerAI tiger;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        tiger = GetComponent<TigerAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tiger.currentState == TigerAI.TigerState.Idle)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
        }
        else if (tiger.currentState == TigerAI.TigerState.HuntingSearching)
        {
            animator.SetBool("isSearching", true);
            animator.SetBool("isIdle", false);
        }
        else if (tiger.currentState == TigerAI.TigerState.Prowling)
        {
            animator.SetBool("isProwling", true);
            animator.SetBool("isSearching", false);
        }
        else if (tiger.currentState == TigerAI.TigerState.Stalking)
        {
            animator.SetBool("isStalking", true);
            animator.SetBool("isSearching", false);
        }
        else if (tiger.currentState == TigerAI.TigerState.Evacuating)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isStalking", false);
            animator.SetBool("isProwling", false);
        } 
        else if (tiger.currentState == TigerAI.TigerState.Chase)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isStalking", false);
            animator.SetBool("isProwling", false);
        }

        

        

        

       

        
    }
}
