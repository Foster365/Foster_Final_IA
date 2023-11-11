using Newtonsoft.Json.Serialization;
using Unity.Collections.LowLevel.Unsafe;

public class TagManager
{
    #region Animations tags

    public const string CHARACTER_MOVEMENT_ANIMATION_BOOL = "isReadyToMove";

    public const string CHARACTER_ATTACK1_ANIMATION_TRIGGER = "attack1Trigger";
    public const string CHARACTER_ATTACK2_ANIMATION_TRIGGER = "attack2Trigger";
    public const string CHARACTER_ATTACK3_ANIMATION_TRIGGER = "attack3Trigger";

    public const string CHARACTER_ENHANCED_ATTACK1_ANIMATION_TRIGGER = "enhancedAttack1Trigger";
    public const string CHARACTER_ENHANCED_ATTACK2_ANIMATION_TRIGGER = "enhancedAttack2Trigger";
    public const string CHARACTER_ENHANCED_ATTACK3_ANIMATION_TRIGGER = "enhancedAttack3Trigger";

    public const string CHARACTER_DESPERATE_ATTACK1_ANIMATION_TRIGGER = "desperateAttack1Trigger";
    public const string CHARACTER_DESPERATE_ATTACK2_ANIMATION_TRIGGER = "desperateAttack2Trigger";
    public const string CHARACTER_DESPERATE_ATTACK3_ANIMATION_TRIGGER = "desperateAttack3Trigger";

    public const string CHARACTER_BLOCK1_ANIMATION_TRIGGER = "block1Trigger";
    public const string CHARACTER_BLOCK2_ANIMATION_TRIGGER = "block2Trigger";

    public const string CHARACTER_DAMAGE_ANIMATION_TRIGGER = "damageTrigger";
    public const string CHARACTER_DEATH_ANIMATION_TRIGGER = "deathTrigger";

    #endregion

    #region Characters Tags

    public const string LEADER_TAG = "Leader";
    public const string NPC_TAG = "NPC";

    public const string LEADER_A_NAME_TAG = "Leader_A";
    public const string LEADER_B_NAME_TAG = "Leader_B";
    public const string NPC_A_NAME_TAG = "NPC_A";
    public const string NPC_B_NAME_TAG = "NPC_B";

    #endregion

    #region Environment Tags

    public const string WALL_TAG = "Wall";

    #endregion

    #region Leader FSM States Tags
    public const string LEADER_FSM_IDLE_STATE_TAG = "LeaderIdleState";
    public const string LEADER_FSM_PATROL_STATE_TAG = "LeaderPatrolState";
    public const string LEADER_FSM_SEEK_STATE_TAG = "LeaderSeekState";
    public const string LEADER_FSM_ATTACK_STATE_TAG = "LeaderAttackState";
    public const string LEADER_FSM_DAMAGE_STATE_TAG = "LeaderDamageState";
    public const string LEADER_FSM_BLOCK_STATE_TAG = "LeaderBlockState";
    public const string LEADER_FSM_DEATH_STATE_TAG = "LeaderDeathState";
    #endregion

    #region NPC FSM States Tags
    public const string NPC_FSM_IDLE_STATE_TAG = "NPCIdleState";
    public const string NPC_FSM_FOLLOW_LEADER_STATE_TAG = "NPCFollowLeaderTag";
    public const string NPC_FSM_SEEK_STATE_TAG = "NPCSeekStateTag";
    public const string NPC_FSM_ATTACK_STATE_TAG = "NPCAttackStateTag";
    public const string NPC_FSM_DAMAGE_STATE_TAG = "NPCDamageStateTag";
    public const string NPC_FSM_FLEE_STATE_TAG = "NPCFleeStateTag";
    public const string NPC_FSM_DEATH_STATE_TAG = "NPCDeathStateTag";
    #endregion
}
