
using SeraphUtil;

public class HeroStats
{
    public StatInt<HeroUnit> MaxHealth { get; private set; }
    public StatInt<HeroUnit> Damage { get; private set; }
    public StatInt<HeroUnit> Count { get; private set; }
    public StatFloat<HeroUnit> Speed { get; private set; }
    public StatFloat<HeroUnit> Cooldown { get; private set; }
    public StatInt<HeroUnit> Pierce { get; private set; }
    public StatFloat<HeroUnit> RespawnTime { get; private set; }
    public StatFloat<HeroUnit> MovementSpeed { get; private set; }
    public StatInt<HeroUnit> UpgradeCount { get; private set; }


    public HeroStats(HeroBaseStats baseStats)
    {
        MaxHealth = new StatInt<HeroUnit>(baseStats.MaxHealth);
        Damage = new StatInt<HeroUnit>(baseStats.Damage);
        Count = new StatInt<HeroUnit>(baseStats.Count);
        Speed = new StatFloat<HeroUnit>(baseStats.Speed);
        Cooldown = new StatFloat<HeroUnit>(baseStats.Cooldown);
        Pierce = new StatInt<HeroUnit>(baseStats.Pierce);
        RespawnTime = new StatFloat<HeroUnit>(baseStats.RespawnTime);
        MovementSpeed = new StatFloat<HeroUnit>(baseStats.MovementSpeed);
        UpgradeCount = new StatInt<HeroUnit>(baseStats.UpgradeCount);
    }

}
