using Morkwa.Interface;
using Morkwa.MainData;
using Morkwa.Mechanics.CommonBehaviours;
using Morkwa.Mechanics.Spawne;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Morkwa.Mechanics.MainSpawner
{
    public sealed class SpawnerConfigurator : CommonBehaviour, ISpawning
    {
        [SerializeField] private GameObject _game;
        private IGame _iGame;

        [SerializeField] private GameObject _objectCreationPipeline;
        private ISpawner _spawner;

        private readonly List<Vector3> _walls = new List<Vector3>();
        private IReadOnlyList<Vector3> _wallsList => _walls;

        [Space]
        [SerializeField] private GameObject _createrPlanePoint;
        [SerializeField] private Transform _startPointGame;

        [Space]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _exitPrefab;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _groundPrefab;

        [Space]
        [SerializeField] private GameObject[] _obstaclePrefabs;
        [SerializeField] private GameObject[] _enemyPrefabs;

        private string _nameFolderForObstacle = "Obstacle";
        private string _nameFolderForEnemy = "Enemy";
        private string _nameFolderForGround = "Ground";
        private string _nameFolderForWalls = "Walls";

        private float _defaultHeightExit = 0.5f;
        private float _exitPoint = 2f;

        private Transform _groundTransform;
        private Transform _outerWallsTransform;
        private Transform _wallsTransform;
        private Transform _enemiesTransform;

        private int _columnsCount;
        private int _rowsCount;
        private int _obstacleCount;
        private int _enemyCount;

        public void SetSpawningConfiguration(IGame game, GameFieldConfig config)
        {
            _iGame = game;

            _columnsCount = (int)config.SizeGameField.x;
            _rowsCount = (int)config.SizeGameField.y;
            _obstacleCount = config.CountObstacle;
            _enemyCount = config.CountEnemy;

            CreateCreationPipeline();
        }

        private void CreateCreationPipeline()
        {
            if (_objectCreationPipeline.TryGetComponent(out ISpawner spawner))
                _spawner = spawner;

            CreateGameField();
        }

        private void CreateGameField()
        {
            _spawner.GameFieldCreater(CreateGameFieldConfig());
            WallsCreater();
            SetConfigurations();
            _spawner.GeneratorRandomObjects(CreateStructArrayObject(_obstaclePrefabs, _wallsTransform, _nameFolderForObstacle, _obstacleCount, false));
            _spawner.CreatePalyerObject(CreatePlayerObject(_playerPrefab, _startPointGame));
            BakeNavMesh();
            _spawner.GeneratorRandomObjects(CreateStructArrayObject(_enemyPrefabs, _enemiesTransform, _nameFolderForEnemy, _enemyCount, true));
            _spawner.CreateExitObject(ExitObjectCreater());
        }

        private void WallsCreater()
        {
            for (int x = 1; x < _columnsCount; x++)
            {
                for (int z = 1; z < _rowsCount; z++)
                {
                    _walls.Add(new Vector3(x, 0, z));
                }
            }
        }

        private void SetConfigurations()
        {
            ConfigSpawner config = new ConfigSpawner();
            config.IGame = _iGame;
            config.WallsList = _walls;

            _spawner.SetReference(config);
        }

        private void BakeNavMesh()
        {
            var surface = _createrPlanePoint.AddComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }

        private GameFieldCreater CreateGameFieldConfig()
        {
            var confg = new GameFieldCreater();
            confg.GroundTransform = _groundTransform;
            confg.OuterWallsTransform = _outerWallsTransform;
            confg.NameFoldersForGround = _nameFolderForGround;
            confg.NameFoldersForWalls = _nameFolderForWalls;
            confg.SizeField = new Vector2(_columnsCount, _rowsCount);
            confg.GroundPrefabs = _groundPrefab;
            confg.WallPrefabs = _wallPrefab;

            return confg;
        }

        private ArrayCreater CreateStructArrayObject(GameObject[] Prefabs, Transform parent, string parentName, int count, bool isEnemy)
        {
            var array = new ArrayCreater();
            array.Prefabs = Prefabs;
            array.Transform = parent;
            array.ParentName = parentName;
            array.Count = count;
            array.IsEnemy = isEnemy;

            return array;
        }

        private PlayerCreater CreatePlayerObject(GameObject Prefabs, Transform parent)
        {
            var objCreter = new PlayerCreater();
            objCreter.Prefab = Prefabs;
            objCreter.Position = parent;

            return objCreter;
        }

        private ExitObjectCreater ExitObjectCreater()
        {
            var exit = new ExitObjectCreater();
            exit.Prefab = _exitPrefab;
            exit.Position = new Vector3(_columnsCount - _exitPoint, _defaultHeightExit, _rowsCount - _exitPoint);
            exit.quaternion = Quaternion.identity;

            return exit;
        }

        public IReadOnlyList<Vector3> GetWallsList()
        {
            return _wallsList;
        }
    }
}