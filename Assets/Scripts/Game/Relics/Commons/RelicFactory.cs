using Game.Action.Internal.Pending.Relic;

namespace Game.Relics.Commons
{
    public static class RelicFactory
    {
        public static RelicLogic CreateLogicInstance(string key, RelicConfig cfg)
        {
            return key switch
            {
                "relic_adrenaline_radiator" => new AdrenalineRadiator(cfg),
                "relic_black_pearl" => new BlackPearl(cfg),
                "relic_common_pearl" => new CommonPearl(cfg),
                "relic_eye_of_mimic" => new EyeOfMimic(cfg),
                "relic_frost_sigil" => new FrostSigil(cfg),
                "relic_hermos_horn" => new HermosHorn(cfg),
                "relic_mangrove_charm" => new MangroveCharm(cfg),
                "relic_methane_casing" => new MethaneCasing(cfg),
                "relic_ray_stinger" => new RayStinger(cfg),
                "relic_rotting_scythe" => new RottingScythe(cfg),
                "relic_seafoam_phial" => new SeafoamPhial(cfg),
                "relic_sirens_harpoon" => new SirensHarpoon(cfg),
                "relic_storm_capacitor" => new StormCapacitor(cfg),
                "relic_overgrown_slug" => new OvergrownSlug(cfg),
                "relic_precision_monocle" => new PrecisionMonocle(cfg),
                "relic_kelp_banner" => new KelpBanner(cfg),
                "relic_coral_tome" => new CoralTome(cfg),
                _ => null
            };
        }
    }
}
