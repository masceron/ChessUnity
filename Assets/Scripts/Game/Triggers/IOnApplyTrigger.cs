using System;

namespace Game.Triggers
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