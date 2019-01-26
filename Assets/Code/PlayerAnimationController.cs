using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages communicating to the animation controller state machine.
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private const string WALKING_BOOL = "isWalking";
    private const string IS_HOLDING_BOOL = "isHoldingPillow";
    private const string PICKUP_TRIGGER = "Pickup";
    private const string THROW_TRIGGER = "Throw";
    private const string PUTDOWN_TRIGGER = "PutDown";

    public Animator anim;

    public void Walk(bool isWalking)
    {
        anim.SetBool(WALKING_BOOL, isWalking);
    }

    public void Pickup()
    {
        anim.SetTrigger(PICKUP_TRIGGER);
        anim.ResetTrigger(THROW_TRIGGER);
        anim.ResetTrigger(PUTDOWN_TRIGGER);
        anim.SetBool(IS_HOLDING_BOOL, true);
    }

    public void PutDown()
    {
        anim.SetTrigger(PUTDOWN_TRIGGER);
        anim.ResetTrigger(PICKUP_TRIGGER);
        anim.SetBool(IS_HOLDING_BOOL, false);

    }

    public void Throw()
    {
        anim.SetTrigger(THROW_TRIGGER);
        anim.ResetTrigger(PICKUP_TRIGGER);
        anim.SetBool(IS_HOLDING_BOOL, false);
    }

}
