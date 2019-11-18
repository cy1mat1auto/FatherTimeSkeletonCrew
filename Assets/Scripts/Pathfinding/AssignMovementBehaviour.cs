//UNUSED
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssignMovementBehaviour
{
    public static MovementBehaviour CreateBehaviour(MovementTypes addBehaviour, GameObject parent)
    {
        switch (addBehaviour)
        {
            case MovementTypes.A:
                return parent.AddComponent<TestPathfinding>();
            case MovementTypes.BFS:
                return parent.AddComponent<RLEnemyAI>();
            case MovementTypes.FLOCK:
                return parent.AddComponent<FlockBehaviour>();
            default:
                return null;
        }
    }
}
