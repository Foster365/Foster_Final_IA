using _Main.Scripts.FSM_SO_VERSION;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : EntityModel
{
    //Components variables
    Rigidbody rb;
    Grid mapGrid;
    HealthController healthController;
    //LeaderAIController charAIController;
    //

    Transform target;
    #region Encapsulated Variables

    public Transform Target { get => target; set => target = value; }
    public Grid MapGrid { get => mapGrid; set => mapGrid = value; }
    public HealthController HealthController { get => healthController; set => healthController = value; }
    #endregion

    #region Unity Engine Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mapGrid = GameObject.Find("Grid").GetComponent<Grid>();
        View = GetComponent<EntityView>();
        HealthController = new HealthController(Data.MaxHealth);
    }

    private void Start()
    {
        //charAIController = new LeaderAIController();
        new List<Node>();
        SetCharacterTag();
    }

    #endregion

    void SetCharacterTag()
    {
        if (gameObject.tag == TagManager.LEADER_TAG) HealthController.IsLeader = true;
        else if (gameObject.tag == TagManager.NPC_TAG) HealthController.IsNPC = true;
    }

    public override StateData[] GetStates() => CharAIData.FsmStates;

    public override EntityData GetData() => Data;
    public override Vector3 GetFoward() => transform.forward;

    public override float GetSpeed() => rb.velocity.magnitude;
    public override void Move(Vector3 direction)
    {
        Debug.Log("Entro en move character model");
        direction.y = 0;
        //direction += _obstacleAvoidance.GetDir() * multiplier;
        rb.velocity = direction * (Data.MovementSpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, direction, Data.RotationSpeed * Time.deltaTime);
        //View.PlayWalkAnimation(true);
    }

    public override void LookDir(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        direction.y = 0;
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
}
