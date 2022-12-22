using UnityEngine;
using System.Collections;
using System;

namespace UI
{
	public class GroundFire : MonoBehaviour, BurnableTile
    {
        [SerializeField] private Animator fireAnimator;

        public void Burn(bool isBurning)
        {
            fireAnimator.SetBool("IsBurning", isBurning);
        }
    }
}