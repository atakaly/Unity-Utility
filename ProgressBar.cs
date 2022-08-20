using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar {

    private readonly Image fillImage;
    private readonly Gradient gradient;

    private readonly float fillDuration;

    public ProgressBar(Image _fillImage, Gradient _gradient, float _fillDuration = 0.2f)
    {
        fillImage = _fillImage;
        gradient = _gradient;
        fillDuration = _fillDuration;
    }

    public void SetFillAmount(float amount, float maxAmount = 100f)
    {
        var mappedAmount = amount.Remap(0f, maxAmount, 0f, 1f);

        fillImage.DOFillAmount(mappedAmount, fillDuration);
        fillImage.DOColor(GetGradientColor(mappedAmount), fillDuration);
    }

    private Color GetGradientColor(float amount)
    {
        return gradient.Evaluate(amount);
    }
}
