using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using UnityEngine;
using Game.Action.Internal;
using Game.Action;
using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Game.Piece;

namespace Game.Effects.RegionalEffect
{
    public class RedTide: RegionalEffect
    {
        private int isActive;
        public RedTide() : base(RegionalEffectType.RedTide)
        {
            isActive = 1;
        }
        protected override void ApplyEffect(int currentTurn)
        {
            if (isActive == 10)
            {
                var random = new System.Random();
                int randomColumn = random.Next(0, MatchManager.Ins.startingSize.x);
                for (int i = 0; i < MatchManager.Ins.startingSize.y; i++)
                {
                    var piece = BoardUtils.PieceOn(BoardUtils.PosMap(MatchManager.Ins.startingSize.x * i + randomColumn, MatchManager.Ins.startingSize));
                    if (piece != null) {
                        ActionManager.EnqueueAction(new ApplyEffect(new Infected(piece)));
                    }
                }
                isActive = 0;
            }
            isActive++;
        }

    }
}