using Morkwa.Mechanics.Spawne;

namespace Morkwa.Interface
{
    public interface ISpawner
    {
        void SetReference(ConfigSpawner config);
        void GameFieldCreater(GameFieldCreater gameField);
        void GeneratorRandomObjects(ArrayCreater arrayCreater);
        void CreatePalyerObject(PlayerCreater objectCreater);
        void CreateExitObject(ExitObjectCreater objectCreater);
    }
}