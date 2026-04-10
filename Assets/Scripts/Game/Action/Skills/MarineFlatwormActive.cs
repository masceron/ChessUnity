using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MarineFlatwormActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MarineFlatwormActive()
        {
        }

        public MarineFlatwormActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        protected override void ModifyGameState()
        {
            //Làm lại
            // var config = new PieceConfig(GetMakerAsPiece().Type, GetMakerAsPiece().Color, GetTargetPos());
            // ActionManager.EnqueueAction(new SpawnPieceWithEffect(config, new Illusion(GetTargetAsPiece())));
            // ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}