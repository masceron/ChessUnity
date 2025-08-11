using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Velkaris : PieceLogic, IPieceWithSkill
    {
        public PieceLogic Marked;

        public Velkaris(PieceConfig cfg) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            Marked = null;
            SkillCooldown = -1;
            ActionManager.ExecuteImmediately(new ApplyEffect(new VelkarisMarker(this)));
            
            Skills = list =>
            {
                if (SkillCooldown == 0 && Marked != null)
                {
                    list.Add(new VelkarisKill(Pos, Pos, Marked.Pos));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}