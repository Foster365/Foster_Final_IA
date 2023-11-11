using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderView : EntityView
{
   
    public void EnhancedAttack1Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ENHANCED_ATTACK1_ANIMATION_TRIGGER);
    }
    public void EnhancedAttack2Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ENHANCED_ATTACK1_ANIMATION_TRIGGER);
    }
    public void EnhancedAttack3Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ENHANCED_ATTACK1_ANIMATION_TRIGGER);
    }
}
