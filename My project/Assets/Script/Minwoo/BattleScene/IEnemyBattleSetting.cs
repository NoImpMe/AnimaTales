using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBattleSetting 
{
    List<float> DamageX { get; set; }
    List<float> DamageY { get; set; }
    List<GameObject> EnemyObjPrefab { get; set; }
    List<GameObject> EnemyInstance { get; set; }
    string ObjName { get; set; }
    List<string> ObjectFileList { get; set; }
    List<string> BattleEnemyAnima { get; set; }
    List<GameObject> EnemyHpPrefab { get; set; }
    List<GameObject> EnemyHpInstance { get; set; }
    List<GameObject> EnemyInfoPrefab { get; set; }
    List<GameObject> EnemyInfoInstance { get; set; }
    List<GameObject> EnemyParserPrefab { get; set; }
    List<GameObject> EnemyParserInstance { get; set; }
    GameObject Canvas { get; set; }
    GameObject BattleParser { get; set; }
    BattleManager BattleManager { get; }
    string Stage { get; set; }
}
