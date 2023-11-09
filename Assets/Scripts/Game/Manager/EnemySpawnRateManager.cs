using UnityEngine;

namespace Game.Manager
{
    public class EnemySpawnRateManager : Singleton<EnemySpawnRateManager>
    {
        public AnimationCurve enemySpawnRateByStage;
        public float enemySpawnRate { get; private set; }
        [SerializeField] private float treshold;

        [SerializeField] private int howManyCoinPassed;


        private void OnEnable()
        {
            howManyCoinPassed = 0;
            Coin.OnCoinCollected += DropSpawnRate;
            Coin.OnCoinMissed += DropSpawnRate;
        }

        private void OnDisable()
        {
            Coin.OnCoinCollected -= DropSpawnRate;
            Coin.OnCoinMissed -= DropSpawnRate;
        }

        public void SetSpawnRate(int currentStage)
        {
            enemySpawnRate = enemySpawnRateByStage.Evaluate(currentStage);
        }

        private void DropSpawnRate()
        {
            howManyCoinPassed++; // Test Amacli
            
            if (enemySpawnRate <= treshold)
            {
                enemySpawnRate = treshold;
                return;
            }
            
            enemySpawnRate -= 0.05f;
        }
    }
}
