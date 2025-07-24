using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class HardenedShield: Effect
    { 
        public HardenedShield(sbyte duration, PieceLogic piece, sbyte stack = 1) : base(duration, stack, piece, EffectType.HardenedShield)
        {}
        
        public override void OnCall(Action.Action action)
        {
            if (action == null || action.To != Piece.pos || action.Result != ActionResult.Succeed) return;
            action.Result = ActionResult.Failed;
            
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, MatchManager.gameState.MainBoard[action.Caller])));

            if (Strength > 1) Strength--;
            else
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}