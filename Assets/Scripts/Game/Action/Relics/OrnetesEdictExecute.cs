using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Managers;
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

        public OrnetesEdictExecute(int target) : base(target)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            var piece = BoardUtils.PieceOn(Target);
            if (piece == null) return;

            var numberOfDebuffedPieces = piece.Effects.Count(e => e.Category == EffectCategory.Debuff);
            var rate = 7 * numberOfDebuffedPieces;

            if (MatchManager.Roll(rate)) ActionManager.EnqueueAction(new KillPiece(Target));
        }
    }
}