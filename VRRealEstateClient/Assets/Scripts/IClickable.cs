using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
  public void OnPointerEnter();

  public void OnPointerExit();

  public void OnPointerClick();
}
