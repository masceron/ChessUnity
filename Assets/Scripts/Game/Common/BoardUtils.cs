using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Board.Effects;
using Game.Board.General;
using Game.Board.Piece;
using Game.Board.Piece.PieceLogic;
using UnityEngine;
using Action = Game.Board.Action.Action;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        
    public static class BoardUtils
    {
        public const int MaxLength = 40;
        public const int BoardSize = MaxLength * MaxLength;

        public static int RankOf(int index)
        {
            return index / MaxLength;
        }

        public static int FileOf(int index)
        {
            return index % MaxLength;
        }

        public static (int, int) RankFileOf(int index)
        {
            return (RankOf(index), FileOf(index));
        }

        public static int IndexOf(int rank, int file)
        {
            return rank * MaxLength + file;
        }

        public static bool VerifyUpperBound(int dimension)
        {
            return dimension < MaxLength;
        }

        public static bool VerifyBounds(int dimension)
        {
            return dimension is >= 0 and < MaxLength;
        }

        public static bool VerifyIndex(int index)
        {
            return index is >= 0 and < BoardSize;
        }

        public static bool VerifyUpperIndex(int index)
        {
            return index < BoardSize;
        }
 
        public static int PushWhite(int pos)
        {
            return pos - MaxLength;
        }

        public static int PushBlack(int pos)
        {
            return pos + MaxLength;
        }

        public static int ClampDown(int dimension)
        {
            return Math.Min(dimension, MaxLength - 1);
        }

        public static int ClampUp(int dimension)
        {
            return Math.Max(dimension, 0);
        }

        public static int RowIndex(int row)
        {
            return row * MaxLength;
        }
        
        public static int Distance(int pos1, int pos2)
        {
            return Math.Max(Math.Abs(RankOf(pos1) - RankOf(pos2)), Math.Abs(FileOf(pos1) - FileOf(pos2)));
        }

        public static int PosMap(int pos, Vector2Int startingSize)
        {
            var rank = pos / startingSize.x;
            var file = pos % startingSize.y;

            return IndexOf(rank + (MaxLength - startingSize.x) / 2, file + (MaxLength - startingSize.y) / 2);
        }

        public static PieceLogic PieceOn(int pos)
        {
            return MatchManager.Ins.GameState.PieceBoard[pos];
        }

        public static bool IsActive(int pos)
        {
            return MatchManager.Ins.GameState.ActiveBoard[pos];
        }

        public static bool ColorOfSquare(int pos)
        {
            return MatchManager.Ins.GameState.SquareColor[pos];
        }

        public static bool ColorOfPiece(int pos)
        {
            return PieceOn(pos).Color;
        }

        public static void FlipPieceColor(int pos)
        {
            var gameState = MatchManager.Ins.GameState;
            gameState.PieceBoard[pos].Color = !gameState.PieceBoard[pos].Color;
        }

        public static void SetCooldown(int pos, sbyte cd)
        {
            MatchManager.Ins.GameState.PieceBoard[pos].SkillCooldown = cd;
        }

        public static PieceLogic[] PieceBoard()
        {
            return MatchManager.Ins.GameState.PieceBoard;
        }

        public static BitArray ActiveBoard()
        {
            return MatchManager.Ins.GameState.ActiveBoard;
        }

        public static bool OurSide()
        {
            return MatchManager.Ins.GameState.OurSide;
        }

        public static bool SideToMove()
        {
            return MatchManager.Ins.GameState.SideToMove;
        }

        public static ObservableCollection<PieceConfig> WhiteCaptured()
        {
            return MatchManager.Ins.GameState.WhiteCaptured;
        }

        public static ObservableCollection<PieceConfig> BlackCaptured()
        {
            return MatchManager.Ins.GameState.BlackCaptured;
        }

        public static void FlipSideToMove()
        {
            MatchManager.Ins.GameState.FlipSideToMove();
        }

        public static void Notify()
        {
            MatchManager.Ins.GameState.Notify();
        }

        public static void NotifyEnd()
        {
            MatchManager.Ins.GameState.NotifyEnd();
        }

        public static void NotifyOnMoveGen(List<Action> actions)
        {
            MatchManager.Ins.GameState.NotifyOnMoveGen(actions);
        }

        public static void AddObserver(Effect effect)
        {
            MatchManager.Ins.GameState.AddObserver(effect);
        }

        public static void RemoveObserver(Effect effect)
        {
            MatchManager.Ins.GameState.RemoveObserver(effect);
        }

        public static void SetMainAction(Action action)
        {
            MatchManager.Ins.GameState.MainAction = action;
        }
    }
}