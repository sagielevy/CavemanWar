using UnityEngine;
using System.Collections;
using Logic;

namespace UI
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings LevelSettings;

        private LevelGenerator LevelTilesGenerator;
        private Logic.LevelLogicManager LevelLogicManager;
        private Logic.LevelGenerator LevelStateGenerator;

        private LevelState LevelState;

        private void Start()
        {
            LevelStateGenerator = new Logic.LevelGenerator();
            LevelTilesGenerator = FindObjectOfType<LevelGenerator>();
            LevelLogicManager = new LevelLogicManager(LevelSettings);

            LevelState = LevelStateGenerator.InitialState(LevelSettings);
            LevelTilesGenerator.GenerateInitialLevel(transform, LevelState, LevelSettings);
        }

        private void Update()
        {
            //var player1Input =

            LevelTilesGenerator.SpwanNewWeeds(LevelSettings);
            //LevelLogicManager.UpdateLevel()
        }
    }
}