using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using Funnies.Commands;
using Funnies.Modules;

namespace Funnies;


 
public class FunniesPlugin : BasePlugin, IPluginConfig<FunniesConfig>
{
    public override string ModuleName => "Funny plugin";
    public override string ModuleVersion => "0.0.1";

    public FunniesConfig Config { get; set; }

    public override void Load(bool hotReload)
    {

        Globals.Plugin = this;

        RegisterListener<Listeners.CheckTransmit>(OnCheckTransmit);
        RegisterListener<Listeners.OnTick>(OnTick);

        #if DEBUG
        AddCommand("css_debug", "Debug command", CommandDebug.OnDebugCommand);
        #endif

        Invisible.Setup();
        Wallhack.Setup();
        SelfDamage.Setup();
    }

    public override void Unload(bool hotReload)
    {
        #if DEBUG
        if (hotReload)
        {
            Invisible.Cleanup();
            Wallhack.Cleanup();
        }
        #else
        Console.WriteLine($"Reloading: hotReload? {hotReload}");
        #endif
    }

    public void OnTick()
    {
        Invisible.OnTick();
    }

    public void OnCheckTransmit(CCheckTransmitInfoList infoList)
    {
        foreach ((CCheckTransmitInfo info, CCSPlayerController? player) in infoList)
        {
            if (!Util.IsPlayerValid(player))
                continue;

            Wallhack.OnPlayerTransmit(info, player!);
            Invisible.OnPlayerTransmit(info, player!);
        }
    }

    public void OnConfigParsed(FunniesConfig config)
    {
        Config = config;
        Globals.Config = config;
    }
}
