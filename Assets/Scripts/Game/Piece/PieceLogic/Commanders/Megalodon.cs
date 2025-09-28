using System;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Relics;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic.Commanders
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Megalodon: PieceLogic, IRelicCarriable
    {
        public Megalodon(PieceConfig cfg, RelicLogic carriedRelic = null) : base(cfg, RookMoves.Quiets,
            MegalodonMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrenziedVeteran(this)));
            //ActionManager.ExecuteImmediately(new ApplyEffect(new (this)));
        }
        
        //sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        //public SkillsDelegate Skills { get; set; }
        public RelicLogic CarriedRelic { get; set; }



    }
    
}