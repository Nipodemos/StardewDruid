﻿using Microsoft.Xna.Framework;
using StardewDruid.Cast;
using StardewDruid.Map;
using StardewDruid.Monster;
using StardewValley;
using System.Collections.Generic;

namespace StardewDruid.Event
{
    public class Infestation : ChallengeHandle
    {

        public BossSlime bossMonster;

        public Infestation(Mod Mod, Vector2 target, Rite rite, Quest quest)
            : base(Mod, target, rite, quest)
        {

        }

        public override void EventTrigger()
        {

            challengeSpawn = new(){0,};
            challengeFrequency = 1;
            challengeAmplitude = 1;
            challengeSeconds = 60;
            challengeWithin = new(71, 70);
            challengeRange = new(16, 15);
            challengeTorches = new()
            {
                new(75, 74),
                new(82, 74),
                new(75, 81),
                new(82, 81),
            };

            if (questData.name.Contains("Two"))
            {
                challengeFrequency = 2;
                challengeAmplitude = 3;
            }

            SetupSpawn();

            Game1.addHUDMessage(new HUDMessage($"Defeat the slimes!", "2"));

            Game1.changeMusicTrack("tribal", false, Game1.MusicContext.Default);

            mod.RegisterChallenge(this, "active");

        }

        public override void EventRemove()
        {

            if (bossMonster != null)
            {

                riteData.castLocation.characters.Remove(bossMonster);

                bossMonster = null;

            }

            base.EventRemove();

        }

        public override void EventReward()
        {

            List<string> NPCIndex = Map.VillagerData.VillagerIndex("forest");

            Game1.addHUDMessage(new HUDMessage($"You have gained favour with those who love the forest", ""));

            mod.CompleteQuest(questData.name);

            if (!questData.name.Contains("Two"))
            {

                UpdateFriendship(NPCIndex);

                mod.UpdateEffigy(questData.name);

                mod.LevelBlessing("stars");

            }

        }

        public override void EventInterval()
        {

            activeCounter++;

            monsterHandle.SpawnInterval();

            if (activeCounter == 19)
            {

                StardewValley.Monsters.Monster theMonster = MonsterData.CreateMonster(13, new(79, 72), riteData.combatModifier);

                bossMonster = theMonster as BossSlime;

                bossMonster.posturing = true;

                riteData.castLocation.characters.Add(bossMonster);

                bossMonster.update(Game1.currentGameTime, riteData.castLocation);

            }

            if (activeCounter <= 19)
            {
                return;
            }

            if (bossMonster.Health >= 1)
            {
                switch (activeCounter)
                {

                    case 15: bossMonster.showTextAboveHead("HOW BORING", 3000); break;

                    case 18: bossMonster.showTextAboveHead("the monarchs must be asleep.", 3000); break;

                    case 21: bossMonster.showTextAboveHead("if they send only a farmer", 3000); break;

                    case 24: bossMonster.showTextAboveHead("to face the onslaught...", 3000); break;

                    case 27: bossMonster.showTextAboveHead("OF THE MIGHTY SLIME", 3000); break;

                    case 30: bossMonster.showTextAboveHead("you will be consumed", 3000); break;

                    case 33: bossMonster.showTextAboveHead("along with the whole valley", 3000); break;

                    case 36: bossMonster.showTextAboveHead("ALL WILL BE JELLY", 3000); break;

                    case 37:

                        bossMonster.posturing = false;
                        
                        bossMonster.focusedOnFarmers = true; 
                        
                        break;

                    case 57:

                        bossMonster.showTextAboveHead("bloop?");

                        bossMonster.Halt();

                        break;

                    case 58:

                        bossMonster.showTextAboveHead("that's a lot of star power");

                        bossMonster.Halt();

                        break;

                    case 59:

                        bossMonster.showTextAboveHead("!!!!");

                        Vector2 meteorVector = bossMonster.getTileLocation();

                        ModUtility.AnimateMeteorZone(riteData.castLocation, meteorVector + new Vector2(-2, 1), Color.Red);

                        ModUtility.AnimateMeteor(riteData.castLocation, meteorVector + new Vector2(-2, 1), true);

                        ModUtility.AnimateMeteorZone(riteData.castLocation, meteorVector + new Vector2(1, -2), Color.Red);

                        ModUtility.AnimateMeteor(riteData.castLocation, meteorVector + new Vector2(1, -2), true);

                        ModUtility.AnimateMeteorZone(riteData.castLocation, meteorVector + new Vector2(2, 1), Color.Red);

                        ModUtility.AnimateMeteor(riteData.castLocation, meteorVector + new Vector2(2, 1), false);

                        ModUtility.AnimateMeteorZone(riteData.castLocation, meteorVector + new Vector2(1, 2), Color.Red);
                        
                        ModUtility.AnimateMeteor(riteData.castLocation, meteorVector + new Vector2(1, 2), false);

                        DelayedAction.functionAfterDelay(MeteorImpact, 600);

                        break;

                    default: break;

                }

            }

        }

        public void MeteorImpact()
        {

            List<Vector2> impactVectors;

            Vector2 impactCenter = bossMonster.getTileLocation();

            for (int i = 0; i < 5; i++)
            {

                impactVectors = ModUtility.GetTilesWithinRadius(riteData.castLocation, impactCenter, i);

                foreach (Vector2 impactVector in impactVectors)
                {

                    ModUtility.ImpactVector(targetLocation, impactVector);

                }

            }

            bossMonster.takeDamage(bossMonster.MaxHealth + 5, 0, 0, false, 999, targetPlayer);


        }

    }
}
