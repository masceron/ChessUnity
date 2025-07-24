using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Buffs
{
    public class Vengeful: Effect
    {
        public Vengeful(PieceLogic piece) : base(-1, 1, piece, EffectType.Vengeful)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null) return;
            
            if (action.To == Piece.pos && action.Result != ActionResult.Failed)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(3, MatchManager.gameState.MainBoard[action.To])));
            }
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}