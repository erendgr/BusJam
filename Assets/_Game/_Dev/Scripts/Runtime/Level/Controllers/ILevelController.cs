using _Game._Dev.Scripts.Runtime.Level.Models;

namespace _Game._Dev.Scripts.Runtime.Level.Controllers
{
    public interface ILevelController
    {
        void LoadLevel(LevelSO levelData);
    }
}