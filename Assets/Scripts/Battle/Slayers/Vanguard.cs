namespace Doragon.Battle
{
    public class Vanguard : IBattleEntity
    {
        /// <summary>
        /// Inherit basic constructor from <see cref="IBattleEntity"/>.
        /// </summary>
        /// <param name="manaType"></param>
        /// <param name="hp"></param>
        /// <param name="atk"></param>
        /// <param name="def"></param>
        /// <param name="matk"></param>
        /// <param name="mdef"></param>
        /// <param name="spd"></param>
        public Vanguard(bool team, bool frontline, int lineIndex, string formalName, ManaType manaType, DamageType damageType, int hp, int atk, int def, int matk, int mdef, int spd)
        : base(team, frontline, lineIndex, formalName, manaType, damageType, hp, atk, def, matk, mdef, spd) { }

        // TODO: Provoke => buff to self that increases AI aggro
        public void Provoke()
        {
            
        }
    }
}
