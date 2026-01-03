using Game.Common;
using Game.Managers;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalMove: Action, IQuiets
    {
        public NormalMove(int f, int t) : base(f)
        {
            Target = t;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            BoardUtils.Move(Maker, Target);
            Maker = Target;
        }
    }
}