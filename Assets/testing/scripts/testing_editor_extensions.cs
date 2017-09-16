// <license file="testing_editor_extensions.cs" repository="https://github.com/DrizzlyBear/u3d_text_3ngine">
//     MIT License https://github.com/DrizzlyBear/u3d_text_3ngine/blob/master/LICENSE
// </license>

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class testing_editor_extensions : MonoBehaviour
{
    /// <summary>
    /// Allows right click adding of a u3d_text_3ngine object in the unity editor
    /// </summary>
    /// <param name="menuCommand">The event args from unity</param>
    [MenuItem("GameObject/UI/fps_display")]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject engine = new GameObject("fps_display");
        engine.AddComponent<fps_display>();

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(engine, menuCommand.context as GameObject);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(engine, "Create " + engine.name);
        Selection.activeObject = engine;
    }
}

#endif