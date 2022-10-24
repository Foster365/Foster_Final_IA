using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    CharacterModel characterModel;
    void Start()
    {
        characterModel = GetComponent<CharacterModel>();
    }

    void Update()
    {
        if (characterModel.ReadyToMove)
        {
            characterModel.Run();
        }
    }
}
