using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Siphonophore : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Radius = 8;
        private const int Unit = 3;

        public Siphonophore(PieceConfig cfg) : base(cfg, RangerMove.Quiets, RangerMove.Captures)
        {
            SetStat(SkillStat.Range, Radius);
            SetStat(SkillStat.Unit, Unit);
            ActionManager.ExecuteImmediately(new ApplyEffect(new SiphonophorePassive(this)));

            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), GetStat(SkillStat.Range)))
                    {
                        var idx = IndexOf(rank, file);
                        if (!VerifyIndex(idx) || !IsActive(idx)) continue;
                        var pOn = PieceOn(idx);
                        //Làm lại
                        //if (pOn == null) list.Add(new SiphonophorePending(this, idx));
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}