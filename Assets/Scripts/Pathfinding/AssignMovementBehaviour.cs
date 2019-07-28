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
            case MovementTypes.FLOCK:
                return parent.AddComponent<FlockBehaviour>();
            case MovementTypes.BFS:
                return parent.AddComponent<RLEnemyAI>();
            default:
                return null;
        }
    }
}
