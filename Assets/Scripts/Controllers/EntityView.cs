using UnityEngine;
using UnityEngine.UIElements;

public class EntityView : MonoBehaviour
{
    protected Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void CharacterMoveAnimation(bool _isMoveAnim)
    {
        anim.SetBool(TagManager.CHARACTER_MOVEMENT_ANIMATION_BOOL, _isMoveAnim);//CharacterAttack1Animation
    }

    public void CharacterAttack1Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ATTACK1_ANIMATION_TRIGGER);
        Debug.Log("Anim Ataque 1");
    }

    public void CharacterAttack2Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ATTACK2_ANIMATION_TRIGGER);
        Debug.Log("Anim Ataque 2");
    }

    public void CharacterAttack3Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ATTACK3_ANIMATION_TRIGGER);
        Debug.Log("Anim Ataque 3");
    }

    public void CharacterAttack4Animation()
    {
        anim.SetTrigger(TagManager.CHARACTER_ATTACK4_ANIMATION_TRIGGER);
        Debug.Log("Anim Ataque 4");
    }

    public void CharacterBlockAnimation(bool value)
    {
        anim.SetBool(TagManager.CHARACTER_BLOCK_ANIMATION_BOOL, value);
    }

    //public void CharacterBlock2Animation()
    //{
    //    anim.SetTrigger(TagManager.CHARACTER_BLOCK2_ANIMATION_TRIGGER);
    //}

    public void CharacterDamageAnimation()
    {
        anim.SetTrigger(TagManager.CHARACTER_DAMAGE_ANIMATION_TRIGGER);
    }

    public void CharacterDeathAnimation()
    {
        anim.SetTrigger(TagManager.CHARACTER_DEATH_ANIMATION_TRIGGER);
    }

}
