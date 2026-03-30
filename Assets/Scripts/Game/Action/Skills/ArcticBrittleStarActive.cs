using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class ArcticBrittleStarActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ArcticBrittleStarActive()
        {
        }

        public ArcticBrittleStarActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            if (GetMaker() is not PieceLogic maker || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -5;
            return 0;
        }

        protected override void ModifyGameState()
        {
            Formation anchorIce = new AnchorIce(((PieceLogic)GetMaker()).Color);
            anchorIce.SetDuration(3);
            FormationManager.Ins.SetFormation(GetTargetPos(), anchorIce);

            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}