using Game.Action.Internal;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class OrnetesEdictExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private OrnetesEdictExecute()
        {
        }

        public OrnetesEdictExecute(PieceLogic target) : base(null, target)
        {
        }

        protected override void ModifyGameState()
        {
            if (GetTargetAsPiece() is not PieceLogic piece) return;

            var numberOfDebuffedPieces = piece.Effects.Count(e => e.Category == EffectCategory.Debuff);
            var rate = 7 * numberOfDebuffedPieces;

            if (MatchManager.Roll(rate)) ActionManager.EnqueueAction(new KillPiece(piece));
        }
    }
}