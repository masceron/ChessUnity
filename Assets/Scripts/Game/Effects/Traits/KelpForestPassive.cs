using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using System.Collections.Generic;

namespace Game.Effects.Traits
{
    public class KelpForestPassive : Effect, IEndTurnEffect 
    {

        public KelpForestPassive(PieceLogic piece) : base(-1, 1, piece, "effect_kelp_forest_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            getRandomPos();
        }

        private void getRandomPos() 
        {
            List<int> posList = new List<int>();
            for (int i = 0; i < MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y; i++)
            {
                var mapPos = BoardUtils.PosMap(i, MatchManager.Ins.startingSize);
                if (!TileManager.Ins.IsTileEmpty(mapPos) && FormationManager.Ins.GetFormation(mapPos) == null)
                {
                    posList.Add(mapPos);
                }
            }
            var random = new System.Random();
            int mapPosSelected = posList[random.Next(posList.Count)];
            var kelp = new Kelp(true, true);
            kelp.SetDuration(6);
            FormationManager.Ins.SetFormation(mapPosSelected, kelp);
            
        }

    }
}