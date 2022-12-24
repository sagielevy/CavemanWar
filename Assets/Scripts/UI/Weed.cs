using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
using Logic;

namespace UI
{
	public class Weed : MonoBehaviour, BurnableTile, FadeableTile
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator fireAnimator;

        public void Burn(bool isBurning)
        {
            fireAnimator.enabled = true;
            fireAnimator.SetBool("IsBurning", isBurning);
        }

        public void Fade(float endValue, float duration)
        {
            fireAnimator.enabled = false;
            spriteRenderer.DOFade(endValue, duration)
                .OnComplete(() => fireAnimator.enabled = true);
        }

        public void FadeOutAndDestroy(float duration)
        {
            fireAnimator.enabled = false;
            spriteRenderer.DOFade(0, duration)
                .OnComplete(() => Destroy(gameObject));
        }

        public void SetOpacity(float value)
        {
            spriteRenderer.DOFade(value, 0).Complete();
        }
    }
}
