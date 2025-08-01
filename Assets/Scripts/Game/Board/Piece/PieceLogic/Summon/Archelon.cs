using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Buffs;
using Game.Board.Effects.Traits;
using static Game.Common.BoardUtils;
using static Game.Board.General.MatchManager;

namespace Game.Board.Piece.PieceLogic.Summon
{
    public class Archelon: PieceLogic
    {
        public Archelon(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(-1, this, 3)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ArchelonDraw(this)));
        }

        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!gameState.ActiveBoard[index]) return false;
            var pieceOn = gameState.PieceBoard[index];
            if (pieceOn != null)
            {
                if (pieceOn.Color != Color && distance <= AttackRange)
                {
                    list.Add(new NormalCapture(Pos, index));
                }

                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        private void Skill(List<Action.Action> list)
        {
            if (SkillCooldown != 0) return;
            
            var (rank, file) = RankFileOf(Pos);
            
            for (var r = ClampUp(rank - 3); r <= ClampDown(rank + 3); r++)
            {
                var rowIndex = RowIndex(r);
                for (var f = ClampUp(file - 3); f <= ClampDown(file + 3); f++)
                {
                    var index = rowIndex + f;
                    var pOn = gameState.PieceBoard[index];
                    if (pOn == null || pOn == this) continue;
                    if (pOn.Color == Color)
                    {
                        list.Add(new ArchelonShield(Pos, index));
                    }
                }
            }
        }

        private void Moves(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);

            for (var rankOff = rank - 1; rankOff >= 0 && rank - rankOff <= maxRange; rankOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, file), rank - rankOff)) break;
            }

            for (var rankOff = rank + 1; VerifyUpperBound(rankOff) && rankOff - rank <= maxRange; rankOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, file), rankOff - rank)) break;
            }

            for (var fileOff = file - 1; fileOff >= 0 && file - fileOff <= maxRange; fileOff--)
            {
                if (!MakeMove(list, IndexOf(rank, fileOff), file - fileOff)) break;
            }

            for (var fileOff = file + 1; VerifyUpperBound(fileOff) && fileOff - file <= maxRange; fileOff++)
            {
                if (!MakeMove(list, IndexOf(rank, fileOff), fileOff - file)) break;
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            Moves(list);
            Skill(list);

            return list;
        }
    }
}