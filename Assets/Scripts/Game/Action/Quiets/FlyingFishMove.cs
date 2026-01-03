using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingFishMove: Action, IQuiets
    {
        public int From;
        public FlyingFishMove(int maker, int to) : base(maker)
        {
            From = maker;
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}