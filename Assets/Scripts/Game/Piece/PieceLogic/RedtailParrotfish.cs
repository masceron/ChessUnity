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
    public class RedtailParrotfish : Commons.PieceLogic, IPieceWithSkill
    {
        public RedtailParrotfish(PieceConfig cfg) : base(cfg, ElectricEelMoves.Quiets, ElectricEelMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new RedtailParrotfishPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0){ return; }
                if (isPlayer)
                {
                    for(int i = 0; i < BoardSize; ++i)
                    {
                        if (!IsActive(i)) { continue; }
                        Formation formation = GetFormation(i);
                        if (formation != null)
                        {
                            list.Add(new RedtailParrotfishActive(this, i));
                        }
                    }
                }
                else
                {
                    // ....
                    Debug.Log("AI");
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}

