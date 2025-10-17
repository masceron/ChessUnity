using Game.Effects;
using Game.Action;
using Game.Common;
using Game.Managers;
using System.Linq;
using System.Collections.Generic;
using Game.Tile;
using UnityEngine;

namespace Game.Piece.PieceLogic.Construct.KelpForest
{
    public class KelpForestPassive : Effect, IEndTurnEffect 
    {
        private List<int> randomPos = new List<int>();
        public KelpForestPassive(PieceLogic piece, sbyte countToSpawn) : base(countToSpawn, 1, piece, EffectName.KelpForestPassive)
        {
            
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            getRandomPos();
        }

        private void getRandomPos() 
        {
            
            int pos = new System.Random().Next(1, MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y);
            while (randomPos.Contains(pos))
            {
                pos = new System.Random().Next(1, MatchManager.Ins.startingSize.x * MatchManager.Ins.startingSize.y);
            }
            randomPos.Add(pos);
            FormationManager.Ins.SetFormation(BoardUtils.PosMap(pos, MatchManager.Ins.startingSize), new Kelp());


        }

    }
}