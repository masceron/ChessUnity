using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Traits;
using Game.Board.General;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Velkaris: PieceLogic
    {
        public PieceLogic Marked;

        public Velkaris(PieceConfig cfg) : base(cfg)
        {
            Marked = null;
            SkillCooldown = -1;
            ActionManager.ExecuteImmediately(new ApplyEffect(new VelkarisMarker(this)));
        }

        protected override List<Action.Action> MoveToMake()
        {
            var gameState = MatchManager.Ins.GameState;
            
            var (rank, file) = RankFileOf(Pos);
            
            var list = new List<Action.Action>();

            var totalRange = Math.Max(EffectiveMoveRange, AttackRange);
            
            for (var rankOff = 1; rankOff <= totalRange; rankOff++)
            {
                var rankAfter = rank + rankOff;
                if (!VerifyUpperBound(rankAfter)) break;
                var newPos = IndexOf(rankAfter, file);
                
                if (!gameState.ActiveBoard[newPos]) break;
                var p = gameState.PieceBoard[newPos];
                if (p != null)
                {
                    if (p.Color != Color && rankOff <= AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (rankOff <= EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            for (var rankOff = -1; rankOff >= -totalRange; rankOff--)
            {
                var rankAfter = rank + rankOff;
                if (rankAfter < 0) break;
                var newPos = IndexOf(rankAfter, file);
                
                if (!gameState.ActiveBoard[newPos]) break;
                var p = gameState.PieceBoard[newPos];
                if (p != null)
                {
                    if (p.Color != Color && rankOff >= -AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (rankOff >= -EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            for (var fileOff = 1; fileOff <= totalRange; fileOff++)
            {
                var fileAfter = file + fileOff;
                if (!VerifyUpperBound(fileAfter)) break;
                var newPos = IndexOf(rank, fileAfter);
                
                if (!gameState.ActiveBoard[newPos]) break;
                var p = gameState.PieceBoard[newPos];
                if (p != null)
                {
                    if (p.Color != Color && fileOff <= AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (fileOff <= EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            for (var fileOff = -1; fileOff >= -totalRange; fileOff--)
            {
                var fileAfter = file + fileOff;
                if (fileAfter < 0) break;
                var newPos = IndexOf(rank, fileAfter);
                
                if (!gameState.ActiveBoard[newPos]) break;
                var p = gameState.PieceBoard[newPos];
                if (p != null)
                {
                    if (p.Color != Color && fileOff >= -AttackRange) list.Add(new NormalCapture(Pos, newPos));
                    break;
                }
                if (fileOff >= -EffectiveMoveRange)
                {
                    list.Add(new NormalMove(Pos, newPos));
                }
            }
            
            if (SkillCooldown == 0 && Marked != null)
            {
                list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
            }

            return list;
        }
    }
}