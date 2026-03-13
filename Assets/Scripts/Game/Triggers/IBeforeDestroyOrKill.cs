using Game.Action.Internal;

namespace Game.Triggers
{
    public interface IBeforeDestroyOrKill
    {
        void OnCallBeforeDestroyOrKill(IInternal action);
    }
}