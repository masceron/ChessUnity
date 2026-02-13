using Game.Common;
using Game.Effects.Triggers;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;

namespace Game.Effects.Traits
{
    public class KelpForestPassive : Effect, IEndTurnTrigger 
    {

        public KelpForestPassive(PieceLogic piece) : base(-1, 1, piece, "effect_kelp_forest_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            getRandomPos();
        }

        private void getRandomPos() 
        {
            
            var pos = new System.Random().Next(1, MatchManager.Ins.StartingSize.x * MatchManager.Ins.StartingSize.y);
            while (!TileManager.Ins.IsTileEmpty(pos) && BoardUtils.GetFormation(pos) != null)
            {
                pos = new System.Random().Next(1, MatchManager.Ins.StartingSize.x * MatchManager.Ins.StartingSize.y);
            }
            var mappedPos = BoardUtils.PosMap(pos, MatchManager.Ins.StartingSize);
            var kelp = new Kelp(true, Piece.Color);
            kelp.SetDuration(6);
            BoardUtils.SetFormation(mappedPos, kelp);
            
        }

    }
}