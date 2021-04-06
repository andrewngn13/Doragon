using System;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using Doragon.Battle;

public class DamageHandlerTest
{
    private DamageHandler damageHandler;
    private IBattleEntity vanguard;
    private TextMeshProUGUI manaCalc, manaSum;
    [SetUp]
    public void SetUp()
    {
        manaCalc = new GameObject().AddComponent<TextMeshProUGUI>();
        manaSum = new GameObject().AddComponent<TextMeshProUGUI>();
        damageHandler = new DamageHandler(new ManaLevels(null), manaCalc, manaSum);
        BattleEntityFactory factory = new BattleEntityFactory();
        vanguard = factory.Build("VAN", "Nan", true, true, 0);
    }

    [TearDown]
    public void TearDown()
    {
        damageHandler = new DamageHandler(new ManaLevels(null), manaCalc, manaSum);
    }

    /// <summary>
    /// ArgumentNullException should be thrown when DamageRequest target property is null
    /// </summary>
    [Test]
    public void PushRequestNullTarget()
    {
        Assert.Throws<ArgumentNullException>(() => damageHandler.PushDamageRequest(new DamageRequest()));
    }

    /// <summary>
    /// A valid Damage Request Push
    /// </summary>
    [Test]
    public void PushRequest()
    {
        damageHandler.PushDamageRequest(
            new DamageRequest(ActionRole.Auxiliary, DamageType.Bash, ManaType.Blue, TargettingType.Single, new int[4], 1f, 0, vanguard, new Targets { PrimaryTarget = vanguard }));
    }

    /// <summary>
    /// A valid Damage Request Pop
    /// </summary>
    [Test]
    public void PopRequest()
    {
        damageHandler.PushDamageRequest(
            new DamageRequest(ActionRole.Auxiliary, DamageType.Bash, ManaType.Blue, TargettingType.Single, new int[4], 1f, 0, vanguard, new Targets { PrimaryTarget = vanguard }));
        damageHandler.PopDamageRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void PopRequestSizeCheck()
    {
        Assert.Throws<IndexOutOfRangeException>(() => damageHandler.PopDamageRequest());
    }
    //TODO: TEST damageRequest processing
}
