using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Animator anim; 
    public Rigidbody rb;
    public float attackCountTemp = 0;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int hitHead = 30;
    public int hitStomatch = 20;
    public int hitKidney = 10;
    public float attackCount = 0f;
    public float attackCooldownTimer = 0f;
    public float attackCooldown = 1.5f;
    [Header("info")]
    [SerializeField] public float baseMoveSpeed;
    [Header("Targeting Info")]
    public int MaxAttackersAllowed = 2;
    public List<Entity> attackers;
    public HealthBar healthBar;



    public int originalMaxHealth;
    public int originalHitHead;
    public int originalHitStomatch;
    public int originalHitKidney;
    public float originalBaseMoveSpeed;
    public float originalAttackCooldown;

    public virtual void Awake()
    {
        attackers = new List<Entity>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }



    public virtual void Start()
    {
    }
    public virtual void Update()
    {
      
        
    }





    public void SetHealthBar(HealthBar healthBarPrefab)
    {
        healthBar = healthBarPrefab;
    }

    public void SetUpInfo(float health, float damage, float speed, float cooldown)
    {
        currentHealth = originalMaxHealth + (int)(originalMaxHealth * health);
        maxHealth = currentHealth;
        hitHead = originalHitHead + (int)(originalHitHead * damage);
        hitStomatch = originalHitStomatch + (int)(originalHitStomatch * damage);
        hitKidney = originalHitKidney + (int)(originalHitKidney * damage);
        baseMoveSpeed = originalBaseMoveSpeed + speed* originalBaseMoveSpeed;
        attackCooldown = originalAttackCooldown + cooldown;

    }


    public void RegisterAttacker(Entity enemy)
    {
        if (!attackers.Contains(enemy))
        {
            attackers.Add(enemy);
        }
    }



    public int TakeDamageCount()
    {
        if (attackCount == 0f)
        {
            return hitHead;
        }
        else if (attackCount == 0.5f)
        {
            return  hitKidney;
        }
         return hitStomatch;
        
    }
    public abstract void ResetStatEntity();
}
