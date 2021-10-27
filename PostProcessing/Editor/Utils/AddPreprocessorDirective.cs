using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if !UNITY_POST_PROCESSING_STACK_V2_CONTROLLABLE
public static class AddPreprocessorDirective
{
    const string DEFINE = "UNITY_POST_PROCESSING_STACK_V2_CONTROLLABLE";

    [InitializeOnLoadMethod]
    private static void Init()
    {
        // Get current defines
        var currentDefinesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        // Split at ;
        var defines = currentDefinesString.Split(';').ToList();
        // check if defines already exist given define
        if (!defines.Contains(DEFINE))
        {
            // if not add it at the end with a leading ; separator
            currentDefinesString += $";{DEFINE}";

            // write the new defines back to the PlayerSettings
            // This will cause a recompilation of your scripts
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefinesString);
        }
    }
}
#endif
