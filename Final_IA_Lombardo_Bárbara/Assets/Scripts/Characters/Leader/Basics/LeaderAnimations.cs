using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderAnimations : MonoBehaviour
{

    Animator _animator;

    void Awake()
    {

        _animator = GetComponent<Animator>();

    }

    public void IdleAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_IDLE);
    }

    public void MoveAnimation(bool moving)
    {
        _animator.SetBool(LeaderAnimationTags.LEADER_MOVEMENT, moving);
    }

    public void SeekAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_SEEK);
    }

    public void PunchAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_PUNCH);
    }

    public void KickAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_KICK);
    }

    public void BlockAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_BLOCK);
    }

    public void DamageAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_DAMAGED);
    }

    public void DeathAnimation()
    {
        _animator.SetTrigger(LeaderAnimationTags.LEADER_DEATH);
    }
}
