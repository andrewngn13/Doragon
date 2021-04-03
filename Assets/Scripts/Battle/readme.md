Battle flow:
Scenemanager (some kind of battle setup data config here)
<test>load scene battle and wait for BattleStartUp to finish, then assert
//TODO: proper constructor 
-BattleStartUp(configData)
//TODO: define configData structure
--<test>BattleFactory produces all IBattleEntity(configData)</test>
--UIInputHandler init(list of IBattleEntity)
---<test>DamageHandler creation(manaCalcTextbox, manaSumText)</test>
-----
---<test>TargettingSystem init</test>
---<test>normal attack setup(click to open targetting, await targetting confirm/cancel)</test>
---<test>skill setup(bind IBattleEntity's skill requests to new UI objects)</test>
---<test>skill setup(click to open targetting, await targetting confirm/cancel)</test>


</test>
shared data:
-list of IBattleEntity
--do i need to cascade this if they die?