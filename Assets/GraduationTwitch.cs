using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using wawa.TwitchPlays;
using wawa.TwitchPlays.Domains;

public sealed class GraduationTwitch : Twitch<GraduationModule>
{
    enum MinorVal
    {
        [Alias("0")]
        Forensics,
        [Alias("1")]
        AerospaceEngineering,
        [Alias("2")]
        PublicHealth,
        [Alias("3")]
        ComputerScience,
        [Alias("4")]
        Sociology,
        [Alias("5")]
        CreativeWriting,
        [Alias("6")]
        French,
        [Alias("7")]
        Communications,
        [Alias("8")]
        Business,
        [Alias("9")]
        InternationalRelations,
    }

    [Command]
    private IEnumerable<Instruction> Minor(MinorVal name)
    {
        if (!Module.Status.IsSolved && !Module._isExtra)
        {
            yield return null;
            int target = Array.IndexOf(Enum.GetValues(typeof(MinorVal)), name);
            while (Module._selectedMinor != target)
            {
                yield return Module._minorButtonRight;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    [Command]
    private IEnumerable<Instruction> Course(int val)
    {
        if (!Module.Status.IsSolved && !Module._isExtra)
        {
            if (val >= 1 && val <= 3)
            {
                yield return null;
                Module._courseButtons[val - 1].OnInteract();
            }
            else
            {
                yield return TwitchString.SendToChatError("Selection must be within the range 1-3!", false);
            }
        }
    }

    [Command]
    private IEnumerable<Instruction> Press(params int[] vals)
    {
        if (Module._isExtra && (Module._currentStage == 0 || Module._currentStage == 1))
        {
            int length = Module._extraColorSource.Length;
            if (vals.Any(val => val < 1 || val > length))
            {
                yield return TwitchString.SendToChatError($"Selections must be within the range 1-{length}!", false);
            }
            else
            {
                yield return null;
                foreach (int val in vals)
                {
                    yield return (Module._currentStage == 0 ? Module._extra1Selectables : Module._extra2Selectables)[val - 1];
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    [Command]
    private IEnumerable<Instruction> Key(int val)
    {
        if (Module._isExtra && Module._currentStage == 2)
        {
            if (val >= 1 && val <= 16)
            {
                yield return null;
                while (Module._extra3Selected != val - 1)
                {
                    yield return Module._extra3Right;
                    yield return new WaitForSeconds(0.05f);
                }
                yield return Module._extra3Submit;
            }
            else
            {
                yield return TwitchString.SendToChatError("Key must be within the range 1-16!", false);
            }
        }
    }

    public override IEnumerable<Instruction> ForceSolve()
    {
        if (Module._isExtra)
        {
            switch (Module._currentStage)
            {
                case 0:
                case 1:
                    foreach (int i in Module._extraColorSequence.Skip(Module._extraColorProgress))
                    {
                        (Module._currentStage == 0 ? Module._extra1Selectables : Module._extra2Selectables)[Array.IndexOf(Module._extraColorSource, i)].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;
                case 2:
                    while (Module._extra3Selected != Module._extra3Solution)
                    {
                        Module._extra3Right.OnInteract();
                        yield return new WaitForSeconds(0.05f);
                    }
                    Module._extra3Submit.OnInteract();
                    break;
            }
        }

        while (Module._selectedMinor != Module._correctMinor)
        {
            Module._minorButtonRight.OnInteract();
            yield return new WaitForSeconds(0.05f);
        }

        Module._isForceSolve = true;

        while (!Module.Status.IsSolved)
        {
            Module._courseButtons[Module._correctCourse].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
