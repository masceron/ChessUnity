using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Traits
{
    public class Ambush: Effect
    {
        private byte lastUsed;
        private bool active;
        private const sbyte RangeOffset = 1;

        public Ambush(sbyte duration, PieceLogic piece) : base(duration, -1, piece, EffectType.Ambush)
        {}

        public override void OnCall(Action.Action action)
        {
            if (action == null) return;
            if (action.Caller != Piece.pos)
            {
                lastUsed++;
                if (lastUsed < 6 || active) return;
                active = true;
                Piece.attackRange += RangeOffset;
            }
            else if (active)
            {
                active = false;
                lastUsed = 0;
                Piece.attackRange -= RangeOffset;
            }
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }

        public override void OnRemove()
        {
            if (active) Piece.attackRange -= RangeOffset;
        }
    }
}