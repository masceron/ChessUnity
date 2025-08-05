using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using static Game.Common.BoardUtils;
using SnappingStrike = Game.Action.Captures.SnappingStrike;

namespace Game.Piece.PieceLogic.Champions
{
    public class Swordfish: PieceLogic, IPieceWithSkill
    {
        public Swordfish(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SwordfishAttack(this)));
        }

        private bool snap;

        private bool MakeMove(List<Action.Action> list, int index, int distance)
        {
            if (!IsActive(index)) return false;
            var pieceOn = PieceOn(index);
            if (pieceOn != null)
            {
                if (pieceOn.Color == Color || distance > AttackRange) return false;
                
                if (snap)
                {
                    list.Add(new SnappingStrike(Pos, index));
                }
                else list.Add(new NormalCapture(Pos, index));

                return false;
            }

            if (distance <= EffectiveMoveRange)
            {
                list.Add(new NormalMove(Pos, index));
            }

            return true;
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var rank = RankOf(Pos);
            var file = FileOf(Pos);
            var maxRange = Math.Max(AttackRange, EffectiveMoveRange);
            snap = Effects.Any(e => e.EffectName == EffectName.SnappingStrike);
            
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
            
            if (SkillCooldown == 0) list.Add(new SwordFishActive(Pos));
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}