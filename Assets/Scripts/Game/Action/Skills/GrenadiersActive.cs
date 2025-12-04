using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Tile;
using Game.AI;


namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GrenadiersActive: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -60 : 0;
        }

        public GrenadiersActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            FormationManager.Ins.SetFormation(Target, new NavalMines(true, PieceOn(Maker).Color));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
    }
}