using System;

namespace Game.Triggers
{
    public enum BeforeActionPriority
    {
        Redirection = 100, //Triggers that change the target of the action. (Archelon's passive).
        Declaration = 90, //Triggers that declare the way in which the action will be carried out. (Piercing)
        Mitigation = 80, //Triggers that try to block the execution of the action (Shield).
        Reaction = 70 //Triggers that fire when no mitigation is possible. (Relentless).
    }

    public interface IBeforePieceActionTrigger : IComparable<IBeforeApplyEffectTrigger>
    {
        public BeforeActionPriority Priority { get; }

        int IComparable<IBeforeApplyEffectTrigger>.CompareTo(IBeforeApplyEffectTrigger other)
        {
            return other.Priority.CompareTo(Priority);
        }

        public void OnCallBeforePieceAction(Action.Action action);
    }
}