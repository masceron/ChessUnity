using System.Collections.Generic;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;

namespace Game.Board.Effects.Debuffs
{
    public class Stunned: Effect
    {
        public Stunned(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Stunned)
        {}

        public override string Description()
        {
            return string.Format(AssetManager.Ins.EffectData[EffectName].description, Duration);
        }

        public override List<Action.Action> OnCall(List<Action.Action> actions)
        {
            if (actions.Count == 0 || actions[0].Caller != Piece.Pos) return actions;
            
            actions.Clear();
            return actions;
        }
    }
}