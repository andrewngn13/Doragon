using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using Doragon.Battle;
using Doragon.UI;

public class BattlePlayModeTest
{
    private const string battleSceneName = "Battle";
    private BattleEntityFactory factory = new BattleEntityFactory();
    private IBattleEntity allyFront, allyFront1, allyFront2, allyBack, allyBack1, allyBack2;

    [OneTimeSetUp]
    public async void OneTimeSetup()
    {
        allyFront = factory.Build("VAN", "Nan", true, true, 0);
        allyFront1 = factory.Build("VAN", "Nan", true, true, 1);
        allyFront2 = factory.Build("VAN", "Nan", true, true, 2);
        allyBack = factory.Build("VAN", "Ban", true, false, 0);
        allyBack1 = factory.Build("VAN", "Ban", true, false, 1);
        allyBack2 = factory.Build("VAN", "Ban", true, false, 2);
        // use unity native loading because of non-access to prefabs for our own instanced scenemanager
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(battleSceneName);
        // .. load data into battlestartup
    }
    [Test]
    // at least 1 frontliner
    // maximum of 5 per team
    // maximum of 3 per frontline 
    // max of 3 per backline
    // check for lineIndex conflicts
    // check lineIndex in range of 0-2 inclusive
    public void ValidTeam()
    {
        var battleStartup = new BattleStartup();
        var entities = new List<IBattleEntity>();
        // should false: 0 in team
        Assert.IsFalse(battleStartup.ValidateTeam(entities));
        entities.Add(allyBack);
        // should false: 1 in back
        Assert.IsFalse(battleStartup.ValidateTeam(entities));
        entities.Add(allyFront);
        // should: true 1 front, 1 back 
        Assert.IsTrue(battleStartup.ValidateTeam(entities));
        entities.Add(allyFront);
        // should: false 2 front, 1 back => failed the duplicate lineIndex check
        Assert.IsFalse(battleStartup.ValidateTeam(entities));
        entities.Remove(allyFront);
        entities.Add(allyFront1);
        // should: true 2 unique front, 1 back
        Assert.IsTrue(battleStartup.ValidateTeam(entities));
        entities.Add(allyFront2);
        entities.Add(allyBack1);
        // should: true 3 unique front, 2 unique back
        Assert.IsTrue(battleStartup.ValidateTeam(entities));
        entities.Add(allyBack2);
        Assert.IsFalse(battleStartup.ValidateTeam(entities));
    }

    // TODO: write integration battle tests below

    // slayer profile frames are in correct order {backline, 012} | {frontline, 012}
    // battletargetting sprites == config data count
    // back button init interactable false
    // back button interactable false when first slayer selected
    // mana level setup
    // damage calculation
    // Animation manager / handler

}
