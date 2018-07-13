using UnityEngine.EventSystems;

namespace Assets.Scripts.Messaging
{
    public interface IDrawShootMessageTarget: IEventSystemHandler
    {
        void EnemyShotPlayer();
        void EnemyDrawed();
        void PlayerDrawed();
    }
}
