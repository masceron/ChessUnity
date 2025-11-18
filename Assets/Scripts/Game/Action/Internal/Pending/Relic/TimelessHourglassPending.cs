using Game.Common;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class TimelessHourglassPending : Action, System.IDisposable, IPendingAble
    {
        private TimelessHourglass _timelessHourglass;
        public TimelessHourglassPending(TimelessHourglass t, int maker, bool pos = false) : base(maker, pos)
        {
            _timelessHourglass = t;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            if(BoardUtils.PieceOn(Target).Color == _timelessHourglass.Color)
            {
                BoardUtils.PieceOn(Target).SkillCooldown -= 2;
            }
            else
            {
                BoardUtils.PieceOn(Target).SkillCooldown += 2;
            }

            TileManager.Ins.UnmarkAll();


            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            _timelessHourglass.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        protected override void ModifyGameState()
        {
        }
        
        public void Dispose()
        {
            _timelessHourglass = null;
            BoardViewer.SelectingFunction = 0;
        }
    }

}
