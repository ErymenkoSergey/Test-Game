using Morkwa.Interface;
using Morkwa.Mechanics.Characters;
using Morkwa.Mechanics.CommonBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Morkwa.Mechanics.Spawne
{
    public sealed class ObjectCreationPipeline : CommonBehaviour, ISpawner
    {
        private IGame _iGame;
        private List<Vector3> _wallsList = new List<Vector3>();

        private float _defaultHeight = 0.0f;
        private float _wallDisplacementCoefficientsStep = 1f;
        private float _wallDisplacementCoefficientsHalfStep = 0.5f;

        public void SetReference(ConfigSpawner config)
        {
            _iGame = config.IGame;
            _wallsList = config.WallsList;
        }

        public void GameFieldCreater(GameFieldCreater gameField)
        {
            gameField.GroundTransform = new GameObject(gameField.NameFoldersForGround).transform;
            gameField.OuterWallsTransform = new GameObject(gameField.NameFoldersForWalls).transform;

            for (int x = 0; x < gameField.SizeField.x; x++)
            {
                for (int z = 0; z < gameField.SizeField.y; z++)
                {
                    GameObject CellGround = gameField.GroundPrefabs;

                    if (x == 0 || x == gameField.SizeField.x - 1 || z == 0 || z == gameField.SizeField.y - 1)
                    {
                        if (z == 0) //back
                            Instantiate(gameField.WallPrefabs, new Vector3(x, _defaultHeight, z - _wallDisplacementCoefficientsHalfStep),
                                Quaternion.AngleAxis(180, new Vector3(0, 1, 0))).transform.SetParent(gameField.OuterWallsTransform);
                        if (z == gameField.SizeField.y - 1) //forward
                            Instantiate(gameField.WallPrefabs, new Vector3(x + _wallDisplacementCoefficientsStep, _defaultHeight, z + _wallDisplacementCoefficientsStep),
                                Quaternion.AngleAxis(180, new Vector3(0, 1, 0))).transform.SetParent(gameField.OuterWallsTransform);
                        if (x == 0) //left
                            Instantiate(gameField.WallPrefabs, new Vector3(x - _wallDisplacementCoefficientsStep, _defaultHeight, z + _wallDisplacementCoefficientsHalfStep),
                                Quaternion.AngleAxis(90, new Vector3(0, 1, 0))).transform.SetParent(gameField.OuterWallsTransform);
                        if (x == gameField.SizeField.x - 1) //right
                            Instantiate(gameField.WallPrefabs, new Vector3(x + _wallDisplacementCoefficientsStep, _defaultHeight, z - _wallDisplacementCoefficientsHalfStep),
                                Quaternion.AngleAxis(-90, new Vector3(0, 1, 0))).transform.SetParent(gameField.OuterWallsTransform);
                    }

                    Instantiate(CellGround, new Vector3(x, 0, z), Quaternion.identity).transform.SetParent(gameField.GroundTransform);
                }
            }
        }

        public void GeneratorRandomObjects(ArrayCreater arrayCreater)
        {
            arrayCreater.Transform = new GameObject(arrayCreater.ParentName).transform;

            for (int i = 0; i < arrayCreater.Count; i++)
            {
                var randomPos = GetRandomPosition();

                GameObject prefab = arrayCreater.Prefabs[Random.Range(0, arrayCreater.Prefabs.Length)];

                if (arrayCreater.IsEnemy)
                {
                    Enemy enemy = Instantiate(prefab, randomPos, Quaternion.identity).GetComponent<Enemy>();
                    enemy.SetInfo(_iGame);
                    enemy.transform.SetParent(arrayCreater.Transform);
                }
                else
                {
                    Instantiate(prefab, randomPos, Quaternion.identity).transform.SetParent(arrayCreater.Transform);
                }
            }
        }

        private Vector3 GetRandomPosition()
        {
            var randomIndex = Random.Range(0, _wallsList.Count);
            Vector3 randomPosition = _wallsList[randomIndex];
            _wallsList.RemoveAt(randomIndex);

            return randomPosition;
        }

        public void CreatePalyerObject(PlayerCreater objectCreater)
        {
            Instantiate(objectCreater.Prefab, objectCreater.Position).GetComponent<Player>().SetInfo(_iGame);
        }

        public void CreateExitObject(ExitObjectCreater objectCreater)
        {
            Instantiate(objectCreater.Prefab, objectCreater.Position, objectCreater.quaternion);
        }
    }

    public struct ConfigSpawner
    {
        public IGame IGame;
        public List<Vector3> WallsList;
    }

    public struct GameFieldCreater
    {
        public Transform GroundTransform;
        public Transform OuterWallsTransform;
        public string NameFoldersForGround;
        public string NameFoldersForWalls;
        public Vector2 SizeField;
        public GameObject GroundPrefabs;
        public GameObject WallPrefabs;
    }

    public struct PlayerCreater
    {
        public GameObject Prefab;
        public Transform Position;
    }

    public struct ExitObjectCreater
    {
        public GameObject Prefab;
        public Vector3 Position;
        public Quaternion quaternion;
    }

    public struct ArrayCreater
    {
        public GameObject[] Prefabs;
        public Transform Transform;
        public string ParentName;
        public int Count;
        public bool IsEnemy;
    }
}