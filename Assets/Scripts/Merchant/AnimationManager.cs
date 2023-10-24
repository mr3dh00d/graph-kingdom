using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private bool isIdle = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(!isIdle && stateInfo.IsName("Default"))
        {
            animator.SetInteger("direction", 0);
            isIdle = true;
        }
        if(!stateInfo.IsName("Default"))
        {
            isIdle = false;
        }
    }
}
