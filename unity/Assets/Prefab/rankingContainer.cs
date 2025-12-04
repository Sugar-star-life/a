using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankingContainer : MonoBehaviour
{
    private VerticalLayoutGroup vlg;
    public int space;

    private void Start ()
    {
         vlg = GetComponent<VerticalLayoutGroup>();
        vlg.spacing =space;
    }
    private void Update()
    {
        
    }




}
