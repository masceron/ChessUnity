using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Data.Pieces;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Barracuda: PieceLogic
    {
        public Barracuda(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
        }

        private void MakeMove(List<Action.Action> list, int tRank, int file, int distance)
        {
            if (!VerifyBounds(tRank) || !VerifyBounds(file)) return;
            
            var tpos = IndexOf(tRank, file);
            if (!IsActive(IndexOf(tRank, file))) return;
            
            var rank = RankOf(Pos);
            
            var pieceOn = PieceOn(tpos);
            if (pieceOn == null)
            {
                if (distance == EffectiveMoveRange)
                {
                    if (!Color)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (distance <= EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, tpos));
                }
            }
            else if (pieceOn.Color != Color)
            {
                if (distance == AttackRange)
                {
                    if (!Color)
                    {
                        if (tRank >= rank) return;
                    }
                    else if (tRank <= rank) return;
                }
                
                if (pieceOn.Color != Color)
                    list.Add(new NormalCapture(Pos, tpos));
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var (rank, pieceFile) = RankFileOf(Pos);
            
            for (var i = 1; i <= Math.Max(EffectiveMoveRange, AttackRange); i++)
            {
                for (var file = pieceFile - i; file <= pieceFile + i; file += 1)
                {
                    MakeMove(list, rank - i, file, i);
                }
                
                for (var file = pieceFile - i; file <= pieceFile + i - 1; file += 1)
                {
                    MakeMove(list, rank + i, file, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i; tRank++)
                {
                    MakeMove(list, tRank, pieceFile + i, i);
                }
                
                for (var tRank = rank - i + 1; tRank <= rank + i - 1; tRank++)
                {
                    MakeMove(list, tRank, pieceFile - i, i);
                }
            }
        }
    }
}