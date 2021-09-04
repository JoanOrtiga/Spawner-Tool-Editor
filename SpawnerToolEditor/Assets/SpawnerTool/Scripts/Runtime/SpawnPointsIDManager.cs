using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnerTool
{
    public class SpawnPointsIDManager : MonoBehaviour
    {
        private Dictionary<int, Transform> _spawnPoints = new Dictionary<int, Transform>();

#if UNITY_EDITOR
        [Header("Debug")] public int FontSize = 30;

        public Color FontColor = Color.magenta;
        public bool ShowAlways;
#endif

        private void Awake()
        {
            for (int id = 0; id < transform.childCount; id++)
            {
                _spawnPoints[id] = transform.GetChild(id);
            }
        }

        public Vector3 GetSpawnPosition(int id)
        {
            return _spawnPoints[id].position;
        }

        public bool TryGetSpawnPosition(int id, out Vector3 position)
        {
            Transform transform = null;
            if (_spawnPoints.TryGetValue(id, out transform))
            {
                position = transform.position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!ShowAlways)
                return;
            for (int id = 0; id < transform.childCount; id++)
            {
                _spawnPoints[id] = transform.GetChild(id);
                SpawnerToolsUtility.DrawString(id.ToString(), _spawnPoints[id].position, FontColor, FontSize);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (ShowAlways)
                return;
            for (int id = 0; id < transform.childCount; id++)
            {
                _spawnPoints[id] = transform.GetChild(id);
                SpawnerToolsUtility.DrawString(id.ToString(), _spawnPoints[id].position, FontColor, FontSize);
            }
        }
#endif
    }
}