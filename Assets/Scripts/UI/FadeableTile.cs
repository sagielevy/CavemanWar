using System;

public interface FadeableTile
{
    public void SetOpacity(float value);

    public void Fade(float endValue, float duration);

    public void FadeOutAndDestroy(float duration);
}

