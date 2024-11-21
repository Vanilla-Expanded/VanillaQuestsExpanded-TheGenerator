
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public class Command_SetTargetUraniumLevel : Command
    {
        public CompRefuelableWithOverdrive refuelable;

        private List<CompRefuelableWithOverdrive> refuelables;

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            if (refuelables == null)
            {
                refuelables = new List<CompRefuelableWithOverdrive>();
            }
            if (!refuelables.Contains(refuelable))
            {
                refuelables.Add(refuelable);
            }
            int num = int.MaxValue;
            for (int i = 0; i < refuelables.Count; i++)
            {
                if ((int)refuelables[i].Props.fuelCapacity < num)
                {
                    num = (int)refuelables[i].Props.fuelCapacity;
                }
            }
            int startingValue = num / 2;
            for (int j = 0; j < refuelables.Count; j++)
            {
                if ((int)refuelables[j].TargetFuelLevel <= num)
                {
                    startingValue = (int)refuelables[j].TargetFuelLevel;
                    break;
                }
            }
            Func<int, string> textGetter =  (Func<int, string>)((int x) => "VQE_SetTargetUraniumLevelTo".Translate(x)+"\n\n"+
            "VQE_NuclearPowerOutput".Translate(5 * x * x + 50 * x) + "\n\n" + "VQE_NuclearFuelConsumption".Translate((refuelables.First().constant1 * x * x + refuelables.First().constant2 * x).ToStringDecimalIfSmall()));
            Dialog_Slider dialog_Slider = new Dialog_Slider(textGetter, 0, num, delegate (int value)
            {
                for (int k = 0; k < refuelables.Count; k++)
                {
                    refuelables[k].TargetFuelLevel = value;
                }
            }, startingValue);
            
                dialog_Slider.extraBottomSpace = Text.LineHeight*3 + 4f;
            
            Find.WindowStack.Add(dialog_Slider);
        }

        public override bool InheritInteractionsFrom(Gizmo other)
        {
            if (refuelables == null)
            {
                refuelables = new List<CompRefuelableWithOverdrive>();
            }
            refuelables.Add(((Command_SetTargetUraniumLevel)other).refuelable);
            return false;
        }
    }
}