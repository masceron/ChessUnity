using System;
using System.Collections;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Effects;
using Game.Effects.FieldEffect;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using Game.Tile;
using Game.Triggers;
using UnityEngine;
using UX.UI;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Common
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
            return PieceBoard()[pos];
        }

        public static bool IsActive(int pos)
        {
            return MatchManager.Ins.GameState.ActiveBoard[pos];
        }

        [Mutator]
        public static void SetActiveSquare(int pos, bool value)
        {
            MatchManager.Ins.GameState.ActiveBoard[pos] = value;
        }

        public static bool ColorOfSquare(int pos)
        {
            return MatchManager.Ins.GameState.SquareColor[pos];
        }

        [Mutator]
        public static void FlipPieceColor(PieceLogic piece)
        {
            piece.Color = !piece.Color;
        }

        [Mutator]
        public static void SetCooldown(PieceLogic piece, int cd)
        {
            piece.SkillCooldown = cd;
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

        public static IReadOnlyList<PieceConfig> GetCapturedOf(bool color)
        {
            return !color ? MatchManager.Ins.GameState.Captured.Item1 : MatchManager.Ins.GameState.Captured.Item2;
        }

        [Mutator]
        public static void Move(PieceLogic pieceLogic, int to)
        {
            MatchManager.Ins.GameState.Move(pieceLogic, to);
        }

        [Mutator]
        public static void FlipSideToMove()
        {
            MatchManager.Ins.GameState.FlipSideToMove();
        }

        [Mutator]
        public static void AddEffectObserver(Observer effect)
        {
            MatchManager.Ins.GameState.TriggerHooks.AddObserver(effect);
        }

        [Mutator]
        public static void RemoveObserver(Observer effect)
        {
            MatchManager.Ins.GameState.TriggerHooks.RemoveObserver(effect);
        }

        public static List<PieceLogic> GetPiecesInRadius(int rank, int file, int radius,
            Predicate<PieceLogic> predicate)
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
                    if (predicate(piece)) pieces.Add(piece);
                }
            }

            return pieces;
        }

        public static List<PieceLogic> GetPiecesInSize(int rank, int file, int size, Corner corner,
            Predicate<PieceLogic> predicate)
        {
            var pieces = new List<PieceLogic>();
            switch (corner)
            {
                case Corner.BottomRight:
                    rank = rank - size / 2 + 1;
                    file = file - size / 2 + 1;
                    break;
                case Corner.TopLeft:
                    file = file - size / 2 + 1;
                    rank -= size / 2;
                    break;
                case Corner.TopRight:
                    rank -= size / 2;
                    file -= size / 2;
                    break;
                case Corner.BottomLeft:
                    rank = rank - size / 2 + 1;
                    file -= size / 2;
                    break;
                default:
                    return null;
            }

            for (var r = rank; r < rank + size; r++)
            {
                if (!VerifyBounds(r)) continue;
                for (var f = file; f < file + size; f++)
                {
                    if (!VerifyBounds(f)) continue;
                    var piece = PieceOn(IndexOf(r, f));
                    if (piece != null && predicate(piece)) pieces.Add(piece);
                }
            }

            return pieces;
        }

        public static List<PieceLogic> FindPiece<T>(bool side) where T : PieceLogic
        {
            return PieceBoard().Where(piece => piece is T && piece.Color == side).ToList();
        }

        public static List<PieceLogic> FindAllies(bool side)
        {
            return PieceBoard().Where(piece => piece != null && piece.Color == side).ToList();
        }

        public static RelicLogic GetRelicOf(bool side)
        {
            return !side ? MatchManager.Ins.GameState.Relics.Item1 : MatchManager.Ins.GameState.Relics.Item2;
        }

        public static PieceLogic GetCommanderOf(bool side)
        {
            return side ? MatchManager.Ins.GameState.Commanders.Item1 : MatchManager.Ins.GameState.Commanders.Item2;
        }

        public static Vector3 FromRankFileToWorldPos(float rank, float file)
        {
            return new Vector3(rank, YCoordinate, file);
        }

        public static bool IsNextEachOther(PieceLogic piece)
        {
            var pos = piece.Pos;
            for (var i = -1; i <= 1; i++)
            for (var j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                var indexOff = IndexOf(RankOf(pos) + i, FileOf(pos) + j);

                if (!VerifyIndex(indexOff)) continue;
                var pieceOff = PieceOn(indexOff);
                if (pieceOff != null && pieceOff.Color == piece.Color) return true;
            }

            return false;
        }

        // Return a list of white or black pieces with a specific effect.
        // Use this function even when you want to grab a number of effects by using .Count. 
        public static List<PieceLogic> FindPiecesWithEffectName(bool side, string effectName)
        {
            return MatchManager.Ins?.GameState?.PieceBoard?
                       .Where(piece =>
                           piece != null &&
                           piece.Color == side &&
                           piece.Effects != null &&
                           piece.Effects.Any(effect => effect != null && effect.EffectName == effectName)
                       )
                       .ToList()
                   ?? new List<PieceLogic>();
        }


        public static List<Effect> EffectWithEffectCategory(PieceLogic piece, EffectCategory effectCategory)
        {
            return piece.Effects.Where(effect => effect.Category == effectCategory).ToList();
        }

        public static List<T> GetEffectHookList<T>()
        {
            return MatchManager.Ins.GameState.TriggerHooks.GetList<T>().ToList();
        }

        public static void NotifyOnMoveGen(PieceLogic caller, List<Action.Action> list)
        {
            MatchManager.Ins.GameState.TriggerHooks.NotifyOnMoveGen(caller, list);
        }

        public static void NotifyBeforePieceAction(Action.Action action)
        {
            MatchManager.Ins.GameState.TriggerHooks.NotifyBeforePieceAction(action);
        }

        public static void NotifyBeforeRelicAction(IRelicAction action)
        {
            MatchManager.Ins.GameState.TriggerHooks.NotifyBeforeRelicAction(action);
        }

        public static void NotifyInternalAction(IInternal action)
        {
            switch (action)
            {
                case ApplyEffect apply:
                    MatchManager.Ins.GameState.TriggerHooks.NotifyWhenApplyEffect(apply);
                    break;
                case Game.Action.Internal.KillPiece:
                case DestroyPiece:
                case CarapaceKill:
                case DestroyAdhesivePiece:
                case DestroyParasitePiece:
                    MatchManager.Ins.GameState.TriggerHooks.NotifyBeforeDestroyOrKill(action);
                    break;
            }
        }

        public static bool IsAlive(Entity entity)
        {
            if (entity == null) return true;
            if (entity.Pos == -9999) return true;
            return entity switch
            {
                PieceLogic pieceLogic => PieceOn(pieceLogic.Pos) == pieceLogic,
                Formation formation => GetFormation(formation.Pos) == formation,
                _ => false
            };
        }

        public static List<int> AllSidePos(bool side)
        {
            var positions = new List<int>();
            if (side)
                for (var rank = MaxLength / 2; rank > 0; rank--)
                {
                    if (!VerifyBounds(rank)) continue;
                    for (var file = 0; file < MaxLength; file++)
                    {
                        if (!VerifyBounds(file) || PieceOn(IndexOf(rank, file)) != null) continue;
                        positions.Add(IndexOf(rank, file));
                    }
                }
            else
                for (var rank = MaxLength / 2; rank < MaxLength; rank++)
                {
                    if (!VerifyBounds(rank)) continue;
                    for (var file = 0; file < MaxLength; file++)
                    {
                        if (!VerifyBounds(file) || PieceOn(IndexOf(rank, file)) != null) continue;
                        positions.Add(IndexOf(rank, file));
                    }
                }

            return positions;
        }

        public static List<PieceLogic> FindAllAlliesInEnemyHalf(bool side)
        {
            var list = new List<PieceLogic>();
            var startingSize = MatchManager.Ins.StartingSize;
            var rankStart = (MaxLength - startingSize.x) / 2;
            var fileStart = (MaxLength - startingSize.y) / 2;
            var midRank = rankStart + startingSize.x / 2;

            if (!side) // white: enemy half = ranks >= midRank
                for (var r = midRank; r < rankStart + startingSize.x; r++)
                for (var f = fileStart; f < fileStart + startingSize.y; f++)
                {
                    var idx = IndexOf(r, f);
                    var p = PieceOn(idx);
                    if (p != null) list.Add(p);
                }
            else // black: enemy half = ranks < midRank
                for (var r = rankStart; r < midRank; r++)
                for (var f = fileStart; f < fileStart + startingSize.y; f++)
                {
                    var idx = IndexOf(r, f);
                    var p = PieceOn(idx);
                    if (p != null) list.Add(p);
                }

            return list;
        }

        public static List<(int rank, int file)> GetEmptySquaresRankFile()
        {
            var result = new List<(int, int)>();
            var board = PieceBoard();

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == null)
                {
                    result.Add((RankOf(i), FileOf(i)));
                }
            }

            return result;
        }

        public static bool IsOnBlackSide(int pos)
        {
            return RankOf(pos) <= BoardSize / 2 - 1;
        }

        [Mutator]
        public static void SetFormation(int pos, Formation env)
        {
            FormationManager.Ins.SetFormation(pos, env);
        }

        [Mutator]
        public static void MoveFormation(int from, int to)
        {
            FormationManager.Ins.MoveFormation(from, to);
        }

        [Mutator]
        public static void RemoveFormation(Formation formation)
        {
            FormationManager.Ins.RemoveFormation(formation);
        }

        public static List<Formation> GetFormation(FormationType type)
        {
            return GetFormations().Where(f => f != null && f.GetFormationType() == type).ToList();
        }

        public static Formation[] GetFormations()
        {
            return MatchManager.Ins.GameState.Formations;
        }

        public static Formation GetFormation(int pos)
        {
            return MatchManager.Ins.GameState.Formations[pos];
        }

        public static bool HasFormation(int pos)
        {
            return MatchManager.Ins.GameState.Formations[pos] != null;
        }

        [Mutator]
        public static void DestroyTile(int index)
        {
            TileManager.Ins.DestroyTile(index);
            RemoveFormation(GetFormation(index));
            var on = PieceOn(index);
            if (on != null) ActionManager.EnqueueAction(new KillPiece(null, on));
        }

        public static bool IsDay() => MatchManager.Ins.GameState.IsDay;

        [Mutator]
        public static PieceLogic SpawnPiece(PieceConfig pieceConfig)
        {
            return MatchManager.Ins.GameState.SpawnPiece(pieceConfig);
        }

        public static int NextEntityID()
        {
            return MatchManager.Ins.GameState.NextEntityID();
        }

        public static Entity GetEntityByID(int id)
        {
            return MatchManager.Ins.GameState.GetEntityByID(id);
        }

        public static void AddToEntityList(Entity entity)
        {
            MatchManager.Ins.GameState.AddToEntityList(entity);
        }

        [Mutator]
        public static void KillPiece(PieceLogic pieceLogic, bool record = true)
        {
            MatchManager.Ins.GameState.Kill(pieceLogic, record);
        }

        public static void Prune()
        {
            MatchManager.Ins.GameState.PruneEntityList();
        }

        public static bool GetSideToMove()
        {
            return MatchManager.Ins.GameState.SideToMove;
        }

        public static int GetCurrentTurn()
        {
            return MatchManager.Ins.GameState.CurrentTurn;
        }

        [Mutator]
        public static void SetRelic(bool color, RelicLogic relic)
        {
            if (!color)
            {
                MatchManager.Ins.GameState.Relics.Item1 = relic;
            }
            else MatchManager.Ins.GameState.Relics.Item2 = relic;
        }

        [Mutator]
        public static void Swap(PieceLogic a, PieceLogic b)
        {
            MatchManager.Ins.GameState.Swap(a, b);
        }

        public static FieldEffectType GetFieldEffectType()
        {
            return MatchManager.Ins.GameState.FieldEffect.Type;
        }

        public static TriggerHooks GetTriggerHooks()
        {
            return MatchManager.Ins.GameState.TriggerHooks;
        }

        public static List<PieceLogic> GetAllPieces()
        {
            return MatchManager.Ins.GameState.EntityDict.Where(entity => entity.Value is PieceLogic)
                .Select(e => (PieceLogic)e.Value)
                .ToList();
        }
    }
}