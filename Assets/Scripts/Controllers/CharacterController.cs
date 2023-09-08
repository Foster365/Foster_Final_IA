using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Main.Scripts.FSM_SO_VERSION;

public class CharacterController : MonoBehaviour
{

    [SerializeField] StateData fsmInitState;

    Transform targetPosition;
    CharacterModel model;
    CharacterAIController charAIController;
    [SerializeField] GameObject finalNode;

    public CharacterAIController CharAIController { get => charAIController; set => charAIController = value; }

    //LeaderAIController leaderAIController;

    private void Awake()
    {
        model = GetComponent<CharacterModel>();
    }

    void Start()
    {
        charAIController = new CharacterAIController(model, fsmInitState, finalNode);
        charAIController.InitControllerComponents();
    } 

    void Update()
    {
        charAIController.UpdateControllerComponents();
    }

}