using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.ShortcutManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

[EditorToolbarElement(id, typeof(SceneView))]
public class TransformCopyPasteElement : EditorToolbarDropdownToggle, IAccessContainerWindow
{
    public const string NormalCopyShortcut = "Main Menu/Edit/Copy";
    public const string NormalPasteShortcut = "Main Menu/Edit/Paste";
    public const string TransformCopyShortcut = "Main Menu/Tools/Seraph/Copy Transform";
    public const string TransformPasteShortcut = "Main Menu/Tools/Seraph/Paste Transform";
    
    public static bool CopyPosition = true;
    public static bool CopyRotation = true;
    public static bool CopyScale = true;
    
    
    public const string id = "Seraph Toolbar/Transform Copy Paste Options";
    public EditorWindow containerWindow { get; set; }

    public TransformCopyPasteElement()
    {
        text = "Copy/Paste";
        tooltip = "Copy/Paste Transform";
        icon = EditorGUIUtility.IconContent("d_Transform Icon").image as Texture2D;
        
        this.RegisterValueChangedCallback(OnValueChanged);
        
        dropdownClicked += ShowMenu;
    }

    private void ShowMenu()
    {
        var menu = new  GenericMenu();
        menu.AddItem(new GUIContent("Position"), CopyPosition, () => CopyPosition = !CopyPosition);
        menu.AddItem(new GUIContent("Rotation"), CopyRotation, () => CopyRotation = !CopyRotation);
        menu.AddItem(new GUIContent("Scale"), CopyScale, () => CopyScale = !CopyScale);
        menu.ShowAsContext();
    }
    
    private void OnValueChanged(ChangeEvent<bool> evt)
    {
        KeyCombination ctrlC = new KeyCombination(KeyCode.C, ShortcutModifiers.Control);
        KeyCombination ctrlV = new KeyCombination(KeyCode.V, ShortcutModifiers.Control);
        KeyCombination ctrlShiftAltC = new KeyCombination(KeyCode.C, ShortcutModifiers.Control | ShortcutModifiers.Shift | ShortcutModifiers.Alt);
        KeyCombination ctrlShiftAltV = new KeyCombination(KeyCode.V, ShortcutModifiers.Control | ShortcutModifiers.Shift | ShortcutModifiers.Alt);
        if (evt.newValue)
        {
            ShortcutManager.instance.RebindShortcut(NormalCopyShortcut, new ShortcutBinding(ctrlShiftAltC));
            ShortcutManager.instance.RebindShortcut(NormalPasteShortcut, new ShortcutBinding(ctrlShiftAltV));
            ShortcutManager.instance.RebindShortcut(TransformCopyShortcut, new ShortcutBinding(ctrlC));
            ShortcutManager.instance.RebindShortcut(TransformPasteShortcut, new ShortcutBinding(ctrlV));
        }
        else
        {
            ShortcutManager.instance.RebindShortcut(NormalCopyShortcut, new ShortcutBinding(ctrlC));
            ShortcutManager.instance.RebindShortcut(NormalPasteShortcut, new ShortcutBinding(ctrlV));
            ShortcutManager.instance.RebindShortcut(TransformCopyShortcut, new ShortcutBinding(ctrlShiftAltC));
            ShortcutManager.instance.RebindShortcut(TransformPasteShortcut, new ShortcutBinding(ctrlShiftAltV));
        }
    }
}



[Overlay(typeof(SceneView), "Seraph Tools")]
public class SeraphToolbar : ToolbarOverlay
{
    public SeraphToolbar() : base(
        TransformCopyPasteElement.id
    )
    {}
}
