using System;
using System.Collections;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private bool animationEnded= false;
    
    public void AnimationEnded()
    {
        animationEnded = true;
    }
    public IEnumerator WaitForAnimationEnd(Animator animator , string state, GameObject instance)
    {
        animationEnded = false;
        while(animator.GetCurrentAnimatorStateInfo(0).IsName(state) && instance != null)
        {
            yield return null;
        }
        animationEnded = true;
        yield return new WaitUntil(() => animationEnded == true);
    }
    
}
