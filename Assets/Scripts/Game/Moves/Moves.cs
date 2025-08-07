using System.Collections.Generic;

namespace Game.Moves
{
    public delegate void QuietsDelegate(List<Action.Action> list, int pos);
    public delegate void CapturesDelegate(List<Action.Action> list, int pos);
    public delegate void SkillsDelegate(List<Action.Action> list);
}