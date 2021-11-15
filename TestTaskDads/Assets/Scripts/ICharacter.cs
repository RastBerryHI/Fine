using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface of pawn interaction and behaviour 
/// </summary>
public interface IPawn
{
    /// <summary>
    /// Access to health 
    /// </summary>
    float Health { get; set; }
    /// <summary>
    /// Access to speed 
    /// </summary>
    float MoveSpeed { get; set; }
    /// <summary>
    /// Access to damage 
    /// </summary>
    float Damage { get; set; }
    /// <summary>
    /// Access to attack speed 
    /// </summary>
    float AttackSpeed { get; set; }
    /// <summary>
    /// Method for mutating pawn's health
    /// </summary>
    /// <param name="pawn"> pawn to mutate </param>
    /// <param name="damage"> how much mutate health </param>
    void DealDamage(IPawn pawn, float damage);
    /// <summary>
    /// Use this method to decrease pawn's health
    /// </summary>
    /// <param name="damage"> Damage to add to health </param>
    void EarnDamage(float damage);
    void Attack();
    void Die();
    /// <summary>
    /// Implement movement for your pawn
    /// </summary>
    /// <param name="direction"> The vector to move by </param>
    /// <param name="speed"> Don't forget about difference between frames </param>
    void Move(Vector3 direction, float speed);
}
