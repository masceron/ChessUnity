using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ChamberedNautilusActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ChamberedNautilusActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -5 : 0;
        }

        public ChamberedNautilusActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, PieceOn(Target)), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}