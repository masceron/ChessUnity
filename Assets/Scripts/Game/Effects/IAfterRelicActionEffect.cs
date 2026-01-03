using Game.Action.Relics;

namespace Game.Effects
{
    public interface IAfterRelicActionEffect
    {
        public void OnCallAfterRelicAction(IRelicAction relicAction);
    }
}