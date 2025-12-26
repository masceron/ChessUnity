using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Action.Internal;
using Game.Effects;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
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
        
        public static bool IsAtPromotionRank(int index)
        {
            var rank = RankOf(index);
            return rank is 14 or 25;
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

        public static void SetActiveSquare(int pos, bool value)
        {
            MatchManager.Ins.GameState.ActiveBoard[pos] = value;
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

        public static void Move(int from, int to)
        {
            MatchManager.Ins.GameState.Move(from, to);
        }

        public static void FlipSideToMove()
        {
            MatchManager.Ins.GameState.FlipSideToMove();
        }

        public static void NotifyMainAction(Action.Action mainAction)
        {
            MatchManager.Ins.GameState.NotifyMainAction(mainAction);
        }

        public static void NotifyEnd(Action.Action mainAction)
        {
            MatchManager.Ins.GameState.NotifyEnd(mainAction);
        }

        public static void NotifyInternalAction(Action.Action action)
        {
            if (action is ApplyEffect applyEffect)
            {
                MatchManager.Ins.GameState.NotifyWhenApplyEffect(applyEffect);
            }
            else if (action is Block block)
            {
                MatchManager.Ins.GameState.NotifyBlock(block);
            }
        }

        public static void IncrementSkillUses(Action.Action skill)
        {
            MatchManager.Ins.GameState.IncrementSkillUses(skill);
        }

        public static int SkillUseOf(bool color)
        {
            return color ? MatchManager.Ins.GameState.BlackSkillUses : MatchManager.Ins.GameState.WhiteSkillUses;
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

        public static List<PieceLogic> GetPiecesInSize(int rank, int file, int size, Corner corner, Predicate<PieceLogic> predicate)
        {
            var pieces = new List<PieceLogic>();
            if (corner == Corner.BottomRight)
            {
                rank = rank - size / 2 + 1;
                file = file - size / 2 + 1;
            }
            else if (corner == Corner.TopLeft)
            {
                file = file - size / 2 + 1;
                rank = rank - size / 2;
            }
            else if (corner == Corner.TopRight)
            {
                rank = rank - size / 2;
                file = file - size / 2;
            }
            else if (corner == Corner.BottomLeft)
            {
                rank = rank - size / 2 + 1;
                file = file - size / 2;
            }

            for (var r = rank; r < rank + size; r++)
            {
                if (!VerifyBounds(r)) continue;
                for (var f = file; f < file + size; f++)
                {
                    if (!VerifyBounds(f)) continue;
                    var piece = PieceOn(IndexOf(r, f));
                    if (piece != null && predicate(piece))
                    {
                        pieces.Add(piece);
                    }
                }
            }
            return pieces;
        }

        public static List<PieceLogic> FindPiece<T>(bool side) where T : PieceLogic
        {
            return MatchManager.Ins.GameState.PieceBoard.Where(piece => piece is T && piece.Color == side).ToList();
        }
        
        public static RelicLogic GetRelicOf(bool side)
        {
            return !side ? MatchManager.Ins.GameState.WhiteRelic : MatchManager.Ins.GameState.BlackRelic;
        }
        
        public static PieceLogic GetCommanderOf(bool side)
        {
            return side ? MatchManager.Ins.GameState.BlackCommander : MatchManager.Ins.GameState.WhiteCommander;
        }

        public static Vector3 FromRankFileToWorldPos(float rank, float file) {
            return new Vector3(rank, YCoordinate, file);
        }
        public static bool IsNextEachOther(PieceLogic piece)
        {
            var pos = piece.Pos;
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var indexOff = IndexOf(RankOf(pos) + i, FileOf(pos) + j);

                    if (!VerifyIndex(indexOff)) continue;
                    var pieceOff = PieceOn(indexOff);
                    if (pieceOff != null && pieceOff.Color == piece.Color) return true;
                }
            }
            return false;
        }

        // Return a list of white or black pieces with a specific effect.
        // Use this function even when you want to grab a number of effects by using .Count. 
        public static List<PieceLogic> FindPiecesWithEffectName(bool side, string effectName)
        {
            List<PieceLogic> withEffect = new List<PieceLogic>();
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece.Color == side && piece.Effects.Any(effect => effect.EffectName == effectName))
                {
                    withEffect.Add(piece);
                }
            }
            return withEffect;
        }

        public static List<Effect> EffectWithEffectCategory(PieceLogic piece, EffectCategory effectCategory)
        {
            List<Effect> effectsWithEffectCategory = new List<Effect>();
            foreach (var effect in piece.Effects)
            {
                if (effect.Category == effectCategory)
                {
                    effectsWithEffectCategory.Add(effect);
                }
            }
            return effectsWithEffectCategory;
        }
        
    }
}