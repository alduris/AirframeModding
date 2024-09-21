using BepInEx;
using BepInEx.Logging;
using CharacterAndBikeEditor;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine.UI;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("alduris.testmod", "Test Mod", "1.0.0")]
sealed class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    private static List<IDetour> Hooks = [];

    public void OnEnable()
    {
        Logger = base.Logger;

        // Get down and dirty with the tabs
        Hooks.Add(new Hook(typeof(CharacterEditor).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance), CharacterEditor_Start));
    }

    public void OnDisable()
    {
        foreach (var hook in Hooks)
        {
            hook.Undo();
            hook.Dispose();
        }
        Hooks.Clear();
    }

    private void CharacterEditor_Start(Action<CharacterEditor> orig, CharacterEditor self)
    {
        orig(self);

        // Add our new tab
        Array.Resize(ref self.editorTabs, self.editorTabs.Length + 1);
        self.editorTabs[^1] = new TestTab(self, self.editorTabs.Length - 1);
        self.SwitchTab((CharacterEditor.EditorTab.TabID)(self.editorTabs.Length - 1), isInit: true);
    }

    public class TestTab : CharacterEditor.EditorTab
    {
        public TestTab(CharacterEditor owner, int id) : base(owner, (TabID)id)
        {
            var testButton = owner.AddComponent<Button>();
            testButton.AddComponent<Text>().text = "Hello";
            testButton.onClick.AddListener(owner.BackToMenu);
        }

        public override void Init()
        {
            base.Init();
        }
    }
}
