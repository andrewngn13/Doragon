using Doragon.Logging;
using UnityEngine;

namespace Doragon.Battle
{
    /// <summary>
    /// Acts similar to pseudo Main() for faking a battle startup, or a node selection / other battle start event
    /// </summary>
    public class TestBattle : MonoBehaviour
    {
        private void Start()
        {
            // TODO: spin this up from the scenemanager instead
            DLogger.Log("Spinning up debug battle");
            BattleStartup battle = new BattleStartup("debug");
        }
    }
}