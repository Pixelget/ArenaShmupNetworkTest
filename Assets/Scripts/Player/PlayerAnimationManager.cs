using UnityEngine;
using System.Collections;

//public enum PlayerAnimationState { Idle, Moving, Boost, Hit, Ability }
public class PlayerAnimationManager : MonoBehaviour {
    public Animator frameAnimator;
    
    public void InitializeFrame(RuntimeAnimatorController animController) {
        frameAnimator.runtimeAnimatorController = animController;
    }
    
    public void Moving(bool moving) {
        frameAnimator.SetBool("isMoving", moving);
    }

    public void Boosting(bool boosting) {
        frameAnimator.SetBool("isBoosting", boosting);
    }
    
    public void GetHit() {
        frameAnimator.SetTrigger("gotHit");
    }
}


/* Animations Needed for a Mech */
/*
    Idle
    Moving
    Getting Hit
    Using Ability
    Boosting
    Mech Destruction? [per mech or 1 global one?]
*/

/* Other Animations Needed */
/*
    Mech Engine
    Gun Muzzle Flash
    Explosions
*/
