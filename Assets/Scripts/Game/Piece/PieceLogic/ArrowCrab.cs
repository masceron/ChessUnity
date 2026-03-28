using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArrowCrab : Commons.PieceLogic
    {
        public ArrowCrab(PieceConfig cfg) : base(cfg, CrabMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ArrowCrabNocturnal(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ArrowCrabTerritorial(this)));
        }
    }
}