using Game.Action;
using Game.Action.Internal;
using System.Collections.Generic;
using static Game.Common.BoardUtils;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Common;
using System.Linq;
using UnityEngine;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class BluffingMoves
    {
        public static void Quiets(List<Action.Action> list, int pos, ref int index)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.GetMoveRange(ref index);
            Debug.Log(moveRange);
            var color = piece.Color;
            var push = color ? 1 : -1;

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange - 1))
            {
                MakeMove(rankOff, fileOff);
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange - 1))
            {
                MakeMove(rankOff, fileOff);
            }
            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange - 1))
                {
                    MakeMove(rankOff, fileOff);
                }

                for (var rankOff = rank + push ;rankOff <= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff <= file + (rankOff - rank); fileOff++) 
                    {
                        MakeMove(rankOff, fileOff);
                    }
                }

            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange - 1))
                {
                    MakeMove(rankOff, fileOff);
                }
                
                for (var rankOff = rank + push ; rankOff >= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff >= file + (rankOff - rank); fileOff--) 
                    {
                        MakeMove(rankOff, fileOff);
                    }
                }
            }
            
            return;

            void MakeMove(int rankOff, int fileOff)
            {
                if (!VerifyBounds(rankOff) || !VerifyBounds(fileOff)) return;
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece != null) return;
                list.Add(new NormalMove(pos, index));
            }
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
            var (rank, file) = RankFileOf(pos);
            var piece = PieceOn(pos);
            var moveRange = piece.AttackRange;
            var color = piece.Color;
            var push = color ? 1 : -1;

            foreach (var (rankOff, fileOff) in MoveEnumerators.Right(rank, file, moveRange - 1))
            {
                MakeCapture(rankOff, fileOff);
            }
            foreach (var (rankOff, fileOff) in MoveEnumerators.Left(rank, file, moveRange - 1))
            {
                MakeCapture(rankOff, fileOff);
            }
            if (color)
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Up(rank, file, moveRange - 1))
                {
                    MakeCapture(rankOff, fileOff);
                }

                for (var rankOff = rank + push ;rankOff <= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff <= file + (rankOff - rank); fileOff++) 
                    {
                        MakeCapture(rankOff, fileOff);
                    }
                }

            }
            else
            {
                foreach (var (rankOff, fileOff) in MoveEnumerators.Down(rank, file, moveRange - 1))
                {
                    MakeCapture(rankOff, fileOff);
                }
                
                for (var rankOff = rank + push ; rankOff >= rank + push * moveRange; rankOff += push)
                {
                    for( var fileOff = file - (rankOff - rank); fileOff >= file + (rankOff - rank); fileOff--) 
                    {
                        MakeCapture(rankOff, fileOff);
                    }
                }
            }

            return;

            void MakeCapture(int rankOff, int fileOff)
            {
                if (!VerifyBounds(rankOff) || !VerifyBounds(fileOff)) return;
                var index = IndexOf(rankOff, fileOff);
                if (!IsActive(index)) return;
                var piece = PieceOn(index);
                if (piece == null) return;
                if (piece.Color == color) return;
                list.Add(new NormalCapture(pos, index));
            }
        }
        
    }


    
}