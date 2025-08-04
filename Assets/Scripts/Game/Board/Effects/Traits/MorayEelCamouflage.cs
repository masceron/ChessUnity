using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Buffs;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    public class MorayEelCamouflage: Effect, IEndTurnEffect
    {
        private Camouflage already;

        public MorayEelCamouflage(PieceLogic piece) : base(-1, 1, piece, EffectName.MorayEelCamouflage)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action action)
        {
            var rank = RankOf(Piece.Pos);
            var file = FileOf(Piece.Pos);
            
            if (rank >= 1)
            {
                var idx = IndexOf(rank - 1, file);
                if (!IsActive(idx))
                {
                    if (already != null) return;
                    
                    already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(already));
                    return;
                }
            }
            
            if (rank < MaxLength - 1)
            {
                var idx = IndexOf(rank + 1, file);
                if (!IsActive(idx))
                {
                    if (already != null) return;
                    
                    already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(already));
                    return;
                }
            }
            
            if (file >= 1)
            {
                var idx = IndexOf(rank, file - 1);
                if (!IsActive(idx))
                {
                    if (already != null) return;
                    
                    already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(already));
                    return;
                }
            }
            
            if (file < MaxLength - 1)
            {
                var idx = IndexOf(rank, file + 1);
                if (!IsActive(idx))
                {
                    if (already != null) return;
                    
                    already = new Camouflage(Piece);
                    ActionManager.EnqueueAction(new ApplyEffect(already));
                    return;
                }
            }

            if (already == null) return;
            
            ActionManager.EnqueueAction(new RemoveEffect(already));
            already = null;
        }

        public override string Description()
        {
            return AssetManager.Ins.EffectData[EffectName].description;
        }
    }
}