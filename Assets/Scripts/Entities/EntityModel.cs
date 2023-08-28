using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

public abstract class EntityModel : MonoBehaviour
{
    public Rigidbody _Rb { get; set; }

    public GameObject leftHandCollider;
    public GameObject rightHandCollider;
    public GameObject leftFootCollider;
    public GameObject rightFootCollider;

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material material;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    EntityView view;
    [SerializeField] EntityData data;
    [SerializeField] CharacterAIData charAIData;

    #region FSM variables
    bool isIdle;
    bool isPatrolling;
    bool isSeeingTarget;
    bool isChasing;
    bool isAttacking;
    bool isSearching;
    bool isAllert;

    bool isWalking;
    bool isFleeing;
    bool isBlocking;
    bool isSpecialAttacking;
    bool isAttackDone;
    int regularAttackHealthThreshold;
    int enhancedAttackHealthThreshold;
    #endregion

    #region Encapsulated variables
    public bool IsIdle { get => isIdle; set => isIdle = value; }
    public bool IsPatrolling { get => isPatrolling; set => isPatrolling = value; }
    public bool IsSeeingTarget { get => isSeeingTarget; set => isSeeingTarget = value; }
    public bool IsChasing { get => isChasing; set => isChasing = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsSearching { get => isSearching; set => isSearching = value; }
    public bool IsAllert { get => isAllert; set => isAllert = value; }
    public bool IsWalking { get => isWalking; set => isWalking = value; }
    public bool IsSpecialAttacking { get => isSpecialAttacking; set => isSpecialAttacking = value; }
    public bool IsFleeing { get => isFleeing; set => isFleeing = value; }
    public bool IsBlocking { get => isBlocking; set => isBlocking = value; }
    public EntityView View { get => view; set => view = value; }
    public EntityData Data { get => data; set => data = value; }
    public bool IsAttackDone { get => isAttackDone; set => isAttackDone = value; }
    public int RegularAttackHealthThreshold { get => regularAttackHealthThreshold; set => regularAttackHealthThreshold = value; }
    public int EnhancedAttackHealthThreshold { get => enhancedAttackHealthThreshold; set => enhancedAttackHealthThreshold = value; }
    public CharacterAIData CharAIData { get => charAIData; set => charAIData = value; }
    #endregion

    #region Attack Colliders Activation/Deactivation
    public virtual void TurnOnLeftHandCollider()
    {
        leftHandCollider.SetActive(true);
    }
    public virtual void TurnOffLeftHandCollider()
    {
        leftHandCollider.SetActive(false);
    }
    public virtual void TurnOnRightHandCollider()
    {
        rightHandCollider.SetActive(true);
    }
    public virtual void TurnOffRightHandCollider()
    {
        rightHandCollider.SetActive(false);
    }
    public virtual void TurnOnLeftFootCollider()
    {
        leftFootCollider.SetActive(true);
    }
    public virtual void TurnOffLeftFootCollider()
    {
        leftFootCollider.SetActive(false);
    }
    public virtual void TurnOnRightFootCollider()
    {
        rightFootCollider.SetActive(true);
    }
    public virtual void TurnOffRightFootCollider()
    {
        rightFootCollider.SetActive(false);
    }
    #endregion
    public abstract Rigidbody GetRigidbody();
    public abstract EntityModel GetModel();
    public abstract EntityData GetData();
    public abstract StateData[] GetStates();
    public abstract Vector3 GetFoward();
    public abstract float GetSpeed();
    public abstract void Move(Vector3 direction);
    public abstract void LookDir(Vector3 direction);

    #region Damage_Coroutine
    public void StartingRoutine()
    {
        StartCoroutine(DamageFlash());
    }

    public IEnumerator DamageFlash()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = redMaterial;
            yield return new WaitForSeconds(0.1f);
            meshRenderer.material = material;
        }
        else
        {
            Debug.Log("mesh renderer null");
        }

    }
    #endregion
}
