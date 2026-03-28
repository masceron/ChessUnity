using Game.Action.Skills;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class MirrorButterflyFishPassive : Effect, IBeforePieceActionTrigger
    {
        private const int Chance = 25;

        public MirrorButterflyFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_mirror_butterfly_fish_passive")
        {
        }


        public BeforeActionPriority Priority => BeforeActionPriority.Redirection;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ISkills)
                return;

            var maker = action.GetMaker();
            var target = action.GetTarget();

            if (target != Piece)
                return;

            if (maker == null || !MatchManager.Roll(Chance))
                return;

            action.Target = maker.Pos;
        }
    }
}