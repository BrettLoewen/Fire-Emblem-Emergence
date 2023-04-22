//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerAnimator : MonoBehaviour
//{
//    [HideInInspector] public PlayerManager playerManager;

//    public float speedSmoothTime;

//    public Animator animator;

//    // Start is called before the first frame update
//    void Start()
//    {
//        animator = GetComponent<Animator>();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void SetSpeedPercent(float currentSpeed, float walkSpeed, float runSpeed, bool isSprinting)
//    {
//        float speedPercent = ((isSprinting) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
//        animator.SetFloat("speedPercent", speedPercent, speedSmoothTime, Time.deltaTime);
//    }
//}
