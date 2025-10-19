using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Action.Internal;
using Game.Effects;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class BoardUtils
    {
        public const int MaxLength = 40;
        public const int BoardSize = MaxLength * MaxLength;
        public const float YCoordinate = 1.64f;
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

        public static void Move(ushort from, ushort to)
        {
            MatchManager.Ins.GameState.Move(from, to);
        }

        public static void FlipSideToMove()
        {
            MatchManager.Ins.GameState.FlipSideToMove();
        }

        public static void NotifyMainAction()
        {
            MatchManager.Ins.GameState.Notify();
        }

        public static void NotifyEnd()
        {
            MatchManager.Ins.GameState.NotifyEnd();
        }

        public static void NotifyInternalAction(Action.Action action)
        {
            if (action is ApplyEffect applyEffect)
            {
                MatchManager.Ins.GameState.NotifyWhenApplyEffect(applyEffect);
            }
        }

        public static void NotifyOnMoveGen(List<Action.Action> actions)
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

        public static void SetMainAction(Action.Action action)
        {
            MatchManager.Ins.GameState.MainAction = action;
        }

        public static List<PieceLogic> GetPiecesInRadius(int rank, int file, int radius, Predicate<PieceLogic> predicate)
        {
            // Get all pieces in range of (rank, file) that match the predicate
            var pieces = new List<PieceLogic>();
            for (var rankOff = rank - radius; rankOff <= rank + radius; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - radius; fileOff <= file + radius; fileOff++)
                {
                    if (!VerifyBounds(fileOff)) continue;
                    var piece = PieceOn(IndexOf(rankOff, fileOff));
                    if (predicate(piece))
                    {
                        pieces.Add(piece);
                    }
                }
            }
            return pieces;
        }

        public static List<PieceLogic> FindPiece<T>(bool side) where T : PieceLogic
        {
            List<PieceLogic> validPieces = new List<PieceLogic>();
            foreach (PieceLogic piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece != null && piece is T && piece.Color == side)
                    validPieces.Add(piece);
            }
            return validPieces;
        }

        public static Vector3 FromRankFileToWorldPos(float rank, float file){
            return new Vector3(rank, YCoordinate, file);
        }
    }
}