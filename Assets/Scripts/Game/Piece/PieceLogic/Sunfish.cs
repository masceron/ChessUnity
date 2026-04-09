using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Sunfish : Commons.PieceLogic, IPieceWithSkill
    {
        public Sunfish(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SunfishPassive(this)));

            //Làm lại
            Skills = (list, isPlayer, _) =>
            {
                if (SkillCooldown > 0) return;
                // if (isPlayer)
                //     if (SkillCooldown == 0)
                //         foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 4))
                //         {
                //             var index = IndexOf(rankOff, fileOff);
                //             if (!VerifyIndex(index) || !IsActive(index)) continue;
                //             var pending = new SunfishActive(this, index);
                //             list.Add(pending);
                //         }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}