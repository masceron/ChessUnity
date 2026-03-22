using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Debuffs;
using Game.Effects.SpecialAbility;
using Game.Effects.States;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SealochAnemone : Commons.PieceLogic
    {

        public SealochAnemone(PieceConfig cfg) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            // ActionManager.ExecuteImmediately(new ApplyEffect(new Pacified(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Symbiotic(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SealochAnemonePassive(this)));
        }
    }
}