using BepInEx;
using BepInEx.Logging;
using System.Security.Permissions;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("alduris.testmod", "Test Mod", "1.0.0")]
sealed class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    public void OnEnable()
    {
        Logger = base.Logger;
    }
}
