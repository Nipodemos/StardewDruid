﻿using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using System.Collections.Generic;
using System;

namespace StardewDruid.Monster
{
    public class Shadow : ShadowBrute
    {

        public List<string> ouchList;

        public int spawnBuff;

        public int spawnDamage;

        public Shadow(Vector2 position, int combatModifier)
            : base(position * 64)
        {

            focusedOnFarmers = true;

            Health = combatModifier;

            MaxHealth = Health;

            DamageToFarmer = 0;

            spawnDamage = (int)Math.Max(2, combatModifier * 0.1);

            spawnBuff = 60;

            objectsToDrop.Clear();

            objectsToDrop.Add(769);

            if (Game1.random.Next(3) == 0)
            {
                objectsToDrop.Add(768);
            }
            else if (Game1.random.Next(4) == 0 && combatModifier >= 120)
            {
                List<int> shadowGems = new()
                {
                    62,66,68,70,
                };

                objectsToDrop.Add(shadowGems[Game1.random.Next(shadowGems.Count)]);

            }
            else if (Game1.random.Next(5) == 0 && combatModifier >= 240)
            {
                List<int> shadowGems = new()
                {
                    60,64,72,
                };

                objectsToDrop.Add(shadowGems[Game1.random.Next(shadowGems.Count)]);
            }

            ouchList = new()
            {
                "oooft",
                "deep",
            };

        }
        public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, Farmer who)
        {
            if (spawnBuff > 0)
            {
                return 0;
            }

            int ouchIndex = Game1.random.Next(10);

            if (ouchIndex < ouchList.Count)
            {
                showTextAboveHead(ouchList[ouchIndex], duration: 2000);
            }

            return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision, who);

        }
        public override void behaviorAtGameTick(GameTime time)
        {

            if (spawnBuff > 0)
            {

                spawnBuff--;

                if (spawnBuff < 1)
                {

                    DamageToFarmer = spawnDamage;

                }

            }

            base.behaviorAtGameTick(time);

        }
    }
}
