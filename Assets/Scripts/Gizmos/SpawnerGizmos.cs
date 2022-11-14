using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpawnerController))]
public class SpawnerGizmos : MonoBehaviour
{
    public SpawnerController controller;

    [Header("Direction visualizer")]
    [SerializeField] Color _lineGizmosColor = Color.white;

    [Header("Scopes visualizer")]
    [SerializeField] float _scopeCubeGizmosSize = .1f;
    [SerializeField] Color _scopeCubeGizmosColor = Color.black;

    void OnDrawGizmos()
    {
        if (controller == null) return;
        
        SpawningBehaviour behaviour = controller.spawningBehaviour;
        ProjectileController proj = controller.spawningBehaviour.projectileToSpawnConfig;

        Vector3 gizmosLineEndPos;
        Vector3 endPosition;

        float angle;
        float initAngle = 0;
        float offSet = 360 / controller.spawningBehaviour.NumberOfScopes;
        float nBProjectilePerScope = controller.spawningBehaviour.NumberOfProjectiles / behaviour.NumberOfScopes;
        float counter = 0;

        float rayLength;

        float rotationOffset = behaviour.SpawnerDirectionAffectsProjectile ? controller.projectileMovement.Direction : 0;

        for (int j = 0; j < behaviour.NumberOfScopes; j++)
        {
            angle = initAngle;
            endPosition = CalculateEndPoint(2 * proj.projectileMovement.Direction, angle, rotationOffset);
            Gizmos.color = _scopeCubeGizmosColor;
            Gizmos.DrawCube(controller.transform.position + endPosition, _scopeCubeGizmosSize * Vector3.one);

            for (int i = 1; i <= nBProjectilePerScope; i++)
            {
                rayLength = (proj.projectileMovement.MoveSpeed + GetNewMoveSpeed(proj.projectileMovement.MoveSpeed, counter)) * proj.TimeToLive;

                if (counter < behaviour.NumberOfProjectiles)
                    counter++;

                endPosition = CalculateEndPoint(2 * proj.projectileMovement.Direction, angle, rotationOffset);
                gizmosLineEndPos = rayLength * endPosition;

                Gizmos.color = _lineGizmosColor;
                Gizmos.DrawLine(controller.transform.position, controller.transform.position + gizmosLineEndPos);

                angle += offSet * behaviour.ScopeRange / nBProjectilePerScope;
                if (i == nBProjectilePerScope)
                {
                    Gizmos.color = _scopeCubeGizmosColor;
                    Gizmos.DrawCube(controller.transform.position + endPosition, _scopeCubeGizmosSize * Vector3.one);
                }
            }
            initAngle += offSet;
        }
    }

    Vector3 CalculateEndPoint(float directionInDegree, float angle, float offset)
    {
        float xPosition = Mathf.Cos(GetNewDirection(directionInDegree, angle, offset) * Mathf.Deg2Rad);
        float yPosition = Mathf.Sin(GetNewDirection(directionInDegree, angle, offset) * Mathf.Deg2Rad);
        return new(xPosition, yPosition, 0);
    }

    float GetNewDirection(float dir, float angle, float offSet)
    {
        SpawningBehaviour behaviour = controller.spawningBehaviour;

        return dir + offSet + angle * behaviour.SpawningRange;
    }

    float GetNewMoveSpeed(float speed, float step)
    {
        SpawningBehaviour behaviour = controller.spawningBehaviour;

        return speed + Mathf.Pow(Mathf.Sin(behaviour.NumberOfSides * Mathf.PI / behaviour.NumberOfProjectiles * step), 2) * behaviour.SideBending;
    }
}
