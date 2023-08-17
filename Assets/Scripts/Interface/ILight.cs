using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILight
{
    public void TurnOff();

    public void TurnOn();

    public void Flicker(float wavelength, float amplitude, float duration);

    public void SetColor(Color color);
}
