using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(Zombones.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(Zombones.BuildInfo.Company)]
[assembly: AssemblyProduct(Zombones.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + Zombones.BuildInfo.Author)]
[assembly: AssemblyTrademark(Zombones.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(Zombones.BuildInfo.Version)]
[assembly: AssemblyFileVersion(Zombones.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(Zombones.Zombones), Zombones.BuildInfo.Name, Zombones.BuildInfo.Version, Zombones.BuildInfo.Author, Zombones.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("", "")]