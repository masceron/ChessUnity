using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class AdrenalineRadiatorExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private AdrenalineRadiatorExecute()
        {
        }

        public AdrenalineRadiatorExecute(PieceLogic maker) : base(null, maker)
        {
        }

        protected override void ModifyGameState()
        {
            GetTargetAsPiece().SkillCooldown = 0;
        }
    }
}