using Doragon.Logging;
namespace Doragon.Battle
{
    public struct DamageRequest
    {
        public ActionRole actionRole { get; }
        DamageType damageTyping { get; }
        ManaType manaTyping { get; }
        public TargettingType TargetTyping { get; }
        public int[] ManaChange { get; }
        float damageMod { get; }
        int priority { get; }
        public IBattleEntity source { get; }
        public IBattleEntity target { get; set; }
        public DamageRequest(ActionRole role, DamageType damageType, ManaType manaType, TargettingType targetType, int[] manaChange, float damageModifier, int movePriority, IBattleEntity src, IBattleEntity tar)
        {
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