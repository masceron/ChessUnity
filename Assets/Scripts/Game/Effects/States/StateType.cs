namespace Game.Effects.States
{
    /// <summary>
    ///     Danh sách tất cả các State mà một quân cờ hoặc Formation có thể có.
    ///     Mỗi quân cờ / Formation chỉ mang đúng 1 State tại 1 thời điểm.
    ///     Mặc định là <see cref="None"/>.
    /// </summary>
    public enum StateType : byte
    {
        None = 72,
        Infested = 73,
        Parasite = 74,
        Tethered = 75,
        Securing = 76,
        Cooperative = 77,
        Burrowed = 78,
        Ethereal = 79,
        Petrified = 80,
        Adhesive = 81,
        Attached = 82
    }
}
