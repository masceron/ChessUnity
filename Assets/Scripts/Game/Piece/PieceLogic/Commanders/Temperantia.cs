using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Action.Skills;
using Game.Relics;
using System.Collections.Generic;
using static Game.Common.BoardUtils;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Temperantia: PieceLogic, IRelicCarriable, IPieceWithSkill
    {
        public Temperantia(PieceConfig cfg, RelicLogic carriedRelic = null) : base(cfg, TemperantiaMoves.Quiets, TemperantiaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
            UndyingDevotion trait = new UndyingDevotion(this);
            ActionManager.ExecuteImmediately(new ApplyEffect(trait));

            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                Debug.Log("Temperantia: choose a ally");
                List<PieceLogic> allies = FindPiece<PieceLogic>(Color);
                foreach(PieceLogic ally in allies){
                    if (ally.Equals(this)) { continue; }
                    list.Add(new TemperantiaSwap(Pos, ally.Pos));
                }
                
            };
            CarriedRelic = carriedRelic;
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
        public RelicLogic CarriedRelic { get; set; }
    }
}