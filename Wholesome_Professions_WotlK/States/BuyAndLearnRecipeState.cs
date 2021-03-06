﻿using System;
using System.Collections.Generic;
using System.Threading;
using robotManager.FiniteStateMachine;
using Wholesome_Professions_WotlK.Helpers;
using wManager.Wow.Bot.Tasks;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace Wholesome_Professions_WotlK.States
{
    class BuyAndLearnRecipeState : State
    {
        public override string DisplayName
        {
            get { return "Buying and learning recipe"; }
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
                    || Conditions.IsAttackedAndCannotIgnore)
                    return false;

                if (Main.primaryProfession.ShouldBuyAndLearnRecipe())
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
            Logger.LogDebug("************ RUNNING BUY AND LEARN RECIPE STATE ************");

            Step currentStep = profession.CurrentStep;
            var RecipeVendor = currentStep.ItemoCraft.RecipeVendor;
            Logger.Log($"Buying {currentStep.ItemoCraft.Name} recipe at NPC {profession.ProfessionTrainer.Entry}");

            // Check if continent ok
            if ((ContinentId)Usefuls.ContinentId != RecipeVendor.ContinentId)
            {
                Logger.Log($"The vendor is on continent {RecipeVendor.ContinentId}, launching traveler");
                Bot.SetContinent(RecipeVendor.ContinentId);
                return;
            }

            Broadcaster.autoBroadcast = false;
            
            if (GoToTask.ToPositionAndIntecractWithNpc(RecipeVendor.Position, RecipeVendor.Entry, RecipeVendor.GossipOption))
            {
                Vendor.BuyItem(ItemsManager.GetNameById(currentStep.ItemoCraft.RecipeItemId), 1);
                Thread.Sleep(2000);

                ItemsManager.UseItemByNameOrId(currentStep.ItemoCraft.RecipeItemId.ToString());
                Usefuls.WaitIsCasting();
                Thread.Sleep(300);
            }

            currentStep.KnownRecipe = ToolBox.RecipeIsKnown(currentStep.ItemoCraft.Name, profession);

            Broadcaster.autoBroadcast = true;
        }
    }
}
