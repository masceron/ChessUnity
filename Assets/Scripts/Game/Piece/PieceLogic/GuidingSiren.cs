using System;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuidingSiren: PieceLogic, IPieceWithSkill, IRelicCarriable
    {
        public GuidingSiren(PieceConfig cfg, RelicLogic carriedRelic = null) : base(cfg, GuidingSirenMoves.Quiets, GuidingSirenMoves.Captures)
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

            CarriedRelic = carriedRelic;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public RelicLogic CarriedRelic { get; set; }
    }
}