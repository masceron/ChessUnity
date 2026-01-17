// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs

using Game.Effects.Buffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Action.Relics;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Common;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MethaneCasingPending : Action, System.IDisposable, IRelicAction
    {
        private MethaneCasing methaneCasing;

        public MethaneCasingPending(MethaneCasing methaneCasing, int maker) : base(maker)
        {
            this.methaneCasing = methaneCasing;
            Maker = (ushort)maker;
        }

        protected override void ModifyGameState()
        {
            PieceLogic pieceOn = PieceOn(Maker);
            ActionManager.EnqueueAction(new Purify(Maker, Maker));
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(3, pieceOn)));
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(pieceOn.Pos), FileOf(pieceOn.Pos), 1))
            {
                int ind = IndexOf(rankOff, fileOff);
                if (TileManager.Ins.IsTileEmpty(IndexOf(rankOff, fileOff))){ continue; }
                if (IndexOf(rankOff, fileOff) == pieceOn.Pos) { continue; }
                if (PieceOn(ind) == null)
                {
                    TileManager.Ins.DestroyTile(rankOff, fileOff);
                }
            }

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            methaneCasing.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }

        public void Dispose()
        {
            methaneCasing = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
