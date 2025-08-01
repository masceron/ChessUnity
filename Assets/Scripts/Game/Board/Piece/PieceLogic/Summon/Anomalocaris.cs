using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Effects.Buffs;
using Game.Board.General;
using static Game.Common.BoardUtils;
using SnappingStrike = Game.Board.Effects.Traits.SnappingStrike;

namespace Game.Board.Piece.PieceLogic.Summon
{
    public class Anomalocaris: PieceLogic
    {
        public Anomalocaris(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
        }

        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            var gameState = MatchManager.Ins.GameState;
            if (!gameState.ActiveBoard[index]) return false;
            var pieceOn = gameState.PieceBoard[index];
            if (pieceOn != null)
            {
                if (pieceOn.Color != Color && distance <= AttackRange)
                {
                    list.Add(new Action.Captures.SnappingStrike(Pos, index));
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
            if (SkillCooldown == 0)
            {
                
            }
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
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
            
            for (int rankOff = rank - 1, fileOff = file - 1;
                 rankOff >= 0 && fileOff >= 0 && rank - rankOff <= maxRange && file - fileOff <= maxRange;
                 rankOff--, fileOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank - 1, fileOff = file + 1;
                 rankOff >= 0 && VerifyUpperBound(fileOff) && rank - rankOff <= maxRange && fileOff - file <= maxRange;
                 rankOff--, fileOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank + 1, fileOff = file + 1;
                 VerifyBounds(rankOff) && VerifyUpperBound(fileOff) && rankOff - rank <= maxRange && fileOff - file <= maxRange;
                 rankOff++, fileOff++)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            for (int rankOff = rank + 1, fileOff = file - 1;
                 VerifyBounds(rankOff) && fileOff >= 0 && rankOff - rank <= maxRange && file - fileOff <= maxRange;
                 rankOff++, fileOff--)
            {
                if (!MakeMove(list, IndexOf(rankOff, fileOff), rank - rankOff)) break;
            }
            
            Skill(list);

            return list;
        }
    }
}