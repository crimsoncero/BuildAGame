using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorSprite;

    void Start()
    {
        Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
    }
}
