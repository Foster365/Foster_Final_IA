using System;
using System.Collections;
using System.Collections.Generic;
using UADE.IA.FSM;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    Transform targetPosition;

    LeaderAIController characterAIController;

    private void Awake()
    {
        characterAIController = GetComponent<LeaderAIController>();
    }

    void Start()
    {
    }

    void Update()
    {

        //if (characterAIController.CharacterPathfinding.finalPath != null)
        //{
        //    Debug.Log("Final path nodes count: " + characterAIController.CharacterPathfinding.finalPath.Count);
        //    characterAIController.CharacterModel.waypoints = characterAIController.CharacterPathfinding.finalPath;
        //    Debug.Log("Player waypoints: " + characterAIController.CharacterModel.waypoints.Count);
        //    //for (int i = 0; i < characterModel.waypoints.Count; i++)
        //    //{
        //    characterAIController.CharacterModel.Run();
        //    //}
        //    //characterModel.SetWayPoints(pathfinding.finalPath);
        //}

        //if (characterModel.waypoints != null) characterModel.Run();

        //pathfinding = GameObject.Find("A*").gameObject.GetComponent<Pathfinding>();
        //if (pathfinding.finalPath.Count > 0)
        //{
        //    characterModel.waypoints = pathfinding.finalPath;
        //    Debug.Log("Final path to target is: " + characterModel.waypoints);
        //    //characterModel.SetWayPoints(characterModel.waypoints);
        //}
        //if(characterModel.waypoints != null) characterModel.Run();
    }

}