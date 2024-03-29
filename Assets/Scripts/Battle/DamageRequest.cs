using Doragon.Logging;
namespace Doragon.Battle
{
    /// <summary>
    /// Cut, Stab, Bash, Fire, Ice, Electric, Dark
    /// </summary>
    public enum DamageType
    {
        Cut, Stab, Bash, Fire, Ice, Electric, Dark
    }
    /// <summary>
    /// Single, Row, Column, All, Splash
    /// </summary>
    public enum TargettingType
    {
        Single, Row, Column, All, Splash
    }
    public struct DamageRequest
    {
        public bool TargettingMyTeam { get; }
        public ActionRole actionRole { get; }
        DamageType damageTyping { get; }
        ManaType manaTyping { get; }
        public TargettingType TargetTyping { get; }
        public int[] ManaChange { get; }
        float damageMod { get; }
        int priority { get; }
        public IBattleEntity source { get; }
        public Targets target { get; set; }
        public DamageRequest(bool myTeam, ActionRole role, DamageType damageType, ManaType manaType, TargettingType targetType, int[] manaChange, float damageModifier, int movePriority, IBattleEntity src, Targets tar)
        {
            TargettingMyTeam = myTeam;
            actionRole = role;
            damageTyping = damageType;
            manaTyping = manaType;
            TargetTyping = targetType;
            if (manaChange.Length != 4)
                DLogger.LogError("Mana change is improper length");
            ManaChange = manaChange;
            damageMod = damageModifier;
            priority = movePriority;
            source = src;
            target = tar;
        }
    }
}