public class Spearman : MyUnit
{
    protected override void Defend(Unit other, int damage, bool isHp, bool isTrueDamage)
    {
        var realDamage = damage;
        if (other is Archer)
            realDamage *= 2;//Archer deals double damage to spearman.

        base.Defend(other, realDamage, isHp, isTrueDamage);
    }
}
