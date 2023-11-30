using _Main.Scripts.FSM_SO_VERSION;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : EntityModel
{

    [SerializeField, Header("Attack Roulette Attacks Chances")]
    int attack1Chance;
    [SerializeField] int attack2Chance;
    [SerializeField] int attack3Chance;
    [SerializeField] int attack4Chance;

    //Components variables
    Rigidbody rb;
    Grid mapGrid;
    HealthController healthController;
    //LeaderAIController charAIController;
    //

    public Vector3 dirToMove = Vector3.zero;
    Transform target;
    #region Encapsulated Variables

    public Transform Target { get => target; set => target = value; }
    public Grid MapGrid { get => mapGrid; set => mapGrid = value; }
    public HealthController HealthController { get => healthController; set => healthController = value; }
    public int Attack1Chance { get => attack1Chance; set => attack1Chance = value; }
    public int Attack2Chance { get => attack2Chance; set => attack2Chance = value; }
    public int Attack3Chance { get => attack3Chance; set => attack3Chance = value; }
    public int Attack4Chance { get => attack4Chance; set => attack4Chance = value; }
    #endregion

    #region Unity Engine Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        View = GetComponent<EntityView>();
        HealthController = new HealthController(Data.MaxHealth);
        //healthController.OnDie += DeathHandler;
    }

    private void Start()
    {
        SetCharacterTag();
    }

    #endregion

    public void DeathHandler()
    {
        healthController.IsDead = true;
        //View.CharacterDeathAnimation();
        Destroy(gameObject, 1f);
    }

    void SetCharacterTag()
    {
        if (gameObject.tag == TagManager.LEADER_TAG) HealthController.IsLeader = true;
        else if (gameObject.tag == TagManager.NPC_TAG) HealthController.IsNPC = true;
    }

    public void SpawnMagicProjectile(GameObject go)
    {
        Instantiate(go, leftHandCollider.transform.position, Quaternion.identity);
    }

    public override StateData[] GetStates() => CharAIData.FsmStates;

    public override EntityData GetData() => Data;
    public override Vector3 GetFoward() => transform.forward;

    public override float GetSpeed() => rb.velocity.magnitude;
    public override void Move(Vector3 direction)
    {
        direction.y = 0;
        //direction += _obstacleAvoidance.GetDir() * multiplier;
        rb.velocity = direction * (Data.MovementSpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, direction, Data.RotationSpeed * Time.deltaTime);
    }

    public override void LookDir(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        direction.y = 0;
        StartCoroutine(LookAtTarget(direction));
        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * Data.RotationSpeed);
    }

    public IEnumerator LookAtTarget(Vector3 _targetPos)
    {
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - _targetPos);
        float timer = 0;
        while (timer < .5f)
        {
            Quaternion.Slerp(transform.rotation, lookRotation, timer);

            timer += Time.deltaTime * Data.RotationSpeed;
            yield return null;
        }
    }

    public override Rigidbody GetRigidbody() => rb;

    public override EntityModel GetModel() => this;

    public EntityModel GetTarget()
    {
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, dirToMove);
    }

}
