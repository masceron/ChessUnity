using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelActive : Action, ISkills
    {
        public SnipeEelActive(ushort maker, int to) : base(maker, true)
        {
            Maker = maker;
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Bound(3, BoardUtils.PieceOn(Target))));
        }
    }
}