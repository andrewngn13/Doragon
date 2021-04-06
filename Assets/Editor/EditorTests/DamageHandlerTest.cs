using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using Doragon.Battle;

public class DamageHandlerTest
{
    private DamageHandler damageHandler;
    private DamageRequest nanSource;
    private IBattleEntity vanguard, rearguard, enemyFrontliner, enemyBackliner;
    private TextMeshProUGUI manaCalc, manaSum;
    [SetUp]
    public void SetUp()
    {
        manaCalc = new GameObject().AddComponent<TextMeshProUGUI>();
        manaSum = new GameObject().AddComponent<TextMeshProUGUI>();
        damageHandler = new DamageHandler(new ManaLevels(null), manaCalc, manaSum);
        BattleEntityFactory factory = new BattleEntityFactory();
        vanguard = factory.Build("VAN", "Nan", true, true, 0);
        rearguard = factory.Build("VAN", "San", true, true, 0);
        enemyFrontliner = factory.Build("VAN", "Ban", true, true, 0);
        enemyBackliner = factory.Build("VAN", "Dan", true, true, 0);

        nanSource = new DamageRequest(false, ActionRole.Auxiliary, DamageType.Bash, ManaType.Blue, TargettingType.Single, new int[4], 1f, 0, vanguard, new Targets { PrimaryTarget = vanguard });
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
    public void PushRequestNullSource()
    {
        var noSrc = new DamageRequest();
        noSrc.target = new Targets { PrimaryTarget = vanguard };
        var ex = Assert.Throws<ArgumentNullException>(() => damageHandler.PushDamageRequest(noSrc));
        Debug.Log(ex.Message);
    }

    /// <summary>
    /// ArgumentNullException should be thrown when DamageRequest target property is null
    /// </summary>
    [Test]
    public void PushRequestNullTarget()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => damageHandler.PushDamageRequest(new DamageRequest()));
        Debug.Log(ex.Message);
    }

    /// <summary>
    /// A valid Damage Request Push
    /// </summary>
    [Test]
    public void PushRequest()
    {
        damageHandler.PushDamageRequest(nanSource);
    }

    /// <summary>
    /// A valid Damage Request Pop
    /// </summary>
    [Test]
    public void PopRequest()
    {
        nanSource.target = new Targets { PrimaryTarget = vanguard };
        damageHandler.PushDamageRequest(nanSource);
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
    [Test]
    public void TestCorrectTargets()
    {
        nanSource.target = new Targets { PrimaryTarget = vanguard };
        // Nan to Nan
        damageHandler.PushDamageRequest(nanSource);
        Assert.IsTrue(damageHandler.damageRequests.Where(r => r.source.Name == "Nan" && r.target.PrimaryTarget.Name == "Nan").Count() == 1);
        // Nan to San
        nanSource.target = new Targets { PrimaryTarget = rearguard };
        damageHandler.PushDamageRequest(nanSource);
        Assert.IsTrue(damageHandler.damageRequests.Where(r => r.source.Name == "Nan" && r.target.PrimaryTarget.Name == "San").Count() == 1);
        // Nan to Ban
        nanSource.target = new Targets { PrimaryTarget = enemyFrontliner };
        damageHandler.PushDamageRequest(nanSource);
        Assert.IsTrue(damageHandler.damageRequests.Where(r => r.source.Name == "Nan" && r.target.PrimaryTarget.Name == "Ban").Count() == 1);
        // Nan to Dan
        nanSource.target = new Targets { PrimaryTarget = enemyBackliner };
        damageHandler.PushDamageRequest(nanSource);
        Assert.IsTrue(damageHandler.damageRequests.Where(r => r.source.Name == "Nan" && r.target.PrimaryTarget.Name == "Dan").Count() == 1);
    }
}
