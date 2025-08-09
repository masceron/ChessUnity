using System.Collections.Generic;

namespace Game.Movesets
{
    public delegate void QuietsDelegate(List<Action.Action> list, int pos, ref int index);
    public delegate void CapturesDelegate(List<Action.Action> list, int pos);
    public delegate void SkillsDelegate(List<Action.Action> list);
}