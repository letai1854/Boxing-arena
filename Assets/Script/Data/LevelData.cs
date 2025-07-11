using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData 
{
    public int Level;
    public int playerCount;
    public int allyCount;
    public int enemyCount;
    public int maxSizeCountRow;
    public string mode;
    public PlayerInfo playerInfo;
    public EnemyInfo enemyInfo;
    public AllyInfo allyInfo;


}
[System.Serializable]
public class PlayerInfo
{
    public float player_health_bonus;
    public float player_damage_bonus;
    public float player_speed_bonus;
    public float cooldown_attack_bonus;
}
[System.Serializable]
public class EnemyInfo
{
    public float enemy_health_bonus;
    public float enemy_damage_bonus;
    public float enemy_speed_bonus;
    public float cooldown_attack_bonus;

}
[System.Serializable]
public class AllyInfo
{
    public float ally_health_bonus;
    public float ally_damage_bonus;
    public float ally_speed_bonus;
    public float cooldown_attack_bonus;
}