using UnityEngine;
using System.Collections;
using System;
using Logic;

namespace UI
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;

        private bool isInIFrame;

        public void IFrame(bool isInIFrame)
        {
            this.isInIFrame = isInIFrame;
        }

        public void UpdatePlayer(Logic.Player previousPlayerState,
            Logic.Player currentPlayerState, LevelSettings levelSettings)
        {
            throw new NotImplementedException();
        }
    }
}

