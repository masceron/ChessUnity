using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    public partial class RustyParrotfishActive : Action, ISkills
    {

        [MemoryPackConstructor]
        private RustyParrotfishActive()
        {
        }

        public RustyParrotfishActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            var formation = BoardUtils.GetFormation(Target);
            ActionManager.EnqueueAction(new RemoveFormation(formation));
            ActionManager.EnqueueAction(new CooldownSkill(maker));
        }

        int ISkills.AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}