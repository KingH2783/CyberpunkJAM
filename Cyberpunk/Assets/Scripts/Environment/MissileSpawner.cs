using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HL
{
    public class MissileSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private Transform[] missileSpawnPoints;
        [SerializeField] private int minNumberOfMissiles;
        [SerializeField] private int maxNumberOfMissiles;
        [SerializeField] private float minTimeBetweenMissiles;
        [SerializeField] private float maxTimeBetweenMissiles;

        public IEnumerator SpawnMissiles(AIManager boss)
        {
            int randomNumberOfMissiles = Random.Range(minNumberOfMissiles, maxNumberOfMissiles);
            List<int> usedIndices = new();

            for (int i = 0; i < randomNumberOfMissiles; i++)
            {
                int randomIndex = GetUniqueRandomIndex(missileSpawnPoints.Length, usedIndices);
                usedIndices.Add(randomIndex);

                float randomTime = Random.Range(minTimeBetweenMissiles, maxTimeBetweenMissiles);
                yield return new WaitForSeconds(randomTime);

                GameObject missileGameObject = Instantiate(missilePrefab, missileSpawnPoints[randomIndex]);
                Bullet missile = missileGameObject.GetComponent<Bullet>();
                missile.characterWhoFiredMe = boss;
                missile.weapon = boss.aiStatsManager.currentRangedWeapon;
            }

            yield return null;
        }

        private int GetUniqueRandomIndex(int max, List<int> usedIndices)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, max);
            } 
            while (usedIndices.Contains(randomIndex));

            return randomIndex;
        }

        private void OnValidate()
        {
            if (minNumberOfMissiles < 1)
                minNumberOfMissiles = 1;
            if (minNumberOfMissiles > maxNumberOfMissiles)
                maxNumberOfMissiles = minNumberOfMissiles;
            if (maxNumberOfMissiles < 1)
                maxNumberOfMissiles = 1;

            if (minTimeBetweenMissiles < 0)
                minTimeBetweenMissiles = 0;
            if (minTimeBetweenMissiles > maxTimeBetweenMissiles)
                maxTimeBetweenMissiles = minTimeBetweenMissiles;
            if (maxTimeBetweenMissiles < 0)
                maxTimeBetweenMissiles = 0;
        }
    }
}