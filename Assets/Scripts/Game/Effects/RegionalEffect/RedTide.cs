using Game.Effects.Debuffs;
using Game.Managers;
using Game.Action.Internal;
using Game.Action;
using static Game.Common.BoardUtils;
using System.Collections.Generic;

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
            startingSizeX = (MaxLength - MatchManager.Ins.startingSize.x) / 2;
            startingSizeY = (MaxLength - MatchManager.Ins.startingSize.y) / 2;
        }
        protected override void ApplyEffect(int currentTurn)
        {
            isActive++;
            if (isActive == 10)
            {
                List<int> columnList = new List<int>();
                for (int i = 0; i < MatchManager.Ins.startingSize.y; i++)
                {
                    if(!IsColumnFull(i)) continue;
                    columnList.Add(i);
                }
                var random = new System.Random();
                var randomColumn = columnList[random.Next(columnList.Count)];   
                ApplyEffectToColumn(randomColumn);
                isActive = 0;
            }
        }

        private bool IsColumnFull(int column)
        {
            for (var i = startingSizeY; i < startingSizeY + MatchManager.Ins.startingSize.y; i++)
            {
                if(TileManager.Ins.IsTileEmpty(IndexOf(i, column + startingSizeY))) return false;
            }
            return true;
        }

        private void ApplyEffectToColumn(int column)
        {
            for (var i = startingSizeY; i < startingSizeY + MatchManager.Ins.startingSize.y; i++)
            {
                var piece = PieceOn(IndexOf(i, column + startingSizeY));    
                if(piece == null) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Infected(piece)));
            }
        }

    }
}