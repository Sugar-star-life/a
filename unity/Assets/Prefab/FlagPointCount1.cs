using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPointCount1 : MonoBehaviour
{
    [SerializeField] int point;
    private void OnTriggerEnter(Collider other)
    {
       
        GameObject playerObj = other.gameObject;

        int index = PlayerManager2.instance.Players.IndexOf(playerObj);
        PlayerManager2.instance.AddPlayerPoint(index, point);
        PlayerManager2.instance.EndJudge();

    }

}
