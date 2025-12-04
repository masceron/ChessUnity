using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Temperantia : Commons.PieceLogic, IPieceWithSkill
    {
        public Temperantia(PieceConfig cfg) : base(cfg, TemperantiaMoves.Quiets, TemperantiaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
            var trait = new UndyingDevotion(this);
            ActionManager.ExecuteImmediately(new ApplyEffect(trait));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0 || FindPiece<Commons.PieceLogic>(!Color).Count == 0) return;
                if (isPlayer)
                {
                    Debug.Log("Temperantia: choose a ally");
                    var allies = FindPiece<Commons.PieceLogic>(Color);
                    list.AddRange(from ally in allies where !ally.Equals(this) select new TemperantiaSwap(Pos, ally.Pos));
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