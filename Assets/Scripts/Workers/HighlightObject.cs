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
    public int fontSize = 12;					//Text size
    public string labelToDisplay;
    public SceneManager.GameCursor cursor;      //Cursor to show

    public bool isActive = true;

    protected Highlighter h;

    private GUIStyle style;
    bool showLabel = false;

    public enum OnClickAction { None, ShowInfo, ShowInfoAndExit, Inspect, CallFunction}

    protected void Awake()
    {
        Camera cm= HighlighterManager.instance.camera;
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
        int a;
        if (distance <= actionlDistance)
            a = 0;
        else
            a = 1;
    }
    
    public void MouseEnter(float distance)
	{
        Debug.Log("Enter");
	}

    public void MouseExit(float distance)
    {
        if (!isActive)
            return;
        showLabel = false;
    }

    public void Fire1(float distance)
	{

	}

    public void Fire2(float distance)
    {

    }

    void OnGUI()
    {
        if (labelToDisplay.Equals("") || !showLabel || SceneManager.instance.gameMode != SceneManager.GameMode.ZoomArea)
            return;

        float x = Event.current.mousePosition.x;
        float y = Event.current.mousePosition.y - 10;
        //Debug.Log(SMGlobalConfiguration.instance.mouseCursor.transform.position.y + " - " + Event.current.mousePosition.y);
        if (outlined)
            DrawOutline(new Rect(x - 149, y - 20, 300, 60), labelToDisplay, style, outlineColor, labelColor);
        else
            GUI.Label(new Rect(x - 149, y - 20, 300, 60), labelToDisplay, style);
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
