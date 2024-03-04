using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    public Enemy EnemyPrefab => enemyPrefab;
    public float Hp => hp;
    public float Speed => speed;
    public float Damage => damage;
}