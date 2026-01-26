using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class SurgeWrasse : Commons.PieceLogic, IPieceWithSkill
    {
        public SurgeWrasse(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, FlyingFishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Dominator(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new SurgeWrassePassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0){ return; }
                if (isPlayer)
                {
                    list.Add(new SurgeWrasseActive(this.Pos));
                }
                else
                {
                    // ....
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}

