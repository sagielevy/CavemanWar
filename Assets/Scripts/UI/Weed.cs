using UnityEngine;
using System.Collections;
using System;

namespace UI
{
	public class Weed : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator fireAnimator;

        public void Burn(bool isBurning)
        {
            fireAnimator.SetBool("IsBurning", isBurning);
        }
    }
}
