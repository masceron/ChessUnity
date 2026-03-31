using MemoryPack;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class DwarfCuttlefishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private DwarfCuttlefishActive() { }

        public DwarfCuttlefishActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTargetAsPiece()));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}