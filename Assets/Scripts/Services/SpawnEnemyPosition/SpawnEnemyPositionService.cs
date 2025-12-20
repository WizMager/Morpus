using Scellecs.Morpeh;
using UnityEngine;

namespace Services.SpawnEnemyPosition
{
    public class SpawnEnemyPositionService : ISpawnEnemyPositionService
    {
        private const int MAX_ATTEMPTS = 10;
        
        private readonly float _safeRadiusSqr;
        private readonly float _spawnRadius;
        private readonly Filter _filter;
        private readonly Stash<TransformComponent> _transformStash;
        
        public SpawnEnemyPositionService(float safeRadius, float spawnRadius, World world)
        {
            _safeRadiusSqr = safeRadius * safeRadius;
            _spawnRadius = spawnRadius;
            
            _filter = world.Filter.With<PlayerComponent>().With<TransformComponent>().Build();
            _transformStash = world.GetStash<TransformComponent>();
        }
        
        public Vector3 GetPosition()
        {
            Vector3 candidate;
            var attempts = 0;
            
            var playerPosition = _transformStash.Get(_filter.First()).value.position;
        
            do {
                var randomCircle = Random.insideUnitCircle * _spawnRadius;
                candidate = new Vector3(randomCircle.x, 0, randomCircle.y);
            
                attempts++;
                
                if (attempts >= MAX_ATTEMPTS) 
                {
                    candidate = new Vector3(randomCircle.x, 0, randomCircle.y);
                    break;
                }
            
            } while ((candidate - playerPosition).sqrMagnitude < _safeRadiusSqr);
        
            return candidate;
        }
    }
}