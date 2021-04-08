using System;
using System.Collections.Generic;

namespace Doragon.Battle
{
    public enum ManaType
    {
        Red, Blue, Green, Violet
    }

    public abstract class IBattleEntity
    {
        public bool MyTeam { get; }
        public bool FrontLine { get; }
        public int LineIndex { get; }
        public string Name { get; }
        ManaType manaTyping { get; }
        DamageType normalDamageTyping { get; }
        public int CurrentHP { get; set; }
        public int HP { get; }
        public int SPD { get; }
        int ATK { get; }
        int DEF { get; }
        int MATK { get; }
        int MDEF { get; }
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
        // TODO? change myTeam to a userID if networking ever happens
        protected IBattleEntity(bool team, bool combatline, int lineIndex, string formalName, ManaType manaType, DamageType normalDamageType,
            int hp, int atk, int def, int matk, int mdef, int spd)
        {
            MyTeam = team;
            FrontLine = combatline;
            LineIndex = lineIndex;
            Name = formalName;
            manaTyping = manaType;
            normalDamageTyping = normalDamageType;
            HP = hp;
            CurrentHP = HP;
            ATK = atk;
            DEF = def;
            MATK = matk;
            MDEF = mdef;
            SPD = spd;
        }
        public DamageRequest NormalAttack()
        {
            // TODO: fix normal attacks
            return new DamageRequest(false, ActionRole.Auxiliary, normalDamageTyping, manaTyping, TargettingType.Single, new int[4] { 1, -1, 2, -2 }, 1f, 0, this, new Targets());
        }

        public abstract List<DamageRequest> GetSkills();

    }
}