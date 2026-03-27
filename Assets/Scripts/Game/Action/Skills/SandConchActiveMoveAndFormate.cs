using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects;
using Game.Effects.States;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class SandConchActiveMoveAndFormate : Action, ISkills
    {
        public SandConchActiveMoveAndFormate(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        
        protected override void ModifyGameState()
        {
            var maker = PieceOn(Maker);
            Effect burrowed = maker.Effects.FirstOrDefault(e => e is Burrowed);
            
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
            //TODO: Fix Burrowed to work with SandConch
            
            FormationManager.Ins.SetFormation(Target, new SiltCloud(false));
            if (burrowed != null) ActionManager.EnqueueAction(new RemoveEffect(burrowed));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}