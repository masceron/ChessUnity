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
using static Game.Common.BoardUtils;
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
            Debug.Log("Red Tide Applied");
            if (isActive == 1)
            {
                int startingSizeX = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.x) / 2;
                int startingSizeY = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.y) / 2;
                var random = new System.Random();
                int randomColumn = random.Next(1, MatchManager.Ins.startingSize.x);
                Debug.Log("Random Column: " + randomColumn);
                while(!IsColumnFull(randomColumn))
                {
                    randomColumn = random.Next(1, MatchManager.Ins.startingSize.x);
                }
                for (int i = 0; i < MatchManager.Ins.startingSize.y; i++)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Infected(PieceOn(IndexOf(randomColumn + startingSizeX, i + startingSizeY)))));
                }
                isActive = 0;
            }
            isActive++;
        }

        private bool IsColumnFull(int column)
        {
            int startingSizeX = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.x) / 2;
            int startingSizeY = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.y) / 2;
            for (int i = startingSizeY; i < startingSizeY + MatchManager.Ins.startingSize.y; i++)
            {
                if(!TileManager.Ins.IsTileEmpty(IndexOf(column + startingSizeX, i))) return false;
            }
            return true;
        }

    }
}