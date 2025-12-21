using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirensHarpoon : RelicLogic
    {
        public SirensHarpoon(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
            currentCooldown = 0;
        }
        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;


                    if (piece.Color != Color && piece.PieceRank <= PieceRank.Common)
                    {
                        TileManager.Ins.MarkAsMoveable(piece.Pos);
                        var pending = new SirensHarpoonPending(this, piece.Pos, piece.Color);
                        BoardViewer.ListOf.Add(pending);
                    }
                    BoardViewer.Selecting = -2;
                    BoardViewer.SelectingFunction = 4;
                }
            }
        }

        public override void ActiveForAI()
        {
            var listPieces = new List<PieceLogic>();

            for (var i = 0; i < BoardUtils.BoardSize; ++i)
            {
                var piece = BoardUtils.PieceOn(i);
                if (piece == null || piece.Color == Color) continue;
                if (piece.PieceRank > PieceRank.Common) continue;

                if (piece.Effects.Any(e => (
                        e.EffectName == "effect_extremophile" || e.EffectName == "effect_sanity"))
                   ) continue;

                listPieces.Add(piece);
            }

            if (listPieces.Count == 0) return;
            
            listPieces.Sort((a, b) => 
                b.GetValueForAI().CompareTo(a.GetValueForAI())
            );
            
            int topValue = listPieces[0].GetValueForAI();
            var topGroup = listPieces.Where(p => p.GetValueForAI() == topValue).ToList();
            int idx = UnityEngine.Random.Range(0, topGroup.Count);
            
            var pending = new SirensHarpoonPending(this, topGroup[idx].Pos, topGroup[idx].Color);
            if (pending is IPendingAble p)
            {
                p.CompleteAction();
            }
            else
            {
                BoardViewer.Ins.ExecuteAction(pending);
            }
        }
    }
}