namespace Doragon.Battle
{
    public class BattleEntityFactory
    {
        //TODO: add more parameters besides identifier for creation

        /// <summary>
        /// Manufactures <see cref="IBattleEntity"> according to classID
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Returns an implementor of <see cref="IBattleEntity">, else null.</returns>
        public IBattleEntity Build(string classID, string formalName, bool myTeam, bool isFrontline, int lineIndex)
        {
            IBattleEntity battleEntity = null;
            switch (classID)
            {
                case "VAN":
                    battleEntity = new Vanguard(team: myTeam, isFrontline, lineIndex, formalName, ManaType.Green, DamageType.Bash, 10, 10, 10, 10, 10, 10);
                    break;
            }
            return battleEntity;
        }
    }
}