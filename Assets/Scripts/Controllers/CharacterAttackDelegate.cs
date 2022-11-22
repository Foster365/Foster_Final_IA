using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackDelegate : MonoBehaviour
{
    [SerializeField] GameObject attacker, leftArmCollider, weaponCollider, shieldCollider, magicPrefab;
    [SerializeField] bool hasShield;

    void LeftArmAttackOn()
    {
        leftArmCollider.SetActive(true);
    }

    void LeftArmAttackOff()
    {
        if (leftArmCollider.activeInHierarchy) leftArmCollider.SetActive(false);
    }

    void WeaponColliderOn()
    {
        weaponCollider.SetActive(true);
    }

    void WeaponColliderOff()
    {
        if (weaponCollider.activeInHierarchy) weaponCollider.SetActive(false);
    }

    void ShieldColliderOn()
    {
        if (hasShield) shieldCollider.SetActive(true);
    }
    void ShieldColliderOff()
    {
        if (hasShield && shieldCollider.activeInHierarchy) shieldCollider.SetActive(true);
    }
    void SpawnMysticalAttack()
    {
        GameObject.Instantiate(magicPrefab, leftArmCollider.transform.position, attacker.transform.rotation);
    }
}
