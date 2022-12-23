using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private UnityEngine.UI.Image[] images;
		[SerializeField] private float fadeTime = 1.5f;

		public bool HasGameStarted { get; private set; }

		public void StartGame()
		{
			foreach (var image in images)
			{
				image.DOFade(0, fadeTime);
			}
		}

		private void Update()
		{
			if (!HasGameStarted && Input.GetKeyDown(KeyCode.Space))
			{
				HasGameStarted = true;
                StartGame();
            }
		}
	}
}

