using UnityEngine;
using System.Collections;

public interface ISubGameMode
{
	void Update();
	void Init(GameManager m, IGameMode game);
	void Unload();
}
