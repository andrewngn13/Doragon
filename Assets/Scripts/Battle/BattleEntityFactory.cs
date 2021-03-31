
namespace Doragon.Battle
{
    public class BattleEntityFactory
    {
        //TODO: add more parameters besides identifier for creation

        /// <summary>
        /// Manufactures <see cref="IBattleEntity">
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Returns an inheritor of <see cref="IBattleEntity">, else null.</returns>
        public IBattleEntity Build(string classID, string formalName, bool myTeam, bool line)
        {
            IBattleEntity battleEntity = null;
            switch (classID)
            {
                case "VAN":
                    battleEntity = new Vanguard(team: myTeam, combatLine: line, formalName, ManaType.Green, DamageType.Bash, 10, 10, 10, 10, 10, 10);
                    break;
            }
            return battleEntity;
        }
    }
}