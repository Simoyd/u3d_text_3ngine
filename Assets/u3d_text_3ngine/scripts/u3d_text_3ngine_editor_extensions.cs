// <license file="u3d_text_3ngine_editor_extensions.cs" repository="https://github.com/DrizzlyBear/u3d_text_3ngine">
//     MIT License https://github.com/DrizzlyBear/u3d_text_3ngine/blob/master/LICENSE
// </license>

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

/// <summary>
/// Unity editor extensions for u3d_text_3ngine object
/// </summary>
public class u3d_text_3ngine_editor_extensions : MonoBehaviour
{
    /// <summary>
    /// Validates right click selection before we create the new object
    /// </summary>
    /// <param name="menuCommand">The parent object</param>
    /// <returns>True to create, otherwise false</returns>
    [MenuItem("GameObject/UI/u3d_text_3ngine", validate = true)]
    static bool ValidateCreateCustomGameObject(MenuCommand menuCommand)
    {
        // Ensure the user right clicked an existing object, not created at the root level
        GameObject parentObject = menuCommand.context as GameObject;

        if (!(parentObject is GameObject))
        {
            EditorUtility.DisplayDialog("Invalid Parent", "u3d_text_3ngine must have a Canvas ancestor", "OK");
            return false;
        }

        // Ensure there is a Canvas ancestor, otherwise TMP will not render and instead just throw errors
        Transform transform = parentObject.transform;

        while (transform != null)
        {
            if (transform.gameObject.GetComponent<Canvas>() != null)
            {
                // Found an ancestor
                return true;
            }

            transform = transform.parent;
        }

        // Didn't find an ancestor
        EditorUtility.DisplayDialog("Invalid Parent", "u3d_text_3ngine must have a Canvas ancestor", "OK");
        return false;
    }

    /// <summary>
    /// Allows right click adding of a u3d_text_3ngine object in the unity editor
    /// </summary>
    /// <param name="menuCommand">The event args from unity</param>
    [MenuItem("GameObject/UI/u3d_text_3ngine")]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject engine = new GameObject("u3d_text_3ngine");
        engine.AddComponent<u3d_text_3ngine>();
        RectTransform rect = engine.GetComponent<RectTransform>();

        // Get the parent rect
        RectTransform parentRect = null;
        Transform transform = (menuCommand.context as GameObject).transform;

        while (parentRect == null)
        {
            parentRect = transform.gameObject.GetComponent<RectTransform>();
            transform = transform.parent;
        }

        // Size the rect to 80% of the parent so we can see it
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = new Vector2(parentRect.rect.width * 0.2f, parentRect.rect.height * 0.2f);
        rect.offsetMax = new Vector2(0, 0);

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(engine, menuCommand.context as GameObject);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(engine, "Create " + engine.name);
        Selection.activeObject = engine;
    }
}

#endif
