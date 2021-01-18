using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAnimationTags
{

    public const string FOLLOWER_MOVEMENT = "FollowerMoving";
    public const string FOLLOWER_IDLE = "FollowerIdle";
    public const string FOLLOWER_SEEK = "FollowerSeek";

    public const string FOLLOWER_APUNCH = "FollowerPunch";
    public const string FOLLOWER_KICK = "FollowerKick";
    public const string FOLLOWER_BLOCK = "FollowerBlock";

    public const string FOLLOWER_DAMAGED = "FollowerDamaged";
    public const string FOLLOWER_DEATH = "FollowerDeath";

}

public class CharacterTags
{

    public const string FOLLOWER_TAG = "Follower";
    public const string LEADER_TAG = "Leader";

    public const string LEFT_ARM_TAG = "LeftArm";
    public const string RIGHT_ARM_TAG = "RightArm";
    public const string LEFT_FOOT_TAG = "LeftFoot";
    public const string RIGHT_FOOT_TAG = "RightFoot";

}

public class UtilitiesTags
{

    public const string GROUND_TAG = "Ground";
    public const string MAIN_CAMERA_TAG = "MainCamera";
    public const string HEALTH_UI_TAG = "HealthUI";
    //public const string BOSS_HEALTH_UI_TAG = "HealthUIBoss";
    public const string UNTAGGED_TAG = "Untagged";

}

public class LeaderAnimationTags
{

    public const string LEADER_MOVEMENT = "LeaderMoving";
    public const string LEADER_SEEK = "LeaderSeek";

    public const string LEADER_PUNCH = "LeaderPunch";
    public const string LEADER_KICK = "LeaderKick";
    public const string LEADER_BLOCK = "LeaderBlock";

    public const string LEADER_IDLE = "LeaderIdle";
    public const string LEADER_DAMAGED = "LeaderDamaged";
    public const string LEADER_DEATH = "LeaderDeath";
}