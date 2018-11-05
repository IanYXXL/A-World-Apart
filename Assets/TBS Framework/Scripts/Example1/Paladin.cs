public class Paladin : MyUnit
{
    protected override void Defend(Unit other, int damage, bool isHp, bool isTrueDamage)
    {
        var realDamage = damage;
        if (other is Spearman)
            realDamage *= 2;//Spearman deals double damage to paladin.

        base.Defend(other, realDamage, isHp, isTrueDamage);
    }
}
