using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To see curves go to: http://gizma.com/easing/
public static class EasingCurveHelper
{
    // t: current time
    // b: start value
    // c: change in value
    // d: duration 

    // Linear
    public static float Linear(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }

    // Quadratic
    public static float EaseInQuad(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t + b;
    }

    public static float EaseInOutQuad(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t + b;
        t--;
        return -c / 2 * (t * (t - 2) - 1) + b;
    }

    public static float EaseOutQuad(float t, float b, float c, float d)
    {
        t /= d;
        return -c * t * (t - 2) + b;
    }

    // Cubic
    public static float EaseInCubic(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t + b;
    }

    public static float EaseOutCubic(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return c * (t * t * t + 1) + b;
    }

    public static float EaseInOutCubic(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
    }

    // Quartic
    public static float EaseInQuartic(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t * t + b;
    }

    public static float EaseOutQuartic(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return -c * (t * t * t * t - 1) + b;
    }

    public static float EaseInOutQuartic(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t * t + b;
        t -= 2;
        return -c / 2 * (t * t * t * t - 2) + b;
    }

    // Quintic
    public static float EaseInQuintic(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t * t * t + b;
    }

    public static float EaseOutQuintic(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return c * (t * t * t * t * t + 1) + b;
    }

    public static float EaseInOutQuintic(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t * t * t + 2) + b;
    }

    // Sinusoidal
    public static float EaseInSin(float t, float b, float c, float d)
    {
        return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
    }

    public static float EaseOutSin(float t, float b, float c, float d)
    {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }

    public static float EaseInOutSin(float t, float b, float c, float d)
    {
        return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
    }

    // Exponential
    public static float EaseInExpo(float t, float b, float c, float d)
    {
        return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
    }

    public static float EaseOutExpo(float t, float b, float c, float d)
    {
        return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
    }

    public static float EaseInOutExpo(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
        t--;
        return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
    }

    // Circular
    public static float EaseInCirc(float t, float b, float c, float d)
    {
        t /= d;
        return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
    }

    public static float EaseOutCirc(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return c * Mathf.Sqrt(1 - t * t) + b;
    }

    public static float EaseInOutCirc(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
        t -= 2;
        return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
    }

    // Parametric Blend
    public static float ParametricBlend(float t)
    {
        float sqt = Mathf.Sqrt(t);
        return sqt / (2.0f * (sqt - t) + 1.0f);
    }
}
