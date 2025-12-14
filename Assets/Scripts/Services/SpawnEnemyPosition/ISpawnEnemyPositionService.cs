using UnityEngine;

namespace Services.SpawnEnemyPosition
{
    public interface ISpawnEnemyPositionService
    {
        Vector3 GetPosition();
    }
}