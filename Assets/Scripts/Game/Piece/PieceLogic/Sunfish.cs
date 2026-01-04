using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Common;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Sunfish : Commons.PieceLogic, IPieceWithSkill
    {
        public Sunfish(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Shield(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SunfishPassive(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    if (SkillCooldown == 0)
                    {

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 4))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            if (!VerifyIndex(index) || !IsActive(index)) continue;
                            var pending = new SunfishActive(Pos, index);
                            list.Add(pending);
                        }
                    }

                }
                else
                {
                    //query for AI in here
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
