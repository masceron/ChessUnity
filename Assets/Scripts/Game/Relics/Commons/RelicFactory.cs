namespace Game.Relics.Commons
{
    public static class RelicFactory
    {
        public static RelicLogic CreateLogicInstance(string key, RelicConfig cfg)
        {
            return key switch
            {
                "relic_black_pearl" => new BlackPearl(cfg),
                "relic_common_pearl" => new CommonPearl(cfg),
                "relic_eye_of_mimic" => new EyeOfMimic(cfg),
                "relic_frost_sigil" => new FrostSigil(cfg),
                "relic_mangrove_charm" => new MangroveCharm(cfg),
                "relic_rotting_scythe" => new RottingScythe(cfg),
                "relic_seafoam_phial" => new SeafoamPhial(cfg),
                "relic_sirens_harpoon" => new SirensHarpoon(cfg),
                "relic_storm_capacitor" => new StormCapacitor(cfg),
                "relic_peace_treaty" => new PeaceTreaty(cfg),
                "relic_reliquary" => new Reliquary(cfg),
                "relic_ledger_stone" => new LedgerStone(cfg),
                "relic_ornetes_edict" => new OrnetesEdict(cfg),
                _ => null
            };
        }
    }
}
