namespace SpawnerTool
{
    [System.Serializable]
    public class SpawnEnemyData
    {
        private string _enemyType;
        public string EnemyType
        {
            get => _enemyType;
            set => _enemyType = value;
        }
        
        private int _spawnPointID;
        public int SpawnPointID
        {
            get => _spawnPointID;
            set => _spawnPointID = value;
        }
        
        private int _howManyEnemies;
        public int HowManyEnemies
        {
            get => _howManyEnemies;
            set => _howManyEnemies = value;
        }
        
        private float _timeToStartSpawning;
        public float TimeToStartSpawning
        {
            get => _timeToStartSpawning;
            set => _timeToStartSpawning = value;
        }

        private float _timeBetweenSpawn;
        public float TimeBetweenSpawn
        {
            get => _timeBetweenSpawn;
            set => _timeBetweenSpawn = value;
        }

        private bool _isAlreadySpawned;
        public bool IsAlreadySpawned
        {
            get => _isAlreadySpawned;
            set => _isAlreadySpawned = value;
        }
        
        //For Tool editor
        public int CurrentTrack;

        public SpawnEnemyData(int spawnPointID = 1, float timeToStartSpawning = 0, int howManyEnemies = 5,
            string enemyType = "", float timeBetweenSpawn = 1)
        {
            this.SpawnPointID = spawnPointID;
            this.TimeToStartSpawning = timeToStartSpawning;
            this.HowManyEnemies = howManyEnemies;
            this.EnemyType = enemyType;
            this.TimeBetweenSpawn = timeBetweenSpawn;
            this.CurrentTrack = 0;
            this.IsAlreadySpawned = false;
        }

        public SpawnEnemyData()
        {
            this.SpawnPointID = 1;
            this.TimeToStartSpawning = 0.0f;
            this.HowManyEnemies = 5;
            this.EnemyType = "";
            this.TimeBetweenSpawn = 1.0f;
            this.CurrentTrack = 0;
            this.IsAlreadySpawned = false;
        }
    }
}