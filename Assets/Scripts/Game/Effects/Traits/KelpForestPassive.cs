using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;

namespace Game.Effects.Traits
{
    public class KelpForestPassive : Effect, IEndTurnEffect 
    {

        public KelpForestPassive(PieceLogic piece) : base(-1, 1, piece, "effect_kelp_forest_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            getRandomPos();
        }

        private void getRandomPos() 
        {
            
            int pos = new System.Random().Next(1, MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y);
            while (!TileManager.Ins.IsTileEmpty(pos) && FormationManager.Ins.GetFormation(pos) != null)
            {
                pos = new System.Random().Next(1, MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y);
            }
            int mappedPos = BoardUtils.PosMap(pos, MatchManager.Ins.startingSize);
            Kelp kelp = new Kelp(true, true);
            kelp.SetDuration(6);
            FormationManager.Ins.SetFormation(mappedPos, kelp);
            
        }

    }
}