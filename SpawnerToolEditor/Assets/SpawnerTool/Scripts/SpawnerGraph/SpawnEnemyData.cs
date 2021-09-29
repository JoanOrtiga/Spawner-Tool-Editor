namespace SpawnerTool
{
    [System.Serializable]
    public class SpawnEnemyData
    {
        public string enemyType;
        public int spawnPointID;
        public int howManyEnemies;
        public float timeToStartSpawning;
        public float timeBetweenSpawn;
        public bool IsAlreadySpawned;

        //Tool editor
        public int currentTrack;

        public SpawnEnemyData(int spawnPointID = 1, float timeToStartSpawning = 0, int howManyEnemies = 5,
            string enemyType = "", float timeBetweenSpawn = 1)
        {
            this.spawnPointID = spawnPointID;
            this.timeToStartSpawning = timeToStartSpawning;
            this.howManyEnemies = howManyEnemies;
            this.enemyType = enemyType;
            this.timeBetweenSpawn = timeBetweenSpawn;
            this.currentTrack = 0;
            IsAlreadySpawned = false;
        }

        public SpawnEnemyData()
        {
            this.spawnPointID = 1;
            this.timeToStartSpawning = 0.0f;
            this.howManyEnemies = 5;
            this.enemyType = "";
            this.timeBetweenSpawn = 1.0f;
            this.currentTrack = 0;
            IsAlreadySpawned = false;
        }
    }
}