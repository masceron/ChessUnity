using Game.Save.Player;

namespace Game.Save.Money
{
    public static class MoneySaveLoader
    {
        public static int GetMoney()
        {
            return PlayerSaveLoader.Player.Money;
        }

        public static void AddMoney(int amount)
        {
            PlayerSaveLoader.Player.Money += amount;
            PlayerSaveLoader.Save();
        }

        public static bool SpendMoney(int amount)
        {
            if (PlayerSaveLoader.Player.Money < amount)
            {
                return false;
            }

            PlayerSaveLoader.Player.Money -= amount;
            PlayerSaveLoader.Save();
            return true;
        }
    }
}
