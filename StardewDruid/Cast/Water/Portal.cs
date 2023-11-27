﻿using Microsoft.Xna.Framework;
using StardewDruid.Map;
using StardewDruid.Monster;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using xTile.Layers;
using xTile.Tiles;

namespace StardewDruid.Cast.Water
{
    internal class Portal : CastHandle
    {

        public Portal(Vector2 target, Rite rite)
            : base(target, rite)
        {

            castCost = 0;

        }

        public override void CastEffect()
        {

            Event.World.Portal portalEvent = new(targetVector, riteData);

            portalEvent.EventTrigger();

            castLimit = true;

            castFire = true;

        }

    }

}