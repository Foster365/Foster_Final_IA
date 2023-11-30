using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    public GameObject boss;
    public string bossName;

    // Start is called before the first frame update
    void Start()
    {
        if (boss == null)
        {
            boss = GameObject.Find(bossName);
        }
    }

    private void Update()
    {
        if(boss.gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
        }
    }
}
