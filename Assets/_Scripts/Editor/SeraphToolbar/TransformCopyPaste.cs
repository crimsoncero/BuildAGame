using UnityEditor;
using UnityEngine;

public class TransformCopyPaste : EditorWindow
{
    private static Transform _copiedTransform;
    private static bool _hasCopied;

    [MenuItem("Tools/Seraph/Copy Transform %#&c")]
    private static void CopyTransform()
    {
        if (Selection.activeTransform != null)
        {
            _copiedTransform = Selection.activeTransform;
            _hasCopied = true;
            Debug.Log("Transform copied from: " + Selection.activeTransform.name);
        }
        else
        {
            Debug.LogWarning("No object selected to paste transform from.");
        }
    }
    
    [MenuItem("Tools/Seraph/Paste Transform %#&v")]
    private static void PasteTransform()
    {
        if (!_hasCopied)
        {
            Debug.LogWarning("No transform data copied yet.");
            return;
        }

        if (Selection.activeTransform != null)
        {
            Undo.RecordObject(Selection.activeTransform, "Paste Transform");
            
            if(TransformCopyPasteElement.CopyPosition)
                Selection.activeTransform.position = _copiedTransform.position;
            
            if(TransformCopyPasteElement.CopyRotation)
                Selection.activeTransform.rotation = _copiedTransform.rotation;
            
            if(TransformCopyPasteElement.CopyScale)
                Selection.activeTransform.localScale = _copiedTransform.lossyScale;
            
            Debug.Log("Transform pasted to: " + Selection.activeTransform.name);
        }
        else
        {
            Debug.LogWarning("No object selected to paste transform to.");
        }
    }
    
}
