using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{

    public class ExtraGenetronParameters : DefModExtension
    {

        public bool fineTuningControl = false;
        public bool steamTuningControl = false;

        public bool hideOverdrive = false;
        public bool hidePowerSurge = false;
        public bool hideTuning = false;

        public bool noCriticalBreakdowns = false;
        public bool worksInSolarFlares = false;
        public bool nonLinearMaintenanceLoss = false;

        public bool hasNuclearMeltdowns = false;
        public bool anyoneCanHandle = false;

        public HediffDef studyingHediff = null;
        public bool toggleGeothermalStudied = false;
        public bool toggleNuclearStudied = false;

        public string descriptionOverride;
        public ThingDef convertAfterStudying;
        public bool includeInventorFirstName;
        public bool includeInventorFullName;
    }
}
