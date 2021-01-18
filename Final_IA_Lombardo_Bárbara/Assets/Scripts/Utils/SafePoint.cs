using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePoint : MonoBehaviour
{

    Entity _character;
    [SerializeField]
    float recoveringPoints;

    private void OnCollisionEnter(Collision character)
    {
        if (character.gameObject.tag == "Character")
            Recover();
    }

    public void Recover()
    {

        if (_character.CurrentHealth <= _character.minimumLife)
            _character.CurrentHealth += recoveringPoints;

    }

}
