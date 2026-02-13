using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class CorruptedWisperPending : PendingAction, System.IDisposable
    {
        private readonly CorruptedWisper _corruptedWisper;

        public CorruptedWisperPending(int target, CorruptedWisper corruptedWisper) : base(target)
        {
            Target = target;
            _corruptedWisper = corruptedWisper;
        }


        protected override void CompleteAction()
        {
            var execute = new CorruptedWisperExecute(Target);

            CommitResult(execute);
            _corruptedWisper.LevelUp();
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
        }

    }
}

