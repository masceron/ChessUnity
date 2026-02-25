using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using Unity.Mathematics;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class ThreadPipefishEffect : Effect, IEndTurnTrigger, IOnApplyTrigger, IOnRemoveTrigger
    {
        private const int MaxDistance = 3;
        private readonly PieceLogic Target;
        private int _distance; // distance between 2 connected pieces

        public ThreadPipefishEffect(PieceLogic piece, PieceLogic target) : base(-1, -1, piece,
            "effect_thread_pipefish_effect")
        {
            Target = target;
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            var posMaker = Piece.Pos;
            var posTarget = Target.Pos;
            _distance = math.max(math.abs(RankOf(posMaker) - RankOf(posTarget)),
                math.abs(FileOf(posMaker) - FileOf(posTarget)));

            //Debug.Log(distance);
            if (_distance > MaxDistance) ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(-1, Target)));
            ActionManager.EnqueueAction(new ApplyEffect(new Piercing(-1, Target)));
            ActionManager.EnqueueAction(new ApplyEffect(new Momentum(-1, Target)));
        }

        public void OnRemove()
        {
            foreach (var effect in Target.Effects)
            {
                var name = effect.EffectName;
                if (name.Equals("effect_true_bite") || name.Equals("effect_piercing") || name.Equals("effect_momentum"))
                    ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        }
    }
}