using System;

namespace Doragon.Battle
{
    public enum ManaType
    {
        Red, Blue, Green, Violet
    }

    public enum DamageType
    {
        Cut, Stab, Bash, Fire, Ice, Electric, Dark
    }
    public abstract class IBattleEntity
    {
        public bool MyTeam { get; }
        public bool FrontLine { get; }
        public string Name { get; }
        ManaType manaTyping { get; }
        DamageType normalDamageTyping { get; }
        int currentHp { get; }
        int HP { get; }
        int ATK { get; }
        int DEF { get; }
        int MATK { get; }
        int MDEF { get; }
        int SPD { get; }
        /// <summary>
        /// Construct basic parameters for a IBattleEntity
        /// </summary>
        /// <param name="formalName"></param>
        /// <param name="manaType"></param>
        /// /// <param name="normalDamageType"></param>
        /// <param name="hp"></param>
        /// <param name="atk"></param>
        /// <param name="def"></param>
        /// <param name="matk"></param>
        /// <param name="mdef"></param>
        /// <param name="spd"></param>
        // TODO: Consideration, change myTeam to a userID if networking ever happens
        protected IBattleEntity(bool team, bool combatline, string formalName, ManaType manaType, DamageType normalDamageType,
            int hp, int atk, int def, int matk, int mdef, int spd)
        {
            MyTeam = team;
            FrontLine = combatline;
            Name = formalName;
            manaTyping = manaType;
            normalDamageTyping = normalDamageType;
            HP = hp;
            currentHp = HP;
            ATK = atk;
            DEF = def;
            MATK = matk;
            MDEF = mdef;
            SPD = spd;
        }

        public int GetSpeedRating()
        {
            Random rand = new Random();
            // TODO: better speed formula
            return SPD * rand.Next(90, 121);
        }
        public DamageRequest NormalAttack()
        {
            // TODO: fix normal attacks
            return new DamageRequest(ActionRole.Auxiliary, normalDamageTyping, manaTyping, TargettingType.Single, 1f, 0, this, null);
        }
    }
}