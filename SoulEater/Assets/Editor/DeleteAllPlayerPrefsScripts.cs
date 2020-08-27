using UnityEngine;
using UnityEditor;

public class DeleteAllPlayerPrefsScripts : EditorWindow
{
    [MenuItem("Window/Delete PlayerPrefs (All)")]
    static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
