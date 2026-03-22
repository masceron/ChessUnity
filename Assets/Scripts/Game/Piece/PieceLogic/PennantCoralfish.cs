using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PennantCoralfish : Commons.PieceLogic
    {
        private const int Strength = 1;
        private const int Duration = 2;
        private const int Range = 1;
        public PennantCoralfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, None.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PennantCoralfishPassive(this, Strength, Duration, Range)));
        }
    }
}