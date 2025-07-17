using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Kills;
using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.Piece;

namespace Game.Board.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Velkaris: PieceLogic
    {
        public PieceLogic Marked;
        public sbyte SkillCooldown;

        public Velkaris(PieceConfig cfg) : base(cfg)
        {
            Marked = null;
            SkillCooldown = 0;
            ActionManager.ExecuteImmediately(new ApplyEffect(new VelkarisMarker(this)));
        }

        protected override List<Action.Action> MoveToMake()
        {
            var gameState = InteractionManager.GameState;

            var maxLength = MatchManager.MaxLength;
            
            var rank = pos / maxLength;
            var file = pos % maxLength;
            
            var list = new List<Action.Action>();

            var totalRange = Math.Max(moveRange, attackRange);
            
            for (var rankOff = 1; rankOff <= totalRange; rankOff++)
            {
                var rankAfter = rank + rankOff;
                if (rankAfter >= maxLength) break;
                var newPos = rankAfter * maxLength + file;
                var p = gameState.MainBoard[newPos];
                if (p != null)
                {
                    if (p.color != color && rankOff <= attackRange) list.Add(new NormalCapture(pos, pos, newPos));
                    break;
                }
                if (rankOff <= moveRange)
                {
                    list.Add(new NormalMove(pos, pos, newPos));
                }
            }
            
            for (var rankOff = -1; rankOff >= -totalRange; rankOff--)
            {
                var rankAfter = rank + rankOff;
                if (rankAfter < 0) break;
                var newPos = rankAfter * maxLength + file;
                var p = gameState.MainBoard[newPos];
                if (p != null)
                {
                    if (p.color != color && rankOff >= -attackRange) list.Add(new NormalCapture(pos, pos, newPos));
                    break;
                }
                if (rankOff >= -moveRange)
                {
                    list.Add(new NormalMove(pos, pos, newPos));
                }
            }
            
            for (var fileOff = 1; fileOff <= totalRange; fileOff++)
            {
                var fileAfter = file + fileOff;
                if (fileAfter >= maxLength) break;
                var newPos = rank * maxLength + fileAfter;
                var p = gameState.MainBoard[newPos];
                if (p != null)
                {
                    if (p.color != color && fileOff <= attackRange) list.Add(new NormalCapture(pos, pos, newPos));
                    break;
                }
                if (fileOff <= moveRange)
                {
                    list.Add(new NormalMove(pos, pos, newPos));
                }
            }
            
            for (var fileOff = -1; fileOff >= -totalRange; fileOff--)
            {
                var fileAfter = file + fileOff;
                if (fileAfter < 0) break;
                var newPos = rank * maxLength + fileAfter;
                var p = gameState.MainBoard[newPos];
                if (p != null)
                {
                    if (p.color != color && fileOff >= -attackRange) list.Add(new NormalCapture(pos, pos, newPos));
                    break;
                }
                if (fileOff >= -moveRange)
                {
                    list.Add(new NormalMove(pos, pos, newPos));
                }
            }
            
            if (SkillCooldown == -1 && Marked != null)
            {
                list.Add(new VelkarisKill(pos, pos, Marked.pos));
            }

            return list;
        }
    }
}