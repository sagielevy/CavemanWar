using UnityEngine;
using System.Collections;

namespace UI
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerSpriteRenderer;

        private bool isInIFrame;

        public void IFrame(bool isInIFrame)
        {
            this.isInIFrame = isInIFrame;
        }

        private void Update()
        {
            // TODO: implement IFrame stuff.
        }
    }
}

