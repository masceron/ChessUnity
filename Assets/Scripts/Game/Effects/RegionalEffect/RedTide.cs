using System;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Effects.RegionalEffect
{
    public class RedTide : RegionalEffect
    {
        private readonly int startingSizeY;
        private int isActive;
        private int startingSizeX;

        public RedTide() : base(RegionalEffectType.RedTide)
        {
            isActive = 0;
            startingSizeX = (MaxLength - MatchManager.Ins.StartingSize.x) / 2;
            startingSizeY = (MaxLength - MatchManager.Ins.StartingSize.y) / 2;
        }

        protected override void ApplyEffect(int currentTurn)
        {
            if (isActive == 10)
            {
                var random = new Random();
                var randomColumn = random.Next(0, MatchManager.Ins.StartingSize.y - 1);
                while (!IsColumnFull(randomColumn)) randomColumn = random.Next(0, MatchManager.Ins.StartingSize.y - 1);
                ApplyEffectToColumn(randomColumn);
                isActive = 0;
            }

            isActive++;
        }

        private bool IsColumnFull(int column)
        {
            for (var i = startingSizeY; i < startingSizeY + MatchManager.Ins.StartingSize.y; i++)
                if (TileManager.Ins.IsTileEmpty(IndexOf(i, column + startingSizeY)))
                    return false;
            return true;
        }

        private void ApplyEffectToColumn(int column)
        {
            for (var i = startingSizeY; i < startingSizeY + MatchManager.Ins.StartingSize.y; i++)
            {
                var piece = PieceOn(IndexOf(i, column + startingSizeY));
                if (piece == null) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Infected(piece)));
            }
        }
    }
}