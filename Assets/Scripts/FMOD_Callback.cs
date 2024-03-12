using System;
using UnityEngine;

[CreateAssetMenu(menuName = "My FMOD Callback Handler")]
public class FMOD_Callback : FMODUnity.PlatformCallbackHandler
{
    public override void PreInitialize(FMOD.Studio.System studioSystem, Action<FMOD.RESULT, string> reportResult)
    {
        FMOD.RESULT result;

        FMOD.System coreSystem;
        result = studioSystem.getCoreSystem(out coreSystem);
        reportResult(result, "studioSystem.getCoreSystem");
        
        // Set up studioSystem and coreSystem as desired
        studioSystem.getAdvancedSettings(out FMOD.Studio.ADVANCEDSETTINGS advancedSettings);
        advancedSettings.studioupdateperiod = 10;
        studioSystem.setAdvancedSettings(advancedSettings);
    }
}
