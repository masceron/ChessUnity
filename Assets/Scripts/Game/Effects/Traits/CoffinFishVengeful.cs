using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Condition;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CoffinFishVengeful : Vengeful
    {
        private int probability;
        public CoffinFishVengeful(PieceLogic piece, int probability) : base(piece, VengefulType.OnDeath, "effect_coffin_fish_vengeful")
        {
            this.probability = probability;
        }

        // public void OnCallAfterPieceAction(Action.Action action)
        // {
        //     if (action is not ICaptures || action.Result != ResultFlag.Success) return;

        //     if (!MatchManager.Ins.Roll(Strength)) return;

        //     if (action.Target == Piece.Pos)
        //         ActionManager.EnqueueAction(new ApplyEffect(new Relentless(BoardUtils.action.GetMakerAsPiece(), 1)));
        // }

        /// <summary>
        /// có 25% cơ hội Gây hiệu ứng Relentless 1 lên 1 quân đồng minh
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            var allies = BoardUtils.FindAllies(Piece.Color);
            if (allies.Count == 0) return;
            if (!MatchManager.Ins.Roll(probability)) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Relentless(allies[UnityEngine.Random.Range(0, allies.Count)], 1)));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }
    }
}