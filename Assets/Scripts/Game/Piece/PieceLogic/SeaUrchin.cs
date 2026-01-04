using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeaUrchin: Commons.PieceLogic
    {
        public SeaUrchin(PieceConfig cfg) : base(cfg, PawnPushMoves.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Carapace(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Blinded(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SeaUrchinPassive(this)));
        }
    }
}