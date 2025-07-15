using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Others;
using Game.Board.Piece;

namespace Game.Board.PieceLogic.Elites
{
    public class ElectricEel: PieceLogic
    {
        public sbyte SkillCooldown; 
        
        public ElectricEel(PieceConfig cfg) : base(cfg)
        {
            SkillCooldown = 0;
            ActionManager.ExecuteImmediately(new ApplyEffect(new Vengeful(4, this)));
        }

        public override void PassTurn()
        {
            if (SkillCooldown > 0) SkillCooldown--;
        }

        private void Quiets(List<Action.Action> list)
        {
            
        }

        private void Captures(List<Action.Action> list)
        {
            
        }

        private void Skills(List<Action.Action> list)
        {
            
        }

        protected override List<Action.Action> MoveToMake()
        {
            var list = new List<Action.Action>();
            Quiets(list);
            Captures(list);
            Skills(list);

            return list;
        }
    }
}