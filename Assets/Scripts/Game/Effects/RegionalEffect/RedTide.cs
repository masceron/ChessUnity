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
        private int startingSizeX;
        private int startingSizeY;
        private int isActive;
        public RedTide() : base(RegionalEffectType.RedTide)
        {
            isActive = 0;
            startingSizeX = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.x) / 2;
            startingSizeY = (BoardUtils.MaxLength - MatchManager.Ins.startingSize.y) / 2;
            Debug.Log("Starting Size Y: " + MatchManager.Ins.startingSize.y);
        }
        protected override void ApplyEffect(int currentTurn)
        {
            if (isActive == 9)
            {

                var random = new System.Random();
                int randomColumn = random.Next(0, MatchManager.Ins.startingSize.y - 1);
                Debug.Log("Random Column: " + randomColumn);
                while(!IsColumnFull(randomColumn)){
                    randomColumn = random.Next(0, MatchManager.Ins.startingSize.y - 1);
                }
                ApplyEffectToColumn(randomColumn);
                isActive = 0;
            }
            isActive++;
        }

        private bool IsColumnFull(int column)
        {
            for (int i = startingSizeY; i < startingSizeY + MatchManager.Ins.startingSize.y; i++)
            {
                Debug.Log("Checking tile: " + IndexOf(i, column + startingSizeY));
                if(TileManager.Ins.IsTileEmpty(IndexOf(i, column + startingSizeY))) return false;
            }
            return true;
        }

        private void ApplyEffectToColumn(int column)
        {
            for (int i = startingSizeY; i < startingSizeY + MatchManager.Ins.startingSize.y; i++)
            {
                PieceLogic piece = PieceOn(IndexOf(i, column + startingSizeY));    
                if(piece == null) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Infected(piece)));
            }
        }

    }
}