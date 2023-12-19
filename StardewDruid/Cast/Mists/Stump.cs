﻿using Force.DeepCloner;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewDruid.Cast.Mists
{
    internal class Stump : CastHandle
    {

        private ResourceClump resourceClump;

        private string resourceType;

        public Stump(Vector2 target, Rite rite, ResourceClump ResourceClump, string ResourceType)
            : base(target, rite)
        {

            resourceClump = ResourceClump;

            resourceType = ResourceType;

        }

        public override void CastEffect()
        {

            int axeLevel = Mod.instance.virtualAxe.UpgradeLevel;

            castCost = Math.Max(2, 36 - targetPlayer.ForagingLevel * axeLevel);

            if (resourceClump == null)
            {

                return;

            }

            ModUtility.DestroyStump(targetLocation, targetPlayer, resourceClump, targetVector, Mod.instance.virtualAxe, resourceType);

            resourceClump = null;

            castFire = true;

            ModUtility.AnimateBolt(targetLocation, targetVector);

            return;

        }

    }
}
