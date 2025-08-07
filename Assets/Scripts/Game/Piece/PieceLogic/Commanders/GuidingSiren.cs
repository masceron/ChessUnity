using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic, IPieceWithSkill
    {
        public GuidingSiren(PieceConfig cfg) : base(cfg, GuidingSirenMoves.Quiets, GuidingSirenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SirenDebuffer(this)));
            Skills = list =>
            {
                var (trank, file) = RankFileOf(Pos);

                if (SkillCooldown != 0) return;
            
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
            };
        }
        
        protected override void MoveToMake(List<Action.Action> list)
        {
            Quiets(list, Pos);
            Captures(list, Pos);
            Skills(list);
            
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}