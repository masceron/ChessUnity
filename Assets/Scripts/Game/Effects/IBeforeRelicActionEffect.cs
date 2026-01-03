using Game.Action.Relics;

namespace Game.Effects
{
    public interface IBeforeRelicActionEffect
    {
        public void OnCallBeforeRelicAction(IRelicAction relicAction);
    }
}