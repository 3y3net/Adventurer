using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImText : ReactionBase
{
    [TextArea]
    public string message;                      // The text to be displayed to the screen.
    public Color textColor = Color.white;       // The color of the text when it's displayed (different colors for different characters talking).
    public float delay;                         // How long after the React function is called before the text is displayed.


    private TextManager textManager;            // Reference to the component to display the text.


    protected override void SpecificInit()
    {
        textManager = FindObjectOfType<TextManager>();
        if (!textManager)
            throw new UnityException("TextManager could not be found, ensure that it exists in the Persistent or current scene.");
    }


    protected override void ImmediateReaction()
    {
        textManager.DisplayMessage(message, textColor, delay);
    }
}
