using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPointCount : MonoBehaviour
{
    
    [SerializeField] int point;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            PlayerManager2.instance.AddPlayerPoint(0, point);
            
           
        }
        if (other.CompareTag("Player2"))
        {
            PlayerManager2.instance.AddPlayerPoint(1, point);
            

        }
        if (other.CompareTag("Player3"))
        {
            PlayerManager2.instance.AddPlayerPoint(2, point);


        }
    }
}
