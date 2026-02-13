using System;

namespace Game.Effects.Triggers
{
    public interface IOnApplyTrigger: IComparable<IOnApplyTrigger>
    {
        public void OnApply();

        int IComparable<IOnApplyTrigger>.CompareTo(IOnApplyTrigger other)
        {
            return 0;
        }
    }
}