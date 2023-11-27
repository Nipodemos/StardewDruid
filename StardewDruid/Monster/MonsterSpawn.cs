﻿using StardewDruid.Map;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewDruid.Monster
{
    public class MonsterSpawn
    {

        public StardewValley.Monsters.Monster targetMonster;

        public GameLocation targetLocation;

        public bool spawnComplete;

        public MonsterSpawn(GameLocation Location, StardewValley.Monsters.Monster Monster)
        {

            targetLocation = Location;

            targetMonster = Monster;

        }

        public void InitiateMonster(int delayTimer)
        {

            DelayedAction.functionAfterDelay(ManifestMonster, delayTimer);

        }

        public void ManifestMonster()
        {

            targetLocation.characters.Add(targetMonster);

            targetMonster.update(Game1.currentGameTime, targetLocation);

            spawnComplete = true;

        }

    }
}
