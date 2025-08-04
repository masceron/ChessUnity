using System;
using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Captures;
using Game.Board.Action.Internal;
using Game.Board.Action.Quiets;
using Game.Board.Action.Skills;
using Game.Board.Effects.Traits;
using static Game.Common.BoardUtils;

namespace Game.Board.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic, IPieceWithSkill
    {
        public GuidingSiren(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SirenDebuffer(this)));
        }
        
        
        private void MakeMove(List<Action.Action> list, int trank, int file, int curr)
        {
            if (!VerifyBounds(trank) || !VerifyBounds(file)) return;
            
            var tpos = IndexOf(trank, file);
            if (!IsActive(tpos)) return;

            var p = PieceOn(tpos);
            if (p == null)
            {
                if (curr <= EffectiveMoveRange)
                    list.Add(new NormalMove(Pos, tpos));
            }
            else if (p.Color != Color && curr <= AttackRange)
            {
                list.Add(new NormalCapture(Pos, tpos));
            }
        }

        private void SirenActive(List<Action.Action> list, int trank, int file)
        {
            var push = !Color ? 1 : -1;
            for (var i = -6; i <= 6; i++)
            {
                var rankOff = trank + i;
                if (!VerifyBounds(rankOff)) continue;
                for (var j = -6; j <= 6; j++)
                {
                    var fileOff = file + j;
                    if (!VerifyBounds(fileOff)) continue;
                    
                    var tpos = IndexOf(rankOff, fileOff);
                    var pieceAt = PieceOn(tpos);
                    if (pieceAt == null || pieceAt.Color == Color) continue;
                    
                    var rankForce = rankOff + push;
                    while (Math.Abs(rankForce - rankOff) <= 3 &&
                           VerifyBounds(rankForce) &&
                           PieceOn(IndexOf(rankForce, fileOff)) == null &&
                           IsActive(IndexOf(rankForce, fileOff)))
                    {
                         rankForce += push;
                    }
                    rankForce -= push;
                    if (rankForce == rankOff) continue;
                    list.Add(new SirenActive(Pos, tpos, IndexOf(rankForce, fileOff)));
                }
            }
        }

        protected override void MoveToMake(List<Action.Action> list)
        {
            var (trank, file) = RankFileOf(Pos);
            
            for (var i = 1; i <= Math.Max(AttackRange, EffectiveMoveRange); i++)
            {
                MakeMove(list, trank + i, file, i);
                MakeMove(list, trank - i, file, i);
                MakeMove(list, trank, file + i, i);
                MakeMove(list, trank, file - i, i);
                MakeMove(list, trank + i, file + i, i);
                MakeMove(list, trank - i, file + i, i);
                MakeMove(list, trank + i, file - i, i);
                MakeMove(list, trank - i, file - i, i);
            }

            if (SkillCooldown == 0)
            {
                SirenActive(list, trank, file);
            }
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
    }
}