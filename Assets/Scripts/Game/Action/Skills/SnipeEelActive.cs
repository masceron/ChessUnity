using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public SnipeEelActive(ushort maker, int to) : base(maker)
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