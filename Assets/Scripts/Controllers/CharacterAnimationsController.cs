using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimationsController : MonoBehaviour
{
    Animator characterAnimator;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
    }

    public void CharacterMoveAnimation(bool _isMoveAnim)
    {
        characterAnimator.SetBool(TagManager.CHARACTER_MOVEMENT_ANIMATION_BOOL, _isMoveAnim);//CharacterAttack1Animation
    }

    public void CharacterAttack1Animation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_ATTACK1_ANIMATION_TRIGGER);
    }

    public void CharacterAttack2Animation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_ATTACK2_ANIMATION_TRIGGER);
    }

    public void CharacterAttack3Animation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_ATTACK3_ANIMATION_TRIGGER);
    }

    public void CharacterBlock1Animation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_BLOCK1_ANIMATION_TRIGGER);
    }

    public void CharacterBlock2Animation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_BLOCK2_ANIMATION_TRIGGER);
    }

    public void CharacterDamageAnimation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_DAMAGE_ANIMATION_TRIGGER);
    }

    public void CharacterDeathAnimation()
    {
        characterAnimator.SetTrigger(TagManager.CHARACTER_DEATH_ANIMATION_TRIGGER);
    }

}
