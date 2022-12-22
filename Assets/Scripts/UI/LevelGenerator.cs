using UnityEngine;
using System.Collections;
using Logic;

namespace UI
{
	public class LevelGenerator : MonoBehaviour
	{
		[SerializeField] private Weed WeedPrefab;
        [SerializeField] private Ground GroundPrefab;
        [SerializeField] private Transform RockPrefab;
        [SerializeField] private Player PlayerPrefab;

        public void GenerateInitialLevel(Transform parent,
            LevelState levelState, LevelSettings LevelSettings)
        {
            throw new System.Exception();
        }

        public void SpwanNewWeeds(LevelSettings LevelSettings)
        {
            throw new System.Exception();
        }
    }
}