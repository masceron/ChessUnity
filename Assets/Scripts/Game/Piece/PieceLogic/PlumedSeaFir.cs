using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Effects.SpecialAbility;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PlumedSeaFir : Commons.PieceLogic, IPieceWithSkill
    {
        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public PlumedSeaFir(PieceConfig cfg) : base(cfg, RangerMove.Quiets, PawnPushMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PlumedSeaFirPassive(this)));
            SetStat(SkillStat.Range, 4);
            SetStat(SkillStat.Counter, 0);
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) { return; }
                if (isPlayer)
                {
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), GetStat(SkillStat.Range)))
                    {
                        int target = IndexOf(rankOff, fileOff);
                        Commons.PieceLogic pieceOn = GetTarget();
                        if (pieceOn != null)
                        {
                            if (pieceOn.Color == this.Color)
                            {
                                list.Add(new PlumedSeaFirActive(Pos, target));
                            }
                            else if (pieceOn.Color != this.Color && GetStat(SkillStat.Counter) > 0)
                            {
                                list.Add(new PlumedSeaFirActive(Pos, target));
                            }
                        }
                    }
                }
                else
                {
                    // ....
                }
            };
        }
    }
}