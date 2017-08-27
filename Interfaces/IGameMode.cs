using UnityEngine;
using System.Collections;

public interface IGameMode
{
	void IUpdate();
	void Init(GameManager m, int submode = 0);
	void Unload();
}
