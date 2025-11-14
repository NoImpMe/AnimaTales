using System.Collections.Generic;
using UnityEngine;

public interface IAllyBattleSetting
{
    GameObject Canvas { get; set; }
    List<GameObject> AllyObjPrefab { get; set; }
    List<GameObject> AllyInstance { get; set; }
    List<GameObject> AllyInfoPrefab { get; set; }
    List<GameObject> AllyInfoInstance { get; set; }
    
    string ObjName { get; set; }
    PlayerInfo PlayerInfo { get; }
    GameObject Prefab { get; set; }
    BattleManager BattleManager { get; }
    List<GameObject> AllyHpPrefab { get; set; }
    List<GameObject> AllyHpInstance { get; set; }
    List<float> DamageX { get; set; }
    List<float> DamageY { get; set; }

    List<GameObject> AllyParserPrefab { get; set; }
    List<GameObject> AllyParserInstance { get; set; }
    GameObject BattleParser { get; set; }
}
