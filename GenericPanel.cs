using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Hektor
{
    public class GenericPanel : MonoBehaviour
    {
        public AnimationType animationType;
        public FromDirection fromDirection;

        [SerializeField] private float duration;
        [SerializeField] private Ease ease;

        private RectTransform rectTransform;
        private CanvasScaler canvasScaler;

        private Vector2 referenceResolution;
        private Vector2 initialAnchoredPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = FindObjectOfType<CanvasScaler>();

            if (!rectTransform)
                Debug.LogError("You should be sure this component has a RectTransform component");

            referenceResolution = canvasScaler.referenceResolution;

            switch (animationType)
            {
                case AnimationType.Slide:
                    SetFromPosition();
                    break;
                case AnimationType.Scale:
                    rectTransform.localScale = Vector2.zero;
                    break;
            }

            initialAnchoredPosition = rectTransform.anchoredPosition;
        }

        public void Show()
        {
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            EvaluateAnimationType().Item1.Invoke();

        }

        public void Hide()
        {
            EvaluateAnimationType().Item2.Invoke();
        }

        private void SlideIn()
        {
            rectTransform.DOKill(true);
            rectTransform.DOAnchorPos(Vector2.zero, duration)
                .SetEase(ease);
        }

        private void SlideOut()
        {
            rectTransform.DOKill();
            rectTransform.DOAnchorPos(initialAnchoredPosition, duration)
                .SetEase(ease);
        }

        private void ScaleIn()
        {
            rectTransform.DOKill(true);
            rectTransform.DOScale(Vector2.one, duration).SetEase(ease);
        }

        private void ScaleOut()
        {
            rectTransform.DOKill(true);
            rectTransform.DOScale(Vector2.zero, duration).SetEase(ease);
        }

        private void SetFromPosition()
        {
            if (!rectTransform) return;
            switch (fromDirection)
            {
                case FromDirection.Left:
                    rectTransform.anchoredPosition = new Vector2(-referenceResolution.x, 0);
                    break;
                case FromDirection.Right:
                    rectTransform.anchoredPosition = new Vector2(referenceResolution.x, 0);
                    break;
                case FromDirection.Up:
                    rectTransform.anchoredPosition = new Vector2(0, referenceResolution.y);
                    break;
                case FromDirection.Down:
                    rectTransform.anchoredPosition = new Vector2(0, -referenceResolution.y);
                    break;
            }
        }

        /// <summary>
        /// Returns in animation as Item1 and out animation as Item2
        /// </summary>
        /// <returns></returns>
        private (Action, Action) EvaluateAnimationType()
        {
            switch (animationType)
            {
                case AnimationType.Scale:
                    return (ScaleIn, ScaleOut);
                case AnimationType.Slide:
                    return (SlideIn, SlideOut);
            }
            return (null, null);
        }

    }


    public enum FromDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum AnimationType
    {
        Slide,
        Scale
    }

}

