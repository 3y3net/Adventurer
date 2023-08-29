using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour {

    public GameCursor gameCursor;

    void OnMouseEnter()
    {

    }

    private void OnMouseOver()
    {
        CursorManager.instance.SetCursor(gameCursor);
    }

    private void OnMouseExit()
    {
        CursorManager.instance.SetCursor(GameCursor.ModeNormal);
    }
}
