using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class SloaneViperfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SloaneViperfishActive() { }

        [MemoryPackInclude]
        private bool _bleeding;
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        public SloaneViperfishActive(int maker, bool bleeding) : base(maker)
        {
            Maker = maker;
            Target = maker;
            _bleeding = bleeding;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(_bleeding
                ? new ApplyEffect(new Poison(1, PieceOn(Target)), PieceOn(Maker))
                : new ApplyEffect(new Bleeding(5, PieceOn(Target)), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}