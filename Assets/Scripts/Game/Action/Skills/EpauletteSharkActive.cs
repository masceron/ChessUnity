
using Game.Action.Internal;
using Game.AI;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Action.Skills
{
    public class EpauletteSharkActive : Action, ISkills, IAIAction
    {
        public int AIPenaltyValue
        {
            get
            {
                var targetPiece = PieceOn(Target);
                return targetPiece != null
                        && targetPiece.Color != PieceOn(Maker)?.Color
                        && targetPiece.PieceRank == PieceRank.Swarm ? -50 : 0;
            }
        }

        public EpauletteSharkActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Target));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}