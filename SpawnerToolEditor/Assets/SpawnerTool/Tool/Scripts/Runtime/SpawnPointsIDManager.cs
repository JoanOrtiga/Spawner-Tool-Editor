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
        [Header("Debug"), SerializeField, Tooltip("Show gizmos always, even when object is not selected")] private bool _showAlways;
        [SerializeField] private bool _showSpawnNumbersAsGizmos = true;
        [SerializeField] private int _numbersFontSize = 30;
        [SerializeField] private Color _fontColor = Color.magenta;
        [SerializeField] private bool _showSpawnPoints = true;
        [SerializeField] private float _sphereRadius = 0.2f;
        [SerializeField] private Color _sphereColor = Color.magenta;
#endif

        private void Awake()
        {
            for (int id = 0; id < transform.childCount; id++)
            {
                _spawnPoints[id] = transform.GetChild(id);
            }
        }

        /// <summary>
        /// Get Position of the id, 
        /// </summary>
        /// <see cref="https://joanorba.gitbook.io/spawnertool-editor/api/runtime/spawnpointsidmanager/trygetspawnposition"/>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vector3 GetSpawnPosition(int id)
        {
            return _spawnPoints[id].position;
        }

        /// <summary>
        /// Get Position of the ID.
        /// </summary>
        /// https://app.gitbook.com/s/HUKylQVAXeOUhje2mIB9/runtime/spawnpointsidmanager
        /// <see cref="https://joanorba.gitbook.io/spawnertool-editor/api/runtime/spawnpointsidmanager/trygetspawnposition"/>
        /// <param name="id">The ID of the spawner</param>
        /// <param name="position">Vector3 with the returned position</param>
        /// <returns>Returns true if ID exists, false if not.</returns>
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
            if (!_showAlways)
                return;

            DrawElements();
        }

        private void OnDrawGizmosSelected()
        {
            if (_showAlways)
                return;
            
            DrawElements();
        }

        private void DrawElements()
        {
            for (int id = 0; id < transform.childCount; id++)
            {
                _spawnPoints[id] = transform.GetChild(id);
                DrawString(id);
                DrawPoints(id);
            }
        }

        private void DrawPoints(int id)
        {
            if (!_showSpawnPoints)
                return;
            Gizmos.color = _sphereColor;
            Gizmos.DrawSphere(_spawnPoints[id].position, _sphereRadius);
        }

        private void DrawString(int id)
        {
            if(!_showSpawnNumbersAsGizmos)
                return;
            SpawnerToolsUtility.DrawString(id.ToString(), _spawnPoints[id].position, _fontColor, _numbersFontSize);
        }
#endif
    }
}