using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldEvent {
    int EventID { get; set; }
    void StartEvent(IMode mode);
    void NextEvent();
    void EndEvent();
    void PlayerAction();

}

public enum EndType {
    None,
    EventEnd,
    SceneEnd,
    ChapterEnd,
}