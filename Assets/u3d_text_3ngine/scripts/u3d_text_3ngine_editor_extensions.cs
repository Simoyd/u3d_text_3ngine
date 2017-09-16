// <license file="u3d_text_3ngine_editor_extensions.cs" repository="https://github.com/DrizzlyBear/u3d_text_3ngine">
//     MIT License https://github.com/DrizzlyBear/u3d_text_3ngine/blob/master/LICENSE
// </license>

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class u3d_text_3ngine_editor_extensions : MonoBehaviour
{
    [MenuItem("GameObject/UI/u3d_text_3ngine", validate = true)]
    static bool ValidateCreateCustomGameObject(MenuCommand menuCommand)
    {
        GameObject parentObject = menuCommand.context as GameObject;

        if (!(parentObject is GameObject))
        {
            EditorUtility.DisplayDialog("Invalid Parent", "u3d_text_3ngine must have a Canvas anscestor", "OK");
            return false;
        }

        Transform transform = parentObject.transform;

        while (transform != null)
        {
            if (transform.gameObject.GetComponent<Canvas>() != null)
            {
                return true;
            }

            transform = transform.parent;
        }

        EditorUtility.DisplayDialog("Invalid Parent", "u3d_text_3ngine must have a Canvas anscestor", "OK");
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
        }

        // Size the rect to 80% of the parent so we can see it
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = new Vector2(parentRect.rect.width * 0.2f, parentRect.rect.height * 0.2f);
        rect.offsetMax = new Vector2(0, 0); // opposite top

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(engine, menuCommand.context as GameObject);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(engine, "Create " + engine.name);
        Selection.activeObject = engine;
    }
}

#endif
