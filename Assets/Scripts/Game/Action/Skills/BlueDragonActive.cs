using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class BlueDragonActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private BlueDragonActive()
        {
        }

        public BlueDragonActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            if (GetMaker() is not PieceLogic maker) return 0;
            return pieceAI.Color != maker.Color ? -15 : 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, GetTarget() as PieceLogic), GetMaker() as PieceLogic));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}