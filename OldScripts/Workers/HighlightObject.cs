using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An object that can be selected, inspected and/or clicked
// This scripts will be run only when GameModer = ZoomArea

public class HighlightObject : MonoBehaviour
{
    public float actionlDistance = 4.0f;        //Minimun distace from player to object needed to show label
    public float lightDistance = 4.0f;          //Minimun distace from player to object needed to switch highlight on
    public Color lightColor = Color.cyan;
    public float labelDistance = 2.0f;          //Minimun distace from player to object needed to show label
    public Color labelColor = Color.white;      //Text color
    public bool outlined = true;                //Set true for display outlined text
    public Color outlineColor = Color.black;    //Outline text color
    public Font textFont = null;                //Text font
    public int fontSize = 52;					//Text size
    public string labelToDisplay;
    public Vector2 labelDelta = new Vector2(149, 20);
    public SceneManager.GameCursor cursor;      //Cursor to show

    public bool isActive = true;

    protected Highlighter h;

    private GUIStyle style;
    bool showLabel = false;

    public enum OnClickAction { None, ShowInfo, ShowInfoAndExit, Inspect, CallFunction}

    public OnClickAction clickAction, exitAction;
    public int infoId, exitId;

    protected void Awake()
    {
        Camera cm= HighlighterManager.instance.usedCamera;
        h = GetComponent<Highlighter>();
        if (h == null) { h = gameObject.AddComponent<Highlighter>(); }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        style = new GUIStyle();
        style.normal.textColor = labelColor;
        style.alignment = TextAnchor.UpperCenter;
        style.wordWrap = true;
        style.fontSize = fontSize;
        if (textFont != null)
            style.font = textFont;
        else
            style.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.instance.gameMode != SceneManager.GameMode.ZoomArea)
            return;
    }

    public void MouseOver(float distance, GameObject target = null)
    {        
        if (!isActive || SceneManager.instance.gameMode != SceneManager.GameMode.ZoomArea)
        {
            showLabel = false;
            return;
        }        

        // Highlight object for one frame in case MouseOver event has arrived
        if (distance <= lightDistance)
            h.Hover(lightColor);
        
        if (distance <= labelDistance)
            showLabel = true;
        else
            showLabel = false;
        if (distance <= actionlDistance) { }
        
    }
    
    public void MouseEnter(float distance)
	{
        Debug.Log("Enter "+distance);
	}

    public void MouseExit(float distance)
    {
        if (!isActive)
            return;
        showLabel = false;
    }

    public void Fire1(float distance)
	{
        Debug.Log("Fire1");
    }

    public void Fire2(float distance)
    {
        Debug.Log("Fire2");
    }

    void OnGUI()
    {
        if (labelToDisplay.Equals("") || !showLabel || SceneManager.instance.gameMode != SceneManager.GameMode.ZoomArea)
            return;

        float x = Event.current.mousePosition.x;
        float y = Event.current.mousePosition.y - 10;
        //Debug.Log(SMGlobalConfiguration.instance.mouseCursor.transform.position.y + " - " + Event.current.mousePosition.y);
        if (outlined)
            DrawOutline(new Rect(x - labelDelta.x, y - labelDelta.y, 300, 60), labelToDisplay, style, outlineColor, labelColor);
        else
            GUI.Label(new Rect(x - labelDelta.x, y - labelDelta.y, 300, 60), labelToDisplay, style);
    }

    //draw text of a specified color, with a specified outline color
    void DrawOutline(Rect position, string text, GUIStyle theStyle, Color outColor, Color inColor)
    {
        var backupStyle = theStyle;
        theStyle.normal.textColor = outColor;
        position.x--;
        GUI.Label(position, text, style);
        position.x += 2;
        GUI.Label(position, text, style);
        position.x--;
        position.y--;
        GUI.Label(position, text, style);
        position.y += 2;
        GUI.Label(position, text, style);
        position.y--;
        theStyle.normal.textColor = inColor;
        GUI.Label(position, text, style);
        theStyle = backupStyle;
    }
}
