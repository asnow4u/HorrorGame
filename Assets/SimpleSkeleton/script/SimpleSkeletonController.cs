using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSkeletonController : MonoBehaviour
{
    Animator animator;
    SkinnedMeshRenderer skinnedMesh;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
            animator.SetTrigger("lying");
        else if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("jump");
        else if (Input.GetKeyDown(KeyCode.K))
            animator.SetTrigger("knockdown");
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetTrigger("punch_L");
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            animator.SetTrigger("punch_R");

        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        if(Input.GetKey(KeyCode.LeftShift))
            animator.SetBool("running", true);
        else
            animator.SetBool("running", false);
        if (Input.GetKey(KeyCode.LeftControl))
            animator.SetBool("sidefix", true);
        else
            animator.SetBool("sidefix", false);
    }
}
