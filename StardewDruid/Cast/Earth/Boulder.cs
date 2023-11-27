﻿using Force.DeepCloner;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewDruid.Cast.Earth
{
    internal class Boulder : CastHandle
    {

        private readonly ResourceClump resourceClump;

        public Boulder(Vector2 target, Rite rite, ResourceClump ResourceClump)
            : base(target, rite)
        {

            resourceClump = ResourceClump;

        }

        public override void CastEffect()
        {

            int debrisType = 390;

            int debrisAmount = randomIndex.Next(1, 5);

            Dictionary<int, Throw> throwList = new();

            for (int i = 0; i < debrisAmount; i++)
            {

                throwList[i] = new(targetPlayer, targetVector*64, debrisType, 0);

                throwList[i].ThrowObject();

                //Game1.createObjectDebris(debrisType, (int)targetVector.X, (int)targetVector.Y + 1);

            }

            if (debrisAmount == 1)
            {

                throwList[1] = new(targetPlayer, targetVector * 64, 382, 0);

                throwList[1].ThrowObject();

                //Game1.createObjectDebris(382, (int)targetVector.X + 1, (int)targetVector.Y);

            }

            castFire = true;

            targetPlayer.gainExperience(2, 2); // gain foraging experience

            ModUtility.AnimateGrowth(targetLocation, targetVector, new(0.8f, 1, 0.8f, 1));

        }

    }
}