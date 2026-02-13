using System;

namespace Game.Effects.Triggers
{
    public interface IOnRemoveTrigger: IComparable<IOnRemoveTrigger>
    {
        public void OnRemove();

        int IComparable<IOnRemoveTrigger>.CompareTo(IOnRemoveTrigger other)
        {
            return 0;
        }
    }
}