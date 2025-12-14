using Configs;
using Scellecs.Morpeh;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
        [SerializeField] private PrefabsConfig _prefabsConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        
        private World _world;

        private void Start()
        {
                _world = World.Default;
                
                _world.AddSystemsGroup(order: 0, InitializeSystemsGroup());
                _world.AddSystemsGroup(order: 1, UpdateSystemsGroup());
        }

        private SystemsGroup InitializeSystemsGroup()
        { 
                var initializeSystemGroup = _world.CreateSystemsGroup();
                
                initializeSystemGroup.AddInitializer(new PlayerInitializeSystem(_prefabsConfig, _playerConfig));

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