using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette
{
    //public string Run(Dictionary<string, int> dic)
    //{
    //    //P=60
    //    //M=30
    //    //S=10
    //    //Mondongo=5
    //    //Total=105
    //    float total = 0;
    //    foreach (var item in dic)
    //    {
    //        total += item.Value;
    //    }
    //    float random = Random.Range(0, total);

    //    foreach (var item in dic)
    //    {
    //        random -= item.Value;
    //        if (random < 0)
    //        {
    //            return item.Key;
    //        }
    //    }
    //    return "";
    //}
    public T Run<T>(Dictionary<T, int> dic)
    {
        float total = 0;
        foreach (var item in dic)
        {
            total += item.Value;
        }
        float random = Random.Range(0, total);

        foreach (var item in dic)
        {
            random -= item.Value;
            if (random < 0)
            {
                return item.Key;
            }
        }
        return default(T);
    }
}
