﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoLine : MonoBehaviour
{
    public GameObject[] Adjacent;
    private Collider[] ToFeed;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        int LM = LayerMask.GetMask("PathfindingWaypoints");
        ToFeed = Physics.OverlapSphere(transform.position, 300f, LM);

        Adjacent = new GameObject[ToFeed.Length];

        for (int j = 0; j < ToFeed.Length; j++)
        {
            Adjacent[j] = ToFeed[j].gameObject;
        }

        if (Adjacent.Length == 0 || Adjacent == null)
        {
            return;
        }

        else
        {
            for (int i = 0; i < Adjacent.Length; i++)
            {
                // ... and draw a line between them
                if (Adjacent[i] != null)
                    Gizmos.DrawLine(transform.position, Adjacent[i].transform.position);
            }
        }
        
    }
}
