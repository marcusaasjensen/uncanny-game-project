using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpawnerController))]
public class SpawnerGizmos : MonoBehaviour
{
    [SerializeField] SpawnerController _spawnerController;

    [Header("Direction visualizer")]
    [SerializeField] Color _lineGizmosColor = Color.white;

    [Header("Scopes visualizer")]
    [SerializeField] float _scopeCubeGizmosSize = .1f;
    [SerializeField] Color _scopeCubeGizmosColor = Color.black;

    void OnDrawGizmos()
    {
        if (_spawnerController == null)
        {
            Debug.LogWarning("Spawner Controller reference in Spawner Gizmos script is missing.");
            return; 
        }

        SpawningBehaviour behaviour = _spawnerController.spawningBehaviour;
        
        if (behaviour == null)
        {
            Debug.LogWarning("Spawning Behaviour reference in spawner controller is missing.");
            return;
        }

        ProjectileController proj = _spawnerController.spawningBehaviour.projectileToSpawnConfig;

        if(proj == null)
        {
            Debug.LogWarning("Projectile Controller reference (config) in spawner controller is missing.");
            return;
        }

        Vector3 gizmosLineEndPos;
        Vector3 endPosition;

        float angle;
        float initAngle = 0;
        float offSet = 360 / _spawnerController.spawningBehaviour.NumberOfScopes;
        float nBProjectilePerScope = _spawnerController.spawningBehaviour.NumberOfProjectiles / behaviour.NumberOfScopes;
        float counter = 0;

        float rayLength;

        float rotationOffset = behaviour.SpawnerDirectionAffectsProjectile ? _spawnerController.ProjectileMovement.Direction : 0;

        for (int j = 0; j < behaviour.NumberOfScopes; j++)
        {
            angle = initAngle;
            endPosition = CalculateEndPoint(2 * proj.ProjectileMovement.Direction, angle, rotationOffset);
            Gizmos.color = _scopeCubeGizmosColor;
            Gizmos.DrawCube(_spawnerController.transform.position + endPosition, _scopeCubeGizmosSize * Vector3.one);

            for (int i = 1; i <= nBProjectilePerScope; i++)
            {
                rayLength = (proj.ProjectileMovement.MoveSpeed + GetNewMoveSpeed(proj.ProjectileMovement.MoveSpeed, counter)) * proj.TimeToLive;

                if (counter < behaviour.NumberOfProjectiles)
                    counter++;

                endPosition = CalculateEndPoint(2 * proj.ProjectileMovement.Direction, angle, rotationOffset);
                gizmosLineEndPos = rayLength * endPosition;

                Gizmos.color = _lineGizmosColor;
                Gizmos.DrawLine(_spawnerController.transform.position, _spawnerController.transform.position + gizmosLineEndPos);

                angle += offSet * behaviour.ScopeRange / nBProjectilePerScope;
                if (i == nBProjectilePerScope)
                {
                    Gizmos.color = _scopeCubeGizmosColor;
                    Gizmos.DrawCube(_spawnerController.transform.position + endPosition, _scopeCubeGizmosSize * Vector3.one);
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
        SpawningBehaviour behaviour = _spawnerController.spawningBehaviour;

        return dir + offSet + angle * behaviour.SpawningRange;
    }

    float GetNewMoveSpeed(float speed, float step)
    {
        SpawningBehaviour behaviour = _spawnerController.spawningBehaviour;

        return speed + Mathf.Pow(Mathf.Sin(behaviour.NumberOfSides * Mathf.PI / behaviour.NumberOfProjectiles * step), 2) * behaviour.SideBending;
    }
}
