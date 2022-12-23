using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameOverCanvas : MonoBehaviour
{
	[SerializeField] private Image image;
    [SerializeField] private TMP_Text playerWonText;
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private float duration = 1.5f;

    private void Start()
    {
        image.DOFade(0, 0);
        playerWonText.DOFade(0, 0);
        titleText.DOFade(0, 0);
    }

    public void SetPlayerName(int name)
    {
        playerWonText.text = $"Player {name} won!";
    }

	public void Fade(float target)
	{
        image.DOFade(target, duration);
        playerWonText.DOFade(target, duration);
        titleText.DOFade(target, duration);
    }
}

