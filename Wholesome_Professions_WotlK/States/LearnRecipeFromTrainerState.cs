﻿using System.Collections.Generic;
using System.Threading;
using robotManager.FiniteStateMachine;
using Wholesome_Professions_WotlK.Helpers;
using wManager.Wow.Bot.Tasks;
using wManager.Wow.Class;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace Wholesome_Professions_WotlK.States
{
    class LearnRecipeFromTrainerState : State
    {
        public override string DisplayName
        {
            get { return "Learning recipe from trainer"; }
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
                    || Conditions.IsAttackedAndCannotIgnore || Main.amountProfessionsSelected <= 0 || Main.primaryProfession.CurrentStep == null)
                    return false;

                if (Main.primaryProfession.CurrentStep != null && Main.primaryProfession.ShouldLearnRecipeFromTrainer())
                {
                    profession = Main.primaryProfession;
                    return true;
                }
                if (Main.secondaryProfession.CurrentStep != null && Main.secondaryProfession.ShouldLearnRecipeFromTrainer())
                {
                    profession = Main.secondaryProfession;
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
            Logger.LogDebug("************ RUNNING LEARN RECIPE FROM TRAINER ************");
            Broadcaster.autoBroadcast = false;

            Step currentStep = profession.CurrentStep;
            Npc trainer = profession.ProfessionTrainer;

            Logger.Log($"Learning {currentStep.itemoCraft.name} at NPC {trainer.Entry}");
            if (GoToTask.ToPositionAndIntecractWithNpc(trainer.Position, trainer.Entry, trainer.GossipOption))
            {
                ToolBox.LearnthisSpell(currentStep.itemoCraft.name);
                Thread.Sleep(1000);
            }

            currentStep.knownRecipe = ToolBox.RecipeIsKnown(currentStep.itemoCraft.name, profession.ProfessionName.ToString());

            Broadcaster.autoBroadcast = true;
        }
    }
}
