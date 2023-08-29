using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(DD_GameManager.InventoryItem))]
public class InventoryItemEditor : Editor
{
    public bool showItem;                       // Is the Reaction editor expanded?
    public SerializedProperty itemsProperty;
    private const float buttonWidth = 30f;

    protected DD_GameManager.InventoryItem inventoryItem;
    protected bool cached = false;
    protected bool loading = false;
    protected Texture2D newIcon =null;

    private void OnEnable()
    {
        inventoryItem = (DD_GameManager.InventoryItem)target;
    }

    // This function should be overridden by inheriting classes that need initialisation.
    protected virtual void Init() { }


    public override void OnInspectorGUI()
    {
        // Pull data from the target into the serializedObject.
        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();

        // Display a foldout for the Reaction with a custom label.
        showItem = EditorGUILayout.Foldout(showItem, GetFoldoutLabel());

        // Show a button which, if clicked, will remove this Reaction from the ReactionCollection.
        if (GUILayout.Button("-", GUILayout.Width(buttonWidth)))
        {

            for (int i = 0; i < itemsProperty.arraySize; i++)
                if (itemsProperty.GetArrayElementAtIndex(i).objectReferenceValue == target)
                {
                    itemsProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
            //EditorApplication.delayCall += () => DestroyImmediate(inventoryItem.gameObject);
        }
        EditorGUILayout.EndHorizontal();

        // If the foldout is open, draw the GUI specific to the inheriting ReactionEditor.
        if (showItem)
        {
            DrawItem();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        // Push data back from the serializedObject to the target.
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawItem()
    {
        serializedObject.Update();

        inventoryItem.UID = EditorGUILayout.IntField("Unique identifier", inventoryItem.UID);

        EditorGUI.BeginChangeCheck();
        inventoryItem.Name = EditorGUILayout.TextField("Unique name",  inventoryItem.Name);
        if (EditorGUI.EndChangeCheck())
        {
            //inventoryItem.gameObject.name = inventoryItem.Name;
        }

        inventoryItem.stackAmount = EditorGUILayout.IntField("Max. stack in slot", inventoryItem.stackAmount);
        inventoryItem.allowRecipes = EditorGUILayout.Toggle("Elegible for recipes?", inventoryItem.allowRecipes);
        EditorGUILayout.LabelField("Item Description");
        inventoryItem.description = EditorGUILayout.TextArea(inventoryItem.description, GUILayout.Height(50));

        EditorGUILayout.LabelField("");

        EditorGUI.BeginChangeCheck();

        inventoryItem.prefab = (GameObject) EditorGUILayout.ObjectField("Asociated Prefab", inventoryItem.prefab, typeof(GameObject), false);
        inventoryItem.getPrefabIcon = EditorGUILayout.Toggle("Get Icon from Prefab?", inventoryItem.getPrefabIcon);
        
        if (EditorGUI.EndChangeCheck())
        {
            if(inventoryItem.prefab!=null && inventoryItem.getPrefabIcon)
            {
                cached = false;
                loading = true;
            }
        }
        EditorGUILayout.HelpBox("Icon to show in inventory. If you set the 'Icon from Prefab' option, the icon is generated automatically but you can override it at any time", MessageType.None, true);
        inventoryItem.Icon = (Texture2D)EditorGUILayout.ObjectField("Icon", inventoryItem.Icon, typeof(Texture2D), false);
        TestForUpdates();
    }

    void TestForUpdates()
    {
        if (inventoryItem.prefab == null)
            return;
        if (inventoryItem.prefab != null && inventoryItem.getPrefabIcon && !cached && loading)
        {
            newIcon = AssetPreview.GetAssetPreview(inventoryItem.prefab.gameObject);
            loading = true;
        }
        if (!AssetPreview.IsLoadingAssetPreview(inventoryItem.prefab.gameObject.GetInstanceID()) && loading)
        {
            loading = false;
        }
        
        if (inventoryItem.prefab != null && inventoryItem.getPrefabIcon && !cached && !loading)
        {
            if (!newIcon)
                return;
            cached = true;
            Debug.Log(inventoryItem.prefab.name);
            Debug.Log("w: " + newIcon.width + " - h: " + newIcon.height);
            int resize = (int)((float)newIcon.width / 1f);
            inventoryItem.Icon = IconUtils.AssetPreviewTransparent(newIcon, resize);
        }
    }

    protected string GetFoldoutLabel()
    {
        return inventoryItem.Name;
    }
}
