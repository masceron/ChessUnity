using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HorseLeechAttack : Effect
    {
        public HorseLeechAttack(PieceLogic piece) : base(-1, 4, piece, EffectName.HorseLeechAttack)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action.Maker != Piece.Pos || action.Result == ActionResult.Failed) return;

            if (!VerifyIndex(action.Target)) return;
            var pieceAttack = PieceOn(action.Target);

            if (pieceAttack != null && pieceAttack.Color != Piece.Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(pieceAttack)));
            }
        }
    }
}