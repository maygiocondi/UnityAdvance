public class Wave
{
    private WaveConfig config;
    private float spawnedEnemyCount;

    public WaveConfig Config => config;

    public Wave(WaveConfig config)
    {
        this.config = config;
    }

    public bool SpawnEnemy()
    {
        if (spawnedEnemyCount >= config.TotalEnemyCount)
        {
            return false;
        }
        spawnedEnemyCount++;
        return true;
    }
}
