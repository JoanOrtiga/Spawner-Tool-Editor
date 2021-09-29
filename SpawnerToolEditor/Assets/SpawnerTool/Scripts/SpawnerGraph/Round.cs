using System.Collections.Generic;

namespace SpawnerTool
{
    [System.Serializable]
    public class Round
    {
        public List<SpawnEnemyData> spawningEnemiesData;
        public float totalRoundTime;

        //Tool editor
        public int totalTracks;

        public Round(List<SpawnEnemyData> spawningEnemies = null, float totalRoundTime = default,
            int totalTracks = default)
        {
            if (spawningEnemies == null)
            {
                this.spawningEnemiesData = new List<SpawnEnemyData>();
            }
            else
                this.spawningEnemiesData = spawningEnemies;

            this.totalRoundTime = totalRoundTime;
            this.totalTracks = totalTracks;
        }

        public void InitializeSpawningEnemies(List<SpawnEnemyData> spawningEnemies)
        {
            this.spawningEnemiesData = spawningEnemies;
        }
    }
}