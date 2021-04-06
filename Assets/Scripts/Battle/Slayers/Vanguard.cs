using System.Collections.Generic;
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

        public override List<DamageRequest> GetSkills()
        {
            var skillList = new List<DamageRequest>();
            skillList.Add(Provoke());

            return skillList;
        }

        // TODO: Provoke => buff to self that increases AI aggro
        public DamageRequest Provoke()
        {
            return new DamageRequest(true, ActionRole.Auxiliary, DamageType.Bash, ManaType.Blue, TargettingType.Single, new int[4] { -5, -5, -5, -5 }, 0f, 1, this, null);
        }
    }
}
