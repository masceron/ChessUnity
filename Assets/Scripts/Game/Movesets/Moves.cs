using System.Collections.Generic;

namespace Game.Movesets
{
    //isPlayer là để phân biệt người dùng hay AI
    //excludeEmptyTile để khi AI chọn các ô có thể ăn/skill thì sẽ lấy cả ô không có quân như vậy sẽ tính được vùng nguy hiểm và trừ điểm action
    public delegate int QuietsDelegate(List<Action.Action> list, int pos, bool isPlayer);

    public delegate int CapturesDelegate(List<Action.Action> list, int pos, bool excludeEmptyTile);

    public delegate void SkillsDelegate(List<Action.Action> list, bool isPlayer, bool excludeEmptyTile);
}