using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Megalodon: Commons.PieceLogic, IRelicCarriable
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