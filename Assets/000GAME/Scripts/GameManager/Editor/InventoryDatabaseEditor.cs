using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(DD_GameManager.InventoryDatabase))]
public class InventoryDatabaseEditor : EditorWithSubEditors<InventoryItemEditor, DD_GameManager.InventoryItem>
{
    private DD_GameManager.InventoryDatabase itemDatabase;
    private SerializedProperty itemsProperty;
    private const string itemsPropName = "items";           // Name of the field for the array of Reactions.

    private const float dropAreaHeight = 50f;               // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;                // Width in pixels between the popup type selection and drop area.
    
    private string searchString = "";

    private void OnEnable()
    {
        // Cache the target.
        itemDatabase = (DD_GameManager.InventoryDatabase)target;

        // Cache the SerializedProperty
        itemsProperty = serializedObject.FindProperty(itemsPropName);

        // If new editors are required for Reactions, create them.
        CheckAndCreateSubEditors(itemDatabase.items);
    }

    private void OnDisable()
    {
        // Destroy all the subeditors.
        CleanupEditors();

    }
    protected override void SubEditorSetup(InventoryItemEditor editor)
    {
        editor.itemsProperty = itemsProperty;
    }

    public override void OnInspectorGUI()
    {
        // Pull all the information from the target into the serializedObject.
        serializedObject.Update();

        // If new editors for Reactions are required, create them.
        CheckAndCreateSubEditors(itemDatabase.items);

        DrawSearchBox();

        // Display all the Reactions.
        for (int i = 0; i < subEditors.Length; i++)
        {
            if(searchString.Length==0 || itemDatabase.items[i].Name.Contains(searchString))
                subEditors[i].OnInspectorGUI();
        }

        // If there are Reactions, add a space.
        if (itemDatabase.items.Length > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        // Create a Rect for the full width of the inspector with enough height for the drop area.
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + EditorGUIUtility.standardVerticalSpacing));

        // Create a Rect for the left GUI controls.
        Rect leftAreaRect = fullWidthRect;


        // It should be in half a space from the top.
        leftAreaRect.y += EditorGUIUtility.standardVerticalSpacing * 0.5f;

        // The width should be slightly less than half the width of the inspector.
        leftAreaRect.width *= 0.5f;
        leftAreaRect.width -= controlSpacing * 0.5f;

        // The height should be the same as the drop area.
        leftAreaRect.height = dropAreaHeight;

        // Create a Rect for the right GUI controls that is the same as the left Rect except...
        Rect rightAreaRect = leftAreaRect;

        // ... it should be on the right.
        rightAreaRect.x += rightAreaRect.width + controlSpacing;

        // Display the GUI for the type popup and button on the left.
        TypeSelectionGUI(fullWidthRect);        

        // Push the information back from the serializedObject to the target.
        serializedObject.ApplyModifiedProperties();
    }

    private void TypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add New Item"))
        {
            DD_GameManager.InventoryItem newItem = (DD_GameManager.InventoryItem)CreateInstance("DD_GameManager.InventoryItem");
            newItem.UID = itemsProperty.arraySize;
            newItem.Name = "Item " + (itemsProperty.arraySize + 1);
            itemsProperty.InsertArrayElementAtIndex(itemsProperty.arraySize);
            itemsProperty.GetArrayElementAtIndex(itemsProperty.arraySize-1).objectReferenceValue = newItem;
            //itemsProperty.AddToObjectArray(newItem);

            /*
            GameObject newItem = new GameObject();
            newItem.transform.parent = itemDatabase.transform;
            newItem.AddComponent<DD_Inventory.InventoryItem>();
            newItem.GetComponent<DD_Inventory.InventoryItem>().UID = itemsProperty.arraySize;
            newItem.GetComponent<DD_Inventory.InventoryItem>().gameObject.name = "Item " + (itemsProperty.arraySize + 1);
            itemsProperty.AddToObjectArray(newItem);
            */
        }
    }

    private void DrawSearchBox()
    {
        return;
        EditorGUIUtility.labelWidth = 0;
        EditorGUIUtility.fieldWidth = 0;
        GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        GUILayout.Label("Filter:", GUILayout.Width(50));
        EditorGUI.BeginChangeCheck();
        searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        {
            // Remove focus if cleared
            searchString = "";
            GUI.FocusControl(null);
        }
        if (EditorGUI.EndChangeCheck())
        {
            
        }
        GUILayout.EndHorizontal();
    }
}
