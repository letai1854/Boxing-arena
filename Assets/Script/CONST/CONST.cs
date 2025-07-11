using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CONST 
{
    public static string TAPPLAYRER = "Player";
    public static string TAPENEMY = "Enemy";
    public static string ALLY = "ALLY";
    public static LayerMask enemyLayer = LayerMask.GetMask("Enemy");
    public static LayerMask playerLayer = LayerMask.GetMask("Player");
    public static LayerMask allyLayer = LayerMask.GetMask("Ally");
    public static readonly LayerMask playerTeamLayer = playerLayer | allyLayer;
    public static string SAVE_FILE_NAME = "levels.json";
    public static string SAVE_FILE_LEVEL = "currentlevel.json";
    public static string POOL_ENEMY = "enemy";  
    public static string POOL_ALLY = "ally";
    public static string POOL_PLAYER = "player";
    public static string POOL_HEALTHBAR = "healthBar";

}
