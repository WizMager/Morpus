using Configs;
using Configs.Enemy;
using Scellecs.Morpeh;
using Services.SpawnEnemyPosition;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private GameConfig _gameConfig;
        
        private World _world;
        private ISpawnEnemyPositionService _spawnEnemyPositionService;

        private void Start()
        {
                _world = World.Default;
                
                _spawnEnemyPositionService = new SpawnEnemyPositionService(1.5f, _gameConfig.EnemySpawnRadius, _world);
                
                _world.AddSystemsGroup(order: 0, InitializeSystemsGroup());
                _world.AddSystemsGroup(order: 1, UpdateSystemsGroup());
        }

        private SystemsGroup InitializeSystemsGroup()
        { 
                var initializeSystemGroup = _world.CreateSystemsGroup();
                
                initializeSystemGroup.AddInitializer(new PlayerInitializeSystem(_prefabsConfig, _playerConfig));
                initializeSystemGroup.AddInitializer(new EnemyInitializeSystem(_gameConfig, _enemyConfig, _prefabsConfig, _spawnEnemyPositionService));

                return initializeSystemGroup;
        }
        
        private SystemsGroup UpdateSystemsGroup()
        {
                var updateSystemGroup = _world.CreateSystemsGroup();
                
                updateSystemGroup.AddSystem(new PlayerMoveSystem());
                updateSystemGroup.AddSystem(new InputSystem());
                
                return updateSystemGroup;
        }
}