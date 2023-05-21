using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Raiders
{
    public class MovementSimulation : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float duration = 1;
        [SerializeField, Min(0f)] private float jumpHeight = 1;
        [SerializeField] private bool jumpOnAwake = false;
        private bool isJumping = false;

        private void Awake()
        {
            if (jumpOnAwake)
                StartJumping();
        }

        public void StartJumping()
        {
            if (isJumping)
                return;

            transform.DOLocalMoveY(jumpHeight, duration).SetLoops(-1, LoopType.Yoyo);
            isJumping = true;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
