using UnityEngine;

[CreateAssetMenu(fileName = "wave_config", menuName = "Data/Wave info")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private int totalEnemyCount;
    [SerializeField] private float waveDuration;

    public float SpawnDelay => spawnDelay;
    public EnemyData EnemyData => enemyData;
    public int TotalEnemyCount => totalEnemyCount;
    public float WaveDuration => waveDuration;
}
