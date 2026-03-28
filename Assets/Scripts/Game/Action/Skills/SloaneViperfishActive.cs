using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class SloaneViperfishActive : Action, ISkills
    {
        [MemoryPackInclude] private bool _bleeding;

        [MemoryPackConstructor]
        private SloaneViperfishActive()
        {
        }

        public SloaneViperfishActive(int maker, bool bleeding) : base(maker)
        {
            _bleeding = bleeding;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(_bleeding
                ? new ApplyEffect(new Poison(1, GetTarget()), GetMaker())
                : new ApplyEffect(new Bleeding(5, GetTarget()), GetMaker()));
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}