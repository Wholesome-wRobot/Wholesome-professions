﻿using robotManager.FiniteStateMachine;
using System.Collections.Generic;
using System.Threading;
using Wholesome_Professions_WotlK.Helpers;
using Wholesome_Professions_WotlK.Items;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace Wholesome_Professions_WotlK.States
{
    class CraftOneState : State
    {
        public override string DisplayName
        {
            get { return "Crafting one"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private IProfession profession;

        public override bool NeedToRun
        {
            get
            {
                if (!Conditions.InGameAndConnectedAndAliveAndProductStartedNotInPause || !ObjectManager.Me.IsValid
                    || Conditions.IsAttackedAndCannotIgnore || ObjectManager.Me.MountDisplayId != 0
                    || !WholesomeProfessionsSettings.CurrentSetting.CraftWhileFarming)
                    return false;
                
                if (Main.primaryProfession.ShouldCraftOne())
                {
                    profession = Main.primaryProfession;
                    return true;
                }

                return false;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override void Run()
        {
            Logger.LogDebug("************ RUNNING CRAFT ONE STATE ************");
            Step currentStep = profession.CurrentStep;

            MovementManager.StopMoveNewThread();

            // Deactivate broadcast
            Broadcaster.autoBroadcast = false;

            // Craft
            Logger.Log($"Crafting {currentStep.ItemoCraft.Name}");
            ToolBox.Craft(profession.Name.ToString(), currentStep.ItemoCraft, 1);

            Logger.Log("Craft complete");
            ToolBox.CloseProfessionFrame();
            Lua.RunMacroText("/stopcasting");

            profession.ReevaluateStep();
            profession.SetContext();
            Broadcaster.autoBroadcast = true;
        }
    }
}
