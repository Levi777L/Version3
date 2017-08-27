using UnityEngine;
using System.Collections;

public interface IMode
{
    void IUpdate();
    void IControlUpdate();
    void SetupMode();
    void ButtonActivate(Node n, bool shift = false);
    void SoftUnload();
}