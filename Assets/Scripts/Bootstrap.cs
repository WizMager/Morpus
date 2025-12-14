using Configs;
using Scellecs.Morpeh;
using Services.SpawnEnemyPosition;
using Services.UiService;
using Ui;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private GameConfig _gameConfig;
        
        [SerializeField] private GameHudWindow _gameHudWindow;
        
        private World _world;
        private ISpawnEnemyPositionService _spawnEnemyPositionService;
        private IUiProvider _uiProvider;

        private void Start()
        {
                _world = World.Default;
                
                _spawnEnemyPositionService = new SpawnEnemyPositionService(_gameConfig.SafeRadius, _gameConfig.EnemySpawnRadius, _world);
                _uiProvider = new UiProvider(_playerConfig, _world);
                
                _gameHudWindow.Initialize(_uiProvider, _playerConfig);
                
                _world.AddSystemsGroup(order: 0, InitializeSystemsGroup());
                _world.AddSystemsGroup(order: 1, UpdateSystemsGroup());
                _world.AddSystemsGroup(order: 2, FixedUpdateSystemsGroup());
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
                updateSystemGroup.AddSystem(new DealDamageSystem());
                updateSystemGroup.AddSystem(new RespawnEnemySystem(_spawnEnemyPositionService, _enemyConfig, _uiProvider));
                updateSystemGroup.AddSystem(new LevelUpSystem());
                
                return updateSystemGroup;
        }
        
        private SystemsGroup FixedUpdateSystemsGroup()
        {
                var fixedUpdateSystemsGroup = _world.CreateSystemsGroup();
                
                fixedUpdateSystemsGroup.AddSystem(new SearchTargetSystem(_playerConfig));
                
                return fixedUpdateSystemsGroup;
        }
}