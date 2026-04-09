using System.Collections.Generic;

namespace Game.Movesets
{
    // Moveset chỉ lo pathfinding: trả về list vị trí có thể đi/ăn
    // PieceLogic.MoveList lo logic game: tạo Action tương ứng
    public delegate int QuietsDelegate(List<int> positions, int pos);

    public delegate int CapturesDelegate(List<int> positions, int pos);

    public delegate void SkillsDelegate(List<Action.Action> list, bool isPlayer, bool excludeEmptyTile);
}