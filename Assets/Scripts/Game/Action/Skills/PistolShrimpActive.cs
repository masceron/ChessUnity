using MemoryPack;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PistolShrimpActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PistolShrimpActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -80;
            return 0;
        }

        public PistolShrimpActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Target));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}