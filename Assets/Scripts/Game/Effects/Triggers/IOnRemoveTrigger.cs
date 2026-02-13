using System;

namespace Game.Effects.Triggers
{
    public interface IOnRemoveTrigger : IComparable<IOnRemoveTrigger>
    {
        int IComparable<IOnRemoveTrigger>.CompareTo(IOnRemoveTrigger other)
        {
            return 0;
        }

        public void OnRemove();
    }
}