﻿using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewDruid.Cast
{
    public class Rite
    {

        public Dictionary<string, bool> spawnIndex;

        public string castType;

        public int castLevel;

        public int castCycle;

        public Vector2 castVector;

        public StardewValley.Farmer caster;

        public StardewValley.GameLocation castLocation;

        public int direction;

        public StardewValley.Tools.Axe castAxe;

        public StardewValley.Tools.Pickaxe castPick;

        public int castDamage;

        public Rite()
        {

            castLevel = 1;

            castType = "CastEarth";

            caster = Game1.player;

            castLocation = caster.currentLocation;

            castVector = caster.getTileLocation();

            spawnIndex = Map.SpawnData.SpawnIndex(castLocation);

            direction = 0;

            castDamage = 10;

        }

    }

}
