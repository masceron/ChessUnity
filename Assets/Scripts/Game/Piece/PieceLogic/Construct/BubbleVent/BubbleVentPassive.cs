using Game.Effects;
using Game.Action;
using Game.Effects.Others;
using Game.Action.Internal;
using UnityEngine;
using Game.Tile;
using Game.Managers;
using Game.Common;
using Game.Effects.Debuffs;

namespace Game.Piece.PieceLogic.Construct
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BubbleVentPassive : Effect, IEndTurnEffect
    {
        private int countToSpawnEffect = 0;
        private readonly int intervalToSpawn = 2;
        private readonly int radius = 2;

        public BubbleVentPassive(PieceLogic piece) : base(-1, 1, piece, EffectName.BubbleVentPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType
        { get; private set; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            countToSpawnEffect++;

            if (countToSpawnEffect < intervalToSpawn) return;
            countToSpawnEffect = 0;

            RandomApplyEffectInRadius();
        }

        private void RandomApplyEffectInRadius()
        {
            var (randRank, randFile) = GetRandomPos();
            if (randRank == null || randFile == null    ) return;
            Formation bubbleVent = new BubbleVentFormation(10, true, Piece.Color);
            FormationManager.Ins.SetFormation(BoardUtils.IndexOf(randRank.Value, randFile.Value), bubbleVent);
            var pieceOn = BoardUtils.PieceOn(BoardUtils.IndexOf(randRank.Value, randFile.Value));
            if (pieceOn != null)
            {
                ActionManager.ExecuteImmediately(new ApplyEffect(new Bound(1, pieceOn)));
            }
        }

        private (int?, int?) GetRandomPos()
        {
            int randRank;
            int randFile;
            int counter = 0;
            do
            {
                counter++;
                var (rank, file) = BoardUtils.RankFileOf(Piece.Pos);
                randRank = Random.Range(Mathf.Max(0, rank - radius), Mathf.Min(BoardUtils.BoardSize - 1, rank + radius) + 1);
                randFile = Random.Range(Mathf.Max(0, file - radius), Mathf.Min(BoardUtils.BoardSize - 1, file + radius) + 1);
                if (counter > 8) return (null, null);
            } while (FormationManager.Ins.GetFormation(BoardUtils.IndexOf(randRank, randFile)) != null);
            
            return (randRank, randFile);
        }
    }

}
