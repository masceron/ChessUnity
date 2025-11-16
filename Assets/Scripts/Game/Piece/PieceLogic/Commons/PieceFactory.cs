using UnityEngine;

namespace Game.Piece.PieceLogic.Commons
{
    public static class PieceFactory
    {
    public static PieceLogic CreateLogicInstance(string key, PieceConfig cfg)
        {
            switch (key)
            {
                case "piece_anglerfish":
                    return new Anglerfish(cfg);
                case "piece_anomalocaris":
                    return new Anomalocaris(cfg);
                case "piece_archelon":
                    return new Archelon(cfg);
                case "piece_archerfish":
                    return new Archerfish(cfg);
                case "piece_arctic_brittle_star":
                    return new ArcticBrittleStar(cfg);
                case "piece_armored_feather_star":
                    return new ArmoredFeatherStar(cfg);
                case "piece_barnacle":
                    return new Barnacle(cfg);
                case "piece_barracuda":
                    return new Barracuda(cfg);
                case "piece_bioluminescent_beacon":
                    return new BioluminescentBeacon(cfg);
                case "piece_black_swallower":
                    return new BlackSwallower(cfg);
                case "piece_blue_dragon":
                    return new BlueDragon(cfg);
                case "piece_bobtail_squid":
                    return new BobtailSquid(cfg);
                case "piece_bottlenose_dolphin":
                    return new BottlenoseDolphin(cfg);
                case "piece_brittle_star":
                    return new BrittleStar(cfg);
                case "piece_chambered_nautilus":
                    return new ChamberedNautilus(cfg);
                case "piece_chrysos":
                    return new Chrysos(cfg);
                case "piece_clown_fish":
                    return new ClownFish(cfg);
                case "piece_coffin_fish":
                    return new CoffinFish(cfg);
                case "piece_contagion_corpse":
                    return new ContagionCorpse(cfg);
                case "piece_electric_eel":
                    return new ElectricEel(cfg);
                case "piece_epaulette_shark":
                    return new EpauletteShark(cfg);
                case "piece_fangtooth":
                    return new Fangtooth(cfg);
                case "piece_feather_star":
                    return new FeatherStar(cfg);
                case "piece_flying_fish":
                    return new FlyingFish(cfg);
                case "piece_fracture_zone":
                    return new FractureZone(cfg);
                case "piece_grenadiers":
                    return new Grenadiers(cfg);
                case "piece_gulper_eel":
                    return new GulperEel(cfg);
                case "piece_hammer_oyster":
                    return new HammerOyster(cfg);
                case "piece_hatchetfish":
                    return new Hatchetfish(cfg);
                case "piece_helicoprion":
                    return new Helicoprion(cfg);
                case "piece_hermit_crab":
                    return new HermitCrab(cfg);
                case "piece_horseleech":
                    return new HorseLeech(cfg);
                case "piece_hourglass_jelly":
                    return new HourglassJelly(cfg);
                case "piece_humboldt_squid":
                    return new HumboldtSquid(cfg);
                case "piece_humilitas":
                    return new Humilitas(cfg);
                case "piece_kelp_bass":
                    return new KelpBass(cfg);
                case "piece_kelp_forest":
                    return new KelpForest(cfg);
                case "piece_lionfish":
                    return new Lionfish(cfg);
                case "piece_living_coral":
                    return new LivingCoral(cfg);
                case "piece_lizard_fish":
                    return new Lizardfish(cfg);
                case "piece_marine_iguana":
                    return new MarineIguana(cfg);
                case "piece_medicinal_leech":
                    return new MedicinalLeech(cfg);
                case "piece_megalodon":
                    return new Megalodon(cfg);
                case "piece_melibe":
                    return new Melibe(cfg);
                case "piece_moorish_idols":
                    return new MoorishIdols(cfg);
                case "piece_moray_eel":
                    return new MorayEel(cfg);
                case "piece_phantom_jelly":
                    return new PhantomJelly(cfg);
                case "piece_phronima":
                    return new Phronima(cfg);
                case "piece_pistol_shrimp":
                    return new PistolShrimp(cfg);
                case "piece_polluted_rock":
                    return new PollutedRock(cfg);
                case "piece_pufferfish":
                    return new Pufferfish(cfg);
                case "piece_remora":
                    return new Remora(cfg);
                case "piece_sea_star":
                    return new SeaStar(cfg);
                case "piece_sea_turtle":
                    return new SeaTurtle(cfg);
                case "piece_sea_urchin":
                    return new SeaUrchin(cfg);
                case "piece_seahorse":
                    return new Seahorse(cfg);
                case "piece_siren":
                    return new GuidingSiren(cfg);
                case "piece_slimehead":
                    return new Slimehead(cfg);
                case "piece_sloane's_viperfish":
                    return new SloaneSViperfish(cfg);
                case "piece_snaggletooths":
                    return new Snaggletooths(cfg);
                case "piece_snipe_eel":
                    return new SnipeEel(cfg);
                case "piece_spider_brittle_star":
                    return new SpiderBrittleStar(cfg);
                case "piece_stingray":
                    return new Stingray(cfg);
                case "piece_stonecrab":
                    return new StoneCrab(cfg);
                case "piece_sunfish":
                    return new Sunfish(cfg);
                case "piece_swordfish":
                    return new Swordfish(cfg);
                case "piece_temperantia":
                    return new Temperantia(cfg);
                case "piece_thalassos":
                    return new Thalassos(cfg);
                case "piece_tiger_prawn":
                    return new TigerPrawn(cfg);
                case "piece_velkaris":
                    return new Velkaris(cfg);
                default:
                    Debug.LogError($"Unknown Piece ID: {key}");
                    return null;
            }
        }

    }
}
