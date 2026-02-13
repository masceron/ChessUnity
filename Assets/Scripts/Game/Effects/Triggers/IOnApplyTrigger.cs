using System;

namespace Game.Effects.Triggers
{
    public interface IOnApplyTrigger : IComparable<IOnApplyTrigger>
    {
        int IComparable<IOnApplyTrigger>.CompareTo(IOnApplyTrigger other)
        {
            return 0;
        }

        public void OnApply();
    }
}