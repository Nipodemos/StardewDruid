﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewDruid.Character;
using StardewDruid.Data;
using StardewDruid.Event;
using StardewDruid.Render;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Enchantments;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using xTile.Layers;
using static StardewDruid.Character.CharacterHandle;
using static StardewDruid.Data.DialogueData;
using static StardewDruid.Data.IconData;
using static StardewDruid.Journal.HerbalData;


namespace StardewDruid.Journal
{
    internal class Druid : IClickableMenu
    {

        public bool active;

        public bool reverse;

        public enum journalTypes
        {
            none,
            quests,
            effects,
            relics,
            herbalism,

        }

        public journalTypes type = journalTypes.quests;

        public const int region_forwardButton = 101;

        public const int region_backButton = 102;
 
        public List<List<string>> pages;

        public List<ClickableComponent> questLogButtons = new();

        public List<ClickableComponent> galleryButtons = new();

        private int currentPage;

        private int questPage = -1;
 
        public ClickableTextureComponent forwardButton;

        public ClickableTextureComponent endButton;

        public ClickableTextureComponent backButton;

        public ClickableTextureComponent startButton;

        public ClickableTextureComponent activeButton;

        public ClickableTextureComponent reverseButton;

        public ClickableTextureComponent questsButton;

        public ClickableTextureComponent effectsButton;

        public ClickableTextureComponent relicsButton;

        public ClickableTextureComponent herbalismButton;

        public ClickableTextureComponent questingButton;

        //protected Page _shownPage;

        protected float _contentHeight;
 
        protected float _scissorRectHeight;
 
        public float scrollAmount;
  
        public ClickableTextureComponent upArrow;

        public ClickableTextureComponent downArrow;

        public ClickableTextureComponent scrollBar;
 
        private bool scrolling;

        public Rectangle scrollBarBounds;

        private string hoverText = "";

        public int hoverDetail = -1;

        public int lastHover = -1;

        // dragon menu

        public bool dragonMenu;

        public DragonRender dragonRender;

        //public TextBox dragonName;

        public ColorPicker dragonPrimary;

        public ColorPicker dragonSecondary;

        public ColorPicker dragonTertiary;

        public int dragonDirection;

        public int dragonScheme;

        public int breathScheme;

        public ClickableTextureComponent dragonPortrait;

        public ClickableTextureComponent resetButton;

        public ClickableTextureComponent saveButton;

        public Dictionary<IconData.schemes,ClickableComponent> dragonSchemes = new();

        public Dictionary<IconData.schemes, ClickableComponent> breathSchemes = new();

        public Druid(journalTypes Type = journalTypes.quests)
          : base(0, 0, 0, 0, true)
        {

            type = Type;

            reverse = Mod.instance.Config.reverseJournal;

            active = Mod.instance.Config.activeJournal;

            Game1.playSound("bigSelect");
 
            width = 960;
            
            height = 640;

            Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(width, height, 0, 0);
           
            xPositionOnScreen = (int)centeringOnScreen.X;
            
            yPositionOnScreen = (int)centeringOnScreen.Y + 32;

            setupPages();

            questLogButtons = new List<ClickableComponent>();
            
            for (int index = 0; index < 6; ++index)
            {
                
                questLogButtons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 16, yPositionOnScreen + 16 + index * ((height - 32) / 6), width - 32, (height - 32) / 6 + 4), index.ToString() ?? "")
                {
                    myID = index,
                    downNeighborID = -7777,
                    upNeighborID = index > 0 ? index - 1 : -1,
                    rightNeighborID = -7777,
                    leftNeighborID = -7777,
                    fullyImmutable = true
                });

            }

            galleryButtons = new List<ClickableComponent>();

            for (int index = 0; index < 18; ++index)
            {

                galleryButtons.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + 16 + (index % 6) * ((width - 32) / 6), yPositionOnScreen + 16 + 48 + (int)(index / 6) * ((height - 32) / 3), (width - 32) / 6, (height - 32) / 3 + 4 - 48), index.ToString() ?? "")
                {
                    myID = 50 + index,
                    downNeighborID = index < 12 ? 50 + index + 5 : -7777,
                    upNeighborID = index > 5 ? 50 + index - 5 : -1,
                    rightNeighborID = index < 17 ? 50 + index + 1 : -7777,
                    leftNeighborID = index > 0 ? 50 + index - 1 : -1,
                    fullyImmutable = true
                });

            }

            

            ClickableTextureComponent closeCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width, yPositionOnScreen +8, 56, 56), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), 4f, false);
            closeCTC.myID = 102;
            closeCTC.rightNeighborID = 999;
            upperRightCloseButton = closeCTC;

            ClickableTextureComponent backCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen - 72, yPositionOnScreen + 8, 56, 56), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), 3f, false);
            backCTC.myID = 102;
            backCTC.rightNeighborID = -7777;
            backButton = backCTC;

            ClickableTextureComponent startCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen - 72, yPositionOnScreen + 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            startCTC.myID = 103;
            startCTC.hoverText = DialogueData.Strings(stringkeys.startPage);
            startButton = startCTC;

            ClickableTextureComponent forwardCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 8, yPositionOnScreen + height - 84, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            forwardCTC.myID = 101;
            forwardButton = forwardCTC;

            ClickableTextureComponent endCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 8, yPositionOnScreen + height - 148, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            endCTC.myID = 104;
            endCTC.hoverText = DialogueData.Strings(stringkeys.endPage);
            endButton = endCTC;
            
            ClickableTextureComponent activeCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 24 - 56 - 56, yPositionOnScreen - 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            activeCTC.myID = 105;
            activeCTC.hoverText = DialogueData.Strings(stringkeys.sortCompletion);
            activeButton = activeCTC;
            
            ClickableTextureComponent reverseCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 24 - 56, yPositionOnScreen - 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            reverseCTC.myID = 106;
            reverseCTC.hoverText = DialogueData.Strings(stringkeys.reverseOrder);
            reverseButton = reverseCTC;

            ClickableTextureComponent questsCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 8, yPositionOnScreen - 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            questsCTC.myID = 107;
            questsCTC.hoverText = DialogueData.Strings(stringkeys.openQuests);
            questsButton = questsCTC;

            ClickableTextureComponent effectsCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 8 + 56, yPositionOnScreen - 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            effectsCTC.myID = 108;
            effectsCTC.hoverText = DialogueData.Strings(stringkeys.openEffects);
            effectsButton = effectsCTC;

            ClickableTextureComponent relicsCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 8 + 56 + 56, yPositionOnScreen - 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            relicsCTC.myID = 109;
            relicsCTC.hoverText = DialogueData.Strings(stringkeys.openRelics);
            relicsButton = relicsCTC;

            ClickableTextureComponent herbalismCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + 8 + 56 + 56 + 56, yPositionOnScreen - 72, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            herbalismCTC.myID = 110;

            ClickableTextureComponent questingCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 24 - 56 - 56, yPositionOnScreen + 16, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            questingCTC.myID = 111;
            questingButton = questingCTC;



            if (Mod.instance.questHandle.IsComplete(QuestHandle.herbalism))
            {

                herbalismCTC.hoverText = DialogueData.Strings(stringkeys.openPotions);

            }
            else
            {

                herbalismCTC.hoverText = DialogueData.Strings(stringkeys.checkHerbalism);

            }

            herbalismButton = herbalismCTC;

            int num = xPositionOnScreen + width + 16;
            
            upArrow = new ClickableTextureComponent(new Rectangle(num, yPositionOnScreen + 96, 44, 48), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), 4f, false);
            
            downArrow = new ClickableTextureComponent(new Rectangle(num, yPositionOnScreen + height - 64, 44, 48), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), 4f, false);
            
            scrollBarBounds = new Rectangle();
            
            scrollBarBounds.X = upArrow.bounds.X + 12;
           
            scrollBarBounds.Width = 24;
            
            scrollBarBounds.Y = upArrow.bounds.Y + upArrow.bounds.Height + 4;
            
            scrollBarBounds.Height = downArrow.bounds.Y - 4 - scrollBarBounds.Y;
            
            scrollBar = new ClickableTextureComponent(new Rectangle(scrollBarBounds.X, scrollBarBounds.Y, 24, 40), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), 4f, false);
            
            if (!Game1.options.SnappyMenus)
                return;
            
            populateClickableComponentList();
            
            base.snapToDefaultClickableComponent();
            
        }

        public static journalTypes JournalButtonPressed()
        {

            if (Mod.instance.Config.journalButtons.GetState() == SButtonState.Pressed)
            {

                return journalTypes.quests;

            }
            else
            if (Mod.instance.Config.effectsButtons.GetState() == SButtonState.Pressed)
            {

                return journalTypes.effects;

            }
            else
            if (Mod.instance.Config.relicsButtons.GetState() == SButtonState.Pressed)
            {

                return journalTypes.relics;

            }
            else
            if (Mod.instance.Config.herbalismButtons.GetState() == SButtonState.Pressed && Mod.instance.questHandle.IsComplete(QuestHandle.herbalism))
            {

                return journalTypes.herbalism;

            }

            return journalTypes.none;

        }

        public void setupPages()
        {

            if (type == journalTypes.quests)
            {

                pages = Mod.instance.questHandle.OrganiseQuests(active, reverse);

            }
            else if (type == journalTypes.relics)
            {

                pages = Mod.instance.relicsData.OrganiseRelics();

                if (Mod.instance.save.reliquary.ContainsKey(IconData.relics.dragon_form.ToString()))
                {

                    if(dragonRender == null)
                    {

                        setupDragonMenu();

                    }

                }

            }
            else if (type == journalTypes.herbalism)
            {

                pages = Mod.instance.herbalData.OrganiseHerbals();

            }
            else
            {

                pages = Mod.instance.questHandle.OrganiseEffects(reverse);

            }

        }

        protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
        {
            if (oldID >= 0 && oldID < 6 && questPage == -1)
            {
                switch (direction)
                {
                    case 1:
                        if (currentPage < pages.Count - 1)
                        {
                            currentlySnappedComponent = getComponentWithID(101);
                            currentlySnappedComponent.leftNeighborID = oldID;
                            break;
                        }
                        break;
                    case 2:
                        if (oldID < 5 && pages[currentPage].Count - 1 > oldID)
                        {
                            currentlySnappedComponent = getComponentWithID(oldID + 1);
                            break;
                        }
                        break;
                    case 3:
                        if (currentPage > 0)
                        {
                            currentlySnappedComponent = getComponentWithID(102);
                            currentlySnappedComponent.rightNeighborID = oldID;
                            break;
                        }
                        break;
                }
            }
            else if (oldID == 102)
            {
                if (questPage != -1)
                    return;
                currentlySnappedComponent = getComponentWithID(0);
            }
            snapCursorToCurrentSnappedComponent();
        }

        public void setupDragonMenu()
        {


            dragonRender = new DragonRender();

            dragonRender.LoadConfigScheme();

            dragonSchemes = new();

            dragonScheme = Mod.instance.Config.dragonScheme;

            breathScheme = Mod.instance.Config.dragonBreath;

            int colourArrayX = xPositionOnScreen + (width / 2) - 352;

            int colourArrayY = yPositionOnScreen + 372 + 40;

            int colourCount = Mod.instance.iconData.DragonOptions.Count;

            for (int s = 0; s < colourCount; s++)
            {

                KeyValuePair<IconData.schemes, string> scheme = Mod.instance.iconData.DragonOptions.ElementAt(s);

                dragonSchemes.Add(scheme.Key, new ClickableComponent(new Rectangle(colourArrayX + (s * 96), colourArrayY, 64, 64), scheme.Value)
                {
                    myID = 700 + s,
                    downNeighborID = -7777,
                    upNeighborID = -7777,
                    rightNeighborID = s < colourCount - 1 ? 700 + s + 1 : -1,
                    leftNeighborID = s > 0 ? 700 + s - 1 : -1,
                    fullyImmutable = true
                });


            }

            colourArrayY = yPositionOnScreen + 480 + 40;

            colourCount = Mod.instance.iconData.BreathOptions.Count;

            for (int s = 0; s < colourCount; s++)
            {

                KeyValuePair<IconData.schemes, string> scheme = Mod.instance.iconData.BreathOptions.ElementAt(s);

                breathSchemes.Add(scheme.Key, new ClickableComponent(new Rectangle(colourArrayX + (s * 96), colourArrayY, 64, 64), scheme.Value)
                {
                    myID = 800 + s,
                    downNeighborID = -7777,
                    upNeighborID = -7777,
                    rightNeighborID = s < colourCount - 1 ? 800 + s + 1 : -1,
                    leftNeighborID = s > 0 ? 800 + s - 1 : -1,
                    fullyImmutable = true
                });

            }

            dragonPrimary = new(DialogueData.Strings(stringkeys.primaryColour), xPositionOnScreen + (width / 2) + 32, yPositionOnScreen + 96);

            dragonPrimary.setColor(Mod.instance.iconData.gradientColours[IconData.schemes.dragon_custom][0]);

            dragonSecondary = new(DialogueData.Strings(stringkeys.secondaryColour), xPositionOnScreen + (width / 2) + 32, yPositionOnScreen + 200);

            dragonSecondary.setColor(Mod.instance.iconData.gradientColours[IconData.schemes.dragon_custom][1]);

            dragonTertiary = new(DialogueData.Strings(stringkeys.tertiaryColour), xPositionOnScreen + (width / 2) + 32, yPositionOnScreen + 304);

            dragonTertiary.setColor(Mod.instance.iconData.gradientColours[IconData.schemes.dragon_custom][2]);

            dragonDirection = 1;

            ClickableTextureComponent dragonPortraitCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + (width / 2) - 352, yPositionOnScreen + 48, 320, 320), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            dragonPortraitCTC.myID = 601;
            dragonPortrait = dragonPortraitCTC;

            ClickableTextureComponent resetButtonCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 160, yPositionOnScreen + height - 240, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            resetButtonCTC.myID = 602;
            resetButtonCTC.hoverText = DialogueData.Strings(stringkeys.dragonReset);
            resetButton = resetButtonCTC;

            ClickableTextureComponent saveButtonCTC = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + width - 160, yPositionOnScreen + height - 160, 56, 56), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), 3f, false);
            saveButtonCTC.myID = 603;
            saveButtonCTC.hoverText = DialogueData.Strings(stringkeys.dragonSave);
            saveButton = saveButtonCTC;

        }

        public override void snapToDefaultClickableComponent()
        {
 
            currentlySnappedComponent = getComponentWithID(0);
 
            snapCursorToCurrentSnappedComponent();

        }

        public override void receiveGamePadButton(Buttons b)
        {
 
            if (b == Buttons.RightTrigger && questPage == -1 && currentPage < pages.Count - 1)
            {
 
                nonQuestPageForwardButton();
 
            }
            else if (b == Buttons.LeftTrigger && questPage == -1)
            {

                if (currentPage > 0)
                {
                    nonQuestPageBackButton();

                }
                else
                {
                    pageEndButton();
                }

            }

        }

        public bool NeedsScroll()
        {
 
            return questPage != -1 && _contentHeight > (double)_scissorRectHeight;

        }

        public override void receiveScrollWheelAction(int direction)
        {
  
            if (NeedsScroll())
            {
  
                float num = scrollAmount - Math.Sign(direction) * 64 / 2;
 
                if ((double)num < 0.0)
                {
                    num = 0.0f;
                }
                    
  
                if ((double)num > _contentHeight - (double)_scissorRectHeight)
                {
                    num = _contentHeight - _scissorRectHeight;

                }

                if (scrollAmount != (double)num)
                {
                    scrollAmount = num;
                    Game1.playSound("shiny4");
                    SetScrollBarFromAmount();
                }
 
            }

            base.receiveScrollWheelAction(direction);
 
        }

        public override void performHoverAction(int x, int y)
        {
            
            hoverText = "";
            
            int j = x - 8;
            int k = y - 8;

            List<ClickableTextureComponent> buttons = new()
            {
                
                upperRightCloseButton, backButton, questsButton, effectsButton, relicsButton, herbalismButton,

            };

            if(type == journalTypes.quests)
            {
                
                buttons.Add(activeButton);
            
            }

            if (questPage == -1 && pages.Count > 0)
            {

                if (currentPage > 0)
                {

                    buttons.Add(startButton);

                }

                if(currentPage < pages.Count - 1)
                {

                    buttons.Add(endButton);

                    buttons.Add(forwardButton);

                }
   
            }
            else if (dragonMenu)
            {

                buttons.Add(resetButton);

                buttons.Add(saveButton);

            }

            if (type == journalTypes.quests || type == journalTypes.effects)
            {

                buttons.Add(reverseButton);

            }

            foreach (ClickableTextureComponent button in buttons)
            {
               
                button.tryHover(j, k, 0.2f);

                if (button.containsPoint(j,k))
                {
                    
                    hoverText = button.hoverText;

                    return;
                
                }

            }

            lastHover = hoverDetail;

            hoverDetail = -1;

            if (questPage == -1 && pages.Count > 0 && (type == journalTypes.herbalism || type == journalTypes.relics))
            {

                CheckGalleryHovers(x,y);

            }

            if(questPage != -1 && dragonMenu)
            {

                CheckDragonHovers(x, y);

            }

            if(questPage != -1 && type == journalTypes.quests)
            {

                CheckQuestHovers(x, y);

            }

            if(type == journalTypes.herbalism)
            {
                
                activeButton.tryHover(x, y, 0.1f);

                if (activeButton.containsPoint(x, y))
                {

                    hoverText = DialogueData.Strings(stringkeys.massHerbalism);
                
                }

            }

            if (!NeedsScroll())
            {
                return;

            }
                
            upArrow.tryHover(x, y, 0.1f);

            downArrow.tryHover(x, y, 0.1f);

            scrollBar.tryHover(x, y, 0.1f);

            int num = scrolling ? 1 : 0;

        }

        public void CheckGalleryHovers(int x, int y)
        {

            int hoverFound = -1;

            for (int index = 0; index < galleryButtons.Count; ++index)
            {

                if (pages[currentPage].Count <= index)
                {

                    return;

                }

                if (galleryButtons[index].containsPoint(x, y))
                {

                    hoverFound = index;

                    break;

                }

            }

            if(hoverFound == -1)
            {

                return;

            }

            if (pages[currentPage][hoverFound] == "blank")
            {

                return;

            }

            hoverDetail = hoverFound;

            if (type == journalTypes.herbalism)
            {

                if (lastHover != hoverDetail)
                {

                    if (pages[currentPage][hoverFound] != "configure")
                    {

                        Mod.instance.herbalData.CheckHerbal(pages[currentPage][hoverFound]);

                    }

                }

            }

        }

        public void CheckDragonHovers(int x, int y)
        {

            foreach(KeyValuePair<IconData.schemes,ClickableComponent> dragonComponent in dragonSchemes)
            {

                if (dragonComponent.Value.containsPoint(x, y))
                {

                    hoverText = dragonComponent.Value.name;

                    return;

                }

            }

            foreach (KeyValuePair<IconData.schemes, ClickableComponent> breathComponent in breathSchemes)
            {

                if (breathComponent.Value.containsPoint(x, y))
                {

                    hoverText = breathComponent.Value.name;

                    return;

                }

            }

            if (dragonPortrait.containsPoint(x, y))
            {

                hoverText = DialogueData.Strings(stringkeys.dragonRotate);

            }
            else if (dragonPrimary.containsPoint(x, y))
            {

                hoverText = DialogueData.Strings(stringkeys.dragonScale);

            }
            else if (dragonSecondary.containsPoint(x, y))
            {

                hoverText = DialogueData.Strings(stringkeys.dragonAccent);

            }
            else if (dragonTertiary.containsPoint(x, y))
            {

                hoverText = DialogueData.Strings(stringkeys.dragonEye);

            }

        }

        public void CheckQuestHovers(int x, int y)
        {

            if (!Context.IsMainPlayer)
            {

                return;

            }

            questingButton.tryHover(x, y, 0.1f);

            if (!questingButton.containsPoint(x, y))
            {

                return;

            }

            string questId = pages[currentPage][questPage];


            if (Mod.instance.save.progress.ContainsKey(questId))
            {

                if (Mod.instance.save.progress[questId].status == 1)
                {

                    hoverText = DialogueData.Strings(stringkeys.skipQuest);

                }

                if (Mod.instance.save.progress[questId].status == 2)
                {

                    hoverText = DialogueData.Strings(stringkeys.replayQuest);

                }

                if (Mod.instance.save.progress[questId].status == 3)
                {

                    hoverText = DialogueData.Strings(stringkeys.replayTomorrow);

                }

                if (Mod.instance.save.progress[questId].status == 4)
                {

                    hoverText = DialogueData.Strings(stringkeys.cancelReplay);

                }

            }

        }

        public override void receiveKeyPress(Keys key)
        {
            
            if (Game1.isAnyGamePadButtonBeingPressed() && questPage != -1 && Game1.options.doesInputListContain(Game1.options.menuButton, key))
            {
                
                exitQuestPage();
            
            }
            else
            {

                base.receiveKeyPress(key);
            }

            if (Game1.options.doesInputListContain(Game1.options.journalButton, key) && readyToClose())
            {
                
                Game1.exitActiveMenu();

                Game1.playSound("bigDeSelect");

            }

            if (Mod.instance.Config.riteButtons.GetState() != SButtonState.Pressed)
            {
                
                return;

            }

            Game1.exitActiveMenu();

        }

        private void nonQuestPageForwardButton()
        {
            ++currentPage;
            Game1.playSound("shwip");
            if (!Game1.options.SnappyMenus || currentPage != pages.Count - 1)
                return;
            currentlySnappedComponent = getComponentWithID(0);
            snapCursorToCurrentSnappedComponent();
        }

        private void nonQuestPageBackButton()
        {
            --currentPage;
            Game1.playSound("shwip");
            if (!Game1.options.SnappyMenus || currentPage != 0)
                return;
            currentlySnappedComponent = getComponentWithID(0);
            snapCursorToCurrentSnappedComponent();
        }

        private void pageEndButton()
        {
            currentPage = pages.Count - 1;
            Game1.playSound("shwip");
            if (!Game1.options.SnappyMenus || currentPage != 0)
                return;
            currentlySnappedComponent = getComponentWithID(0);
            snapCursorToCurrentSnappedComponent();
        }

        private void pageStartButton()
        {
            currentPage = 0;
            Game1.playSound("shwip");
            if (!Game1.options.SnappyMenus || currentPage != 0)
                return;
            currentlySnappedComponent = getComponentWithID(0);
            snapCursorToCurrentSnappedComponent();
        }

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);

            if (questPage != -1 && dragonMenu)
            {
                if (dragonPrimary.containsPoint(x, y))
                {

                    dragonPrimary.clickHeld(x, y);

                }
                else if (dragonSecondary.containsPoint(x, y))
                {

                    dragonSecondary.clickHeld(x, y);

                }
                else if (dragonTertiary.containsPoint(x, y))
                {

                    dragonTertiary.clickHeld(x, y);

                }

            }
            if (scrolling)
                SetScrollFromY(y);
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);

            if (questPage != -1 && dragonMenu)
            {

                if (dragonPrimary.containsPoint(x, y))
                {

                    dragonPrimary.releaseClick();

                    DragonUpdateColour();

                }
                else if (dragonSecondary.containsPoint(x, y))
                {

                    dragonSecondary.releaseClick();

                    DragonUpdateColour();

                }
                else if (dragonTertiary.containsPoint(x, y))
                {

                    dragonTertiary.releaseClick();

                    DragonUpdateColour();

                }

            }

            scrolling = false;

        }

        public void SetScrollFromY(int y)
        {
            int y1 = scrollBar.bounds.Y;
            scrollAmount = Utility.Clamp((y - scrollBarBounds.Y) / (float)(scrollBarBounds.Height - scrollBar.bounds.Height), 0.0f, 1f) * (_contentHeight - _scissorRectHeight);
            SetScrollBarFromAmount();
            if (y1 == scrollBar.bounds.Y)
                return;
            Game1.playSound("shiny4");
        }

        public void UpArrowPressed()
        {
            upArrow.scale = upArrow.baseScale;
            scrollAmount -= 64f;
            if (scrollAmount < 0.0)
                scrollAmount = 0.0f;
            SetScrollBarFromAmount();
        }

        public void DownArrowPressed()
        {
            downArrow.scale = downArrow.baseScale;
            scrollAmount += 64f;
            if (scrollAmount > _contentHeight - (double)_scissorRectHeight)
                scrollAmount = _contentHeight - _scissorRectHeight;
            SetScrollBarFromAmount();
        }

        private void SetScrollBarFromAmount()
        {
            if (!NeedsScroll())
            {
                scrollAmount = 0.0f;
            }
            else
            {
                if (scrollAmount < 8.0)
                    scrollAmount = 0.0f;
                if (scrollAmount > _contentHeight - (double)_scissorRectHeight - 8.0)
                    scrollAmount = _contentHeight - _scissorRectHeight;
                scrollBar.bounds.Y = (int)(scrollBarBounds.Y + (scrollBarBounds.Height - scrollBar.bounds.Height) / (double)Math.Max(1f, _contentHeight - _scissorRectHeight) * scrollAmount);
            }
        }

        public override void applyMovementKey(int direction)
        {
            base.applyMovementKey(direction);
            if (!NeedsScroll())
                return;
            switch (direction)
            {
                case 0:
                    UpArrowPressed();
                    break;
                case 2:
                    DownArrowPressed();
                    break;
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            
            base.receiveLeftClick(x, y, playSound);

            if (Game1.activeClickableMenu == null)
            {
                
                return;
            
            }

            if (questsButton.containsPoint(x, y))
            {

                switchTo(journalTypes.quests);

                return;

            }
            else if (effectsButton.containsPoint(x, y))
            {

                switchTo(journalTypes.effects);

                return;

            }
            else if (relicsButton.containsPoint(x, y))
            {

                switchTo(journalTypes.relics);

                return;

            }
            else if (herbalismButton.containsPoint(x, y))
            {

                if (Mod.instance.questHandle.IsComplete(QuestHandle.herbalism))
                {

                    switchTo(journalTypes.herbalism);

                }

                return;


            }

            if (questPage == -1)
            {

                if (type == journalTypes.herbalism || type == journalTypes.relics)
                {

                    if (type == journalTypes.herbalism)
                    {
                        
                        if (activeButton.containsPoint(x, y))
                        {

                            Mod.instance.herbalData.MassBrew();

                            return;

                        }
                    
                    }

                    for (int index = 0; index < galleryButtons.Count; ++index)
                    {

                        if (pages.Count > 0 && pages[currentPage].Count > index && galleryButtons[index].containsPoint(x, y))
                        {

                            if (pages[currentPage][index] == "blank")
                            {

                                return;

                            }

                            Game1.playSound("smallSelect");

                            if (type == journalTypes.herbalism)
                            {

                                if (pages[currentPage][index] == "configure")
                                {

                                    Mod.instance.herbalData.PotionBehaviour(index);

                                    return;

                                }

                                if (Mod.instance.herbalData.herbalism[pages[currentPage][index]].status == 1)
                                {

                                    int amount = 1;

                                    if(Mod.instance.Helper.Input.GetState(SButton.LeftShift) == SButtonState.Held)
                                    {
                                        amount = 10;
                                    }

                                    Mod.instance.herbalData.BrewHerbal(pages[currentPage][index],amount);

                                }

                            }

                            if (type == journalTypes.relics)
                            {
                                
                                string relicId = pages[currentPage][index];

                                if (!Mod.instance.save.reliquary.ContainsKey(relicId))
                                {
                                    
                                    return;
                                
                                }

                                if (!Mod.instance.relicsData.reliquary[relicId].function)
                                {

                                    return;

                                }

                                int function = Mod.instance.relicsData.RelicFunction(pages[currentPage][index]);

                                switch (function)
                                {

                                    case 1:

                                        exitThisMenu();

                                        return;

                                    case 2:

                                        questPage = index;

                                        dragonMenu = (pages[currentPage][questPage] == IconData.relics.dragon_form.ToString());

                                        openDetailPage();

                                        return;

                                    default:

                                        Game1.playSound("ghost");

                                        return;

                                }

                            }

                            return;

                        }

                    }

                }
                else
                {

                    for (int index = 0; index < questLogButtons.Count; ++index)
                    {

                        if (pages.Count > 0 && pages[currentPage].Count > index && questLogButtons[index].containsPoint(x, y))
                        {

                            questPage = index;

                            openDetailPage();

                            return;

                        }

                    }

                }

                if (currentPage == 0 && backButton.containsPoint(x, y))
                {
                    
                    exitThisMenu(true);
                
                }
                else if (currentPage < pages.Count - 1 && forwardButton.containsPoint(x, y))
                {
                    
                    nonQuestPageForwardButton();
                
                }
                else if (currentPage > 0 && backButton.containsPoint(x, y))
                {
                    
                    nonQuestPageBackButton();
                
                }
                else if (currentPage > 0 && startButton.containsPoint(x, y))
                {
                    
                    pageStartButton();
                
                }
                else if (currentPage < pages.Count - 1 && endButton.containsPoint(x, y))
                {
                    
                    pageEndButton();
                
                }
                else if (reverseButton.containsPoint(x, y))
                {
                    
                    reverse = reverse ? false : true; 
                    
                    setupPages();

                }
                else if (activeButton.containsPoint(x, y))
                { 
                    
                    active = active ? false : true; 
                    
                    setupPages(); 
                
                }
                else
                {

                    exitThisMenu(true);

                }
                    
            }
            else
            {

                if (backButton.containsPoint(x, y))
                {

                    exitQuestPage();

                    return;

                }

                if (dragonMenu)
                {

                    foreach (KeyValuePair<IconData.schemes, ClickableComponent> dragonComponent in dragonSchemes)
                    {

                        if (dragonComponent.Value.containsPoint(x, y))
                        {

                            dragonScheme = (int)dragonComponent.Key;
                            
                            Mod.instance.Config.dragonScheme = dragonScheme;

                            dragonRender.LoadConfigScheme();

                            Game1.playSound("shwip");
                            
                            return;
                        
                        }

                    }

                    foreach (KeyValuePair<IconData.schemes, ClickableComponent> breathComponent in breathSchemes)
                    {

                        if (breathComponent.Value.containsPoint(x, y))
                        {

                            breathScheme = (int)breathComponent.Key;

                            Mod.instance.Config.dragonBreath = breathScheme;

                            dragonRender.LoadConfigScheme();

                            Game1.playSound("shwip");

                            return;

                        }

                    }

                    if (dragonPortrait.containsPoint(x, y))
                    {

                        dragonDirection = (dragonDirection + 1) % 4;

                    }
                    else if (dragonPrimary.containsPoint(x, y))
                    {

                        dragonPrimary.click(x, y);

                        DragonUpdateColour();

                    }
                    else if (dragonSecondary.containsPoint(x, y))
                    {

                        dragonSecondary.click(x, y);

                        DragonUpdateColour();

                    }
                    else if (dragonTertiary.containsPoint(x, y))
                    {

                        dragonTertiary.click(x, y);

                        DragonUpdateColour();

                    }
                    else if (resetButton.containsPoint(x, y))
                    {

                        IconData.schemes newScheme = (IconData.schemes)dragonScheme;

                        if (newScheme != schemes.dragon_custom)
                        {

                            dragonPrimary.setColor(Mod.instance.iconData.gradientColours[newScheme][0]);

                            dragonSecondary.setColor(Mod.instance.iconData.gradientColours[newScheme][1]);

                            dragonTertiary.setColor(Mod.instance.iconData.gradientColours[newScheme][2]);

                            DragonUpdateColour();

                        }

                    }
                    else if (saveButton.containsPoint(x, y))
                    {

                        Mod.instance.SaveConfig();

                        exitThisMenu();

                    }

                    return;

                }

                if(type == journalTypes.quests)
                {

                    if(receiveClickQuesting(x, y)) { return; };


                }

                if (!NeedsScroll())
                {
                    
                    exitQuestPage();

                    return;

                }

                if (downArrow.containsPoint(x, y) && scrollAmount < _contentHeight - (double)_scissorRectHeight)
                {
                    
                    DownArrowPressed();

                    Game1.playSound("shwip");

                }
                else if (upArrow.containsPoint(x, y) && scrollAmount > 0.0)
                {
                    
                    UpArrowPressed();

                    Game1.playSound("shwip");

                }
                else if (scrollBar.containsPoint(x, y))
                {
                    
                    scrolling = true;

                } 
                else if (scrollBarBounds.Contains(x, y))
                {

                    scrolling = true;

                }
                else if (!downArrow.containsPoint(x, y) && x > xPositionOnScreen + width && x < xPositionOnScreen + width + 128 && y > yPositionOnScreen && y < yPositionOnScreen + height)
                {
                    
                    scrolling = true;

                    base.leftClickHeld(x, y);

                    base.releaseLeftClick(x, y);
                
                }

            }
        
        }

        public bool receiveClickQuesting(int x, int y)
        {

            string questId = pages[currentPage][questPage];

            if (!Context.IsMainPlayer)
            {

                return false;

            }

            if (!questingButton.containsPoint(x, y))
            {

                return false;

            }

            if (!Mod.instance.save.progress.ContainsKey(questId))
            {

                return false;

            }

            if (!Mod.instance.questHandle.IsReplayable(questId) && Mod.instance.save.progress[questId].status > 1)
            {
                
                return false;
            
            }

            switch (Mod.instance.save.progress[questId].status)
            {

                default:
                case 1:


                    Game1.playSound("ghost");

                    Mod.instance.questHandle.CompleteQuest(questId);

                    Mod.instance.questHandle.OnCancel(questId);

                    return true;

                case 2:

                    if (Mod.instance.questHandle.IsReplayable(questId))
                    {

                        Game1.playSound("yoba");

                        Mod.instance.questHandle.RevisitQuest(questId);

                    }

                    return true;

                case 3:

                    if (Mod.instance.questHandle.IsReplayable(questId))
                    {

                        //Game1.playSound("ghost");

                    }

                    return true;

                case 4:

                    Game1.playSound("ghost");

                    Mod.instance.questHandle.OnCancel(questId);

                    Mod.instance.SyncMultiplayer();

                    return true;

            }

        }

        public void DragonUpdateColour()
        {

            Microsoft.Xna.Framework.Color primary = dragonPrimary.getSelectedColor();

            Mod.instance.Config.dragonPrimaryR = primary.R;
            Mod.instance.Config.dragonPrimaryG = primary.G;
            Mod.instance.Config.dragonPrimaryB = primary.B;

            Microsoft.Xna.Framework.Color secondary = dragonSecondary.getSelectedColor();

            Mod.instance.Config.dragonSecondaryR = secondary.R;
            Mod.instance.Config.dragonSecondaryG = secondary.G;
            Mod.instance.Config.dragonSecondaryB = secondary.B;

            Microsoft.Xna.Framework.Color tertiary = dragonTertiary.getSelectedColor();

            Mod.instance.Config.dragonTertiaryR = tertiary.R;
            Mod.instance.Config.dragonTertiaryG = tertiary.G;
            Mod.instance.Config.dragonTertiaryB = tertiary.B;

            Mod.instance.iconData.CustomDragonScheme();

            if(dragonScheme == (int)IconData.schemes.dragon_custom)
            {

                dragonRender.LoadConfigScheme();

            }

        }

        public void openDetailPage()
        {

            Game1.playSound("smallSelect");

            scrollAmount = 0.0f;

            SetScrollBarFromAmount();

            if (!Game1.options.SnappyMenus)
            {

                return;

            }

            currentlySnappedComponent = getComponentWithID(102);

            currentlySnappedComponent.rightNeighborID = -7777;

            currentlySnappedComponent.downNeighborID = 104;

            snapCursorToCurrentSnappedComponent();

        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {

            if (Game1.activeClickableMenu == null)
            {

                return;

            }

            if (questPage == -1)
            {

                if (type == journalTypes.herbalism)
                {
                    
                    if(Game1.player.health == Game1.player.maxHealth & Game1.player.Stamina == Game1.player.MaxStamina)
                    {

                        return;

                    }

                    for (int index = 0; index < galleryButtons.Count; ++index)
                    {

                        if (pages.Count > 0 && pages[currentPage].Count > index && galleryButtons[index].containsPoint(x, y))
                        {

                            if(!Mod.instance.herbalData.herbalism.ContainsKey(pages[currentPage][index]))
                            {

                                return;

                            }

                            Herbal herbal = Mod.instance.herbalData.herbalism[pages[currentPage][index]];

                            if (herbal.resource)
                            {

                                return;

                            }

                            if (Mod.instance.save.herbalism.ContainsKey(herbal.herbal))
                            {

                                if (Mod.instance.save.herbalism[herbal.herbal] == 0)
                                {
                                    
                                    return;

                                }

                            }
                            else
                            {

                                return;

                            }

                            Game1.playSound("smallSelect");

                            Mod.instance.herbalData.ConsumeHerbal(pages[currentPage][index]);

                            return;

                        }

                    }

                }

                if (type == journalTypes.relics)
                {
                    
                    for (int index = 0; index < galleryButtons.Count; ++index)
                    {

                        if (pages.Count > 0 && pages[currentPage].Count > index && galleryButtons[index].containsPoint(x, y))
                        {
                            
                            string relicId = pages[currentPage][index];

                            if (!Mod.instance.save.reliquary.ContainsKey(relicId))
                            {

                                return;

                            }

                            if (!Mod.instance.relicsData.reliquary[relicId].cancel)
                            {

                                return;

                            }

                            int function = Mod.instance.relicsData.RelicCancel(pages[currentPage][index]);

                            switch (function)
                            {

                                case 1:

                                    exitThisMenu();

                                    return;

                                default:

                                    Game1.playSound("ghost");

                                    return;

                            }

                        }

                    }

                }

            }

        }

        public void switchTo(journalTypes journalType)
        {

            if(questPage != -1)
            {

                Game1.playSound("shwip");

                if (Game1.options.SnappyMenus)
                {
                    base.snapToDefaultClickableComponent();
                }

            }

            type = journalType;

            questPage = -1;

            dragonMenu = false;

            currentPage = 0;

            setupPages();

        }

        public void exitQuestPage()
        {
            
            questPage = -1;

            setupPages();

            Game1.playSound("shwip");

            if (!Game1.options.SnappyMenus)
            {
                return;
            }
                
            base.snapToDefaultClickableComponent();

        }

        public override void draw(SpriteBatch b)
        {
            
            Texture2D iconTexture = Mod.instance.iconData.displayTexture;

            SpriteBatch spriteBatch1 = b;
            
            Texture2D fadeToBlackRect = Game1.fadeToBlackRect;
            
            Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
            
            Rectangle bounds = viewport.Bounds;
            
            Color color = Color.Black * 0.75f;
            
            spriteBatch1.Draw(fadeToBlackRect, bounds, color);

            string journalTitle = DialogueData.Strings(stringkeys.stardewDruid);

            switch (type)
            {
                case journalTypes.effects:

                    journalTitle = DialogueData.Strings(stringkeys.grimoire);

                    break;

                case journalTypes.relics:

                    journalTitle = DialogueData.Strings(stringkeys.reliquary);

                    if (dragonMenu)
                    {

                        journalTitle = DialogueData.Strings(stringkeys.dragonomicon);

                    }

                    break;

                case journalTypes.herbalism:

                    journalTitle = DialogueData.Strings(stringkeys.apothecary);

                    break;

            }

            SpriteText.drawStringWithScrollCenteredAt(b, journalTitle, xPositionOnScreen + width / 2, yPositionOnScreen - 64);

            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), xPositionOnScreen, yPositionOnScreen, width, height, Color.White, 4f, true, -1f);

            if (questPage == -1)
            {


                if (type == journalTypes.herbalism || type == journalTypes.relics)
                {

                    drawGallery(b);

                } 
                else
                {

                    drawList(b);

                }

                // forward / end
                if (currentPage < pages.Count - 1)
                {

                    b.Draw(iconTexture, new Vector2(forwardButton.bounds.X, forwardButton.bounds.Y) - ((forwardButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.forward), Color.White, 0f, Vector2.Zero, forwardButton.scale, 0, 999f);

                    b.Draw(iconTexture, new Vector2(endButton.bounds.X, endButton.bounds.Y) - ((endButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.end), Color.White, 0f, Vector2.Zero, endButton.scale, 0, 999f);

                }

            }
            else
            {

                drawDetail(b);

            }

            // start
            if (currentPage > 0)
            {

                b.Draw(iconTexture, new Vector2(startButton.bounds.X, startButton.bounds.Y) - ((startButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.end), Color.White, 0f, Vector2.Zero, startButton.scale, SpriteEffects.FlipHorizontally, 999f);

            }

            //back
            b.Draw(iconTexture, new Vector2(backButton.bounds.X, backButton.bounds.Y) - ((backButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.forward), Color.White, 0f, Vector2.Zero, backButton.scale, SpriteEffects.FlipHorizontally, 999f);

            // quests
            b.Draw(iconTexture, new Vector2(questsButton.bounds.X, questsButton.bounds.Y) - ((questsButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.quest), Color.White * (type == journalTypes.quests ? 1f : 0.65f), 0f, Vector2.Zero, questsButton.scale, 0, 999f);

            // effects
            b.Draw(iconTexture, new Vector2(effectsButton.bounds.X, effectsButton.bounds.Y) - ((effectsButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.effect), Color.White * (type == journalTypes.effects ? 1f : 0.65f), 0f, Vector2.Zero, effectsButton.scale, 0, 999f);

            // relics
            b.Draw(iconTexture, new Vector2(relicsButton.bounds.X, relicsButton.bounds.Y) - ((relicsButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.relic), Color.White * (type == journalTypes.relics ? 1f : 0.65f), 0f, Vector2.Zero, relicsButton.scale, 0, 999f);

            // herbalism
            b.Draw(iconTexture, new Vector2(herbalismButton.bounds.X, herbalismButton.bounds.Y) - ((herbalismButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.herbalism), Color.White * (type == journalTypes.herbalism ? 1f : 0.65f), 0f, Vector2.Zero, herbalismButton.scale, 0, 999f);

            // exit
            b.Draw(iconTexture, new Vector2(upperRightCloseButton.bounds.X, upperRightCloseButton.bounds.Y) - ((upperRightCloseButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.exit), Color.White, 0f, Vector2.Zero, upperRightCloseButton.scale, 0, 999f);

            if (type == journalTypes.herbalism)
            {

                drawStats(b);

            }

            if (NeedsScroll())
            {

                upArrow.draw(b);

                downArrow.draw(b);

                scrollBar.draw(b);

            }

            Game1.mouseCursorTransparency = 1f;

            drawMouse(b, false, -1);

            if (hoverText.Length > 0)
            {

                drawHoverText(b, hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, null, -1, -1, -1, 1f, null, null);

            }

            if(hoverDetail != -1)
            {

                drawHoverDetail(b);

            }

        }

        public void drawList(SpriteBatch b)
        {
            
            Texture2D iconTexture = Mod.instance.iconData.displayTexture;

            // =========================================================
            // List controls

            // reverse
            b.Draw(iconTexture, new Vector2(reverseButton.bounds.X, reverseButton.bounds.Y) - ((reverseButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.reverse), Color.White * (reverse ? 1f : 0.65f), 0f, Vector2.Zero, reverseButton.scale, 0, 999f);

            // active
            b.Draw(iconTexture, new Vector2(activeButton.bounds.X, activeButton.bounds.Y) - ((activeButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.active), Color.White * (active ? 1f : 0.65f), 0f, Vector2.Zero, activeButton.scale, 0, 999f);

            // =========================================================
            // List entries

            for (int index = 0; index < questLogButtons.Count; ++index)
            {

                if (pages.Count() > 0 && pages[currentPage].Count() > index)
                {

                    IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), questLogButtons[index].bounds.X, questLogButtons[index].bounds.Y, questLogButtons[index].bounds.Width, questLogButtons[index].bounds.Height, questLogButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) ? Color.Wheat : Color.White, 4f, false, -1f);

                    IconData.displays entryIcon;

                    string entryText;

                    if (type == journalTypes.effects)
                    {

                        string[] effectIds = pages[currentPage][index].Split("|");

                        entryIcon = Mod.instance.questHandle.effects[effectIds[0]][Convert.ToInt32(effectIds[1])].icon;

                        entryText = Mod.instance.questHandle.effects[effectIds[0]][Convert.ToInt32(effectIds[1])].title;

                    }
                    else
                    {

                        string questId = pages[currentPage][index];

                        IconData.displays questIcon;

                        switch (Mod.instance.save.progress[questId].status)
                        {
                            
                            default:
                            case 1:

                                questIcon = IconData.displays.active;

                                break;
                            case 2:
                            case 3:

                                questIcon = IconData.displays.complete;
 
                                break;
                            case 4:

                                questIcon = IconData.displays.replay;

                                break;

                        }

                        b.Draw(iconTexture, new Vector2(questLogButtons[index].bounds.Right - 80, questLogButtons[index].bounds.Y + 28), Mod.instance.iconData.DisplayRect(questIcon), Color.White * 1f, 0f, Vector2.Zero, 3f, 0, 999f);

                        entryIcon = Mod.instance.questHandle.quests[questId].icon;

                        entryText = Mod.instance.questHandle.quests[questId].title;

                    }

                    Utility.drawWithShadow(b, iconTexture, new Vector2(questLogButtons[index].bounds.X + 28, questLogButtons[index].bounds.Y + 28), Mod.instance.iconData.DisplayRect(entryIcon), Color.White, 0.0f, Vector2.Zero, 3f, false, 0.99f, -1, -1, 0.35f);

                    SpriteText.drawString(b, entryText, questLogButtons[index].bounds.X + 100, questLogButtons[index].bounds.Y + 30, 999999, -1, 999999, 1f, 0.88f, false, -1, "", null, 0);

                }

            }

        }

        public void drawGallery(SpriteBatch b)
        {

            if (type == journalTypes.herbalism)
            {

                Dictionary<HerbalData.herbals, List<string>> sections = Mod.instance.herbalData.titles;

                for (int index = 0; index < galleryButtons.Count; ++index)
                {

                    if (pages.Count() > 0 && pages[currentPage].Count() > index)
                    {

                        if (pages[currentPage][index] == "blank") { continue; }

                        if (pages[currentPage][index] == "configure")
                        {

                            HerbalData.herbals potion = herbals.ligna;

                            switch (index)
                            {

                                case 11:

                                    potion = herbals.impes;
                                    break;

                                case 17:

                                    potion = herbals.celeri;
                                    break;

                            }

                            IconData.displays flag = displays.complete;

                            if (Mod.instance.save.potions.ContainsKey(potion))
                            {

                                switch (Mod.instance.save.potions[potion])
                                {

                                    case 0:

                                        flag = displays.exit;

                                        break;

                                    case 1:

                                        flag = displays.complete;

                                        break;

                                    case 2:

                                        flag = displays.flag;

                                        break;


                                }

                            }

                            b.Draw(Mod.instance.iconData.displayTexture, new Vector2(galleryButtons[index].bounds.Center.X - 32, galleryButtons[index].bounds.Center.Y - 32), Mod.instance.iconData.DisplayRect(flag), Color.White, 0f, Vector2.Zero, 4f, 0, 0.900f);

                            continue;

                        }

                        Herbal herbal = Mod.instance.herbalData.herbalism[pages[currentPage][index]];

                        if (index % 6 == 0)
                        {

                            b.DrawString(Game1.smallFont, sections[herbal.line][0], new Vector2(galleryButtons[index].bounds.Left + 10, galleryButtons[index].bounds.Top - 40), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1f);

                            b.DrawString(Game1.smallFont, sections[herbal.line][0], new Vector2(galleryButtons[index].bounds.Left + 10 - 1.5f, galleryButtons[index].bounds.Top - 40 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1.1f);

                            b.DrawString(Game1.smallFont, sections[herbal.line][1], new Vector2(galleryButtons[index].bounds.Right + 10, galleryButtons[index].bounds.Top - 34), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                            b.DrawString(Game1.smallFont, sections[herbal.line][1], new Vector2(galleryButtons[index].bounds.Right + 10, galleryButtons[index].bounds.Top - 34 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                        }

                        bool highlight = galleryButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY());

                        if (herbal.status != 1)
                        {

                            highlight = false;

                        }

                        IClickableMenu.drawTextureBox(
                            b,
                            Game1.mouseCursors,
                            new Rectangle(384, 396, 15, 15),
                            galleryButtons[index].bounds.X,
                            galleryButtons[index].bounds.Y,
                            galleryButtons[index].bounds.Width,
                            galleryButtons[index].bounds.Height,
                            highlight ? Color.Wheat : Color.White,
                            4f,
                            false,
                            -1f
                        );

                        int amount = 0;

                        if (Mod.instance.save.herbalism.ContainsKey(herbal.herbal))
                        {

                            amount = Mod.instance.save.herbalism[herbal.herbal];

                        }

                        string amountString = amount.ToString();

                        SpriteText.drawString(b, amountString, galleryButtons[index].bounds.Center.X - (9*amountString.Length), galleryButtons[index].bounds.Center.Y + 24, 999999, -1, 999999, 1f, 0.88f, false, -1, "", null, 0);

                        Microsoft.Xna.Framework.Color colour = Mod.instance.iconData.SchemeColour(herbal.scheme);

                        if (!highlight && amount <= 0)
                        {

                            colour = Color.LightGray;

                        }

                        b.Draw(Mod.instance.iconData.relicsTexture, new Vector2(galleryButtons[index].bounds.Center.X - 40f + 2f, galleryButtons[index].bounds.Center.Y - 60f + 4f), Mod.instance.iconData.RelicRectangles(herbal.container), Microsoft.Xna.Framework.Color.Black * 0.35f, 0f, Vector2.Zero, 4f, 0, 0.900f);

                        b.Draw(Mod.instance.iconData.relicsTexture, new Vector2(galleryButtons[index].bounds.Center.X - 40f, galleryButtons[index].bounds.Center.Y - 60f), Mod.instance.iconData.RelicRectangles(herbal.container), Color.White, 0f, Vector2.Zero, 4f, 0, 0.901f);

                        b.Draw(Mod.instance.iconData.relicsTexture, new Vector2(galleryButtons[index].bounds.Center.X - 40f, galleryButtons[index].bounds.Center.Y - 60f), Mod.instance.iconData.RelicRectangles(herbal.content), colour, 0f, Vector2.Zero, 4f, 0, 0.902f);
                    
                    }

                }

                b.Draw(
                    Mod.instance.iconData.displayTexture,
                    new Vector2(activeButton.bounds.X, activeButton.bounds.Y) - ((activeButton.scale - 4f) * new Vector2(16, 16)),
                    Mod.instance.iconData.DisplayRect(IconData.displays.knock),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    activeButton.scale,
                    0,
                    -1f
                );

            }
            else
            {

                Dictionary<RelicData.relicsets, List<string>> sections = Mod.instance.relicsData.titles;

                for (int index = 0; index < galleryButtons.Count; ++index)
                {

                    if (pages.Count() > 0 && pages[currentPage].Count() > index)
                    {

                        if (pages[currentPage][index] == "blank")
                        {
                            continue;
                        }

                        string relicId = pages[currentPage][index];

                        Relic relic = Mod.instance.relicsData.reliquary[relicId];

                        if (index % 6 == 0)
                        {

                            b.DrawString(Game1.smallFont, sections[relic.line][0], new Vector2(galleryButtons[index].bounds.Left + 10, galleryButtons[index].bounds.Top - 40), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1f);

                            b.DrawString(Game1.smallFont, sections[relic.line][0], new Vector2(galleryButtons[index].bounds.Left + 10 - 1.5f, galleryButtons[index].bounds.Top - 40 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1.1f);

                            b.DrawString(Game1.smallFont, sections[relic.line][1], new Vector2(galleryButtons[index+1].bounds.Right + 10, galleryButtons[index].bounds.Top - 34), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                            b.DrawString(Game1.smallFont, sections[relic.line][1], new Vector2(galleryButtons[index+1].bounds.Right + 10, galleryButtons[index].bounds.Top - 34 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                        }

                        if (Mod.instance.save.reliquary.ContainsKey(relicId))
                        {
                            
                            bool highlight = galleryButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY());

                            if (!relic.function)
                            {

                                highlight = false;

                            }

                            IClickableMenu.drawTextureBox(
                                b,
                                Game1.mouseCursors,
                                new Rectangle(384, 396, 15, 15),
                                galleryButtons[index].bounds.X,
                                galleryButtons[index].bounds.Y,
                                galleryButtons[index].bounds.Width,
                                galleryButtons[index].bounds.Height,
                                highlight ? Color.Wheat : Color.White,
                                4f,
                                false,
                                -1f
                            );

                            b.Draw(
                                Mod.instance.iconData.relicsTexture,
                                new Vector2(galleryButtons[index].bounds.Center.X - 40f + 1f, galleryButtons[index].bounds.Center.Y - 40f + 3f),
                                Mod.instance.iconData.RelicRectangles(relic.relic),
                                Microsoft.Xna.Framework.Color.Black * 0.35f,
                                0f,
                                Vector2.Zero,
                                4f, 0, 0.900f);

                            b.Draw(
                                Mod.instance.iconData.relicsTexture,
                                new Vector2(galleryButtons[index].bounds.Center.X - 40f, galleryButtons[index].bounds.Center.Y - 40f),
                                Mod.instance.iconData.RelicRectangles(relic.relic),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                4f, 0, 0.901f);

                        }
                        else
                        {

                            IClickableMenu.drawTextureBox(
                                b,
                                Game1.mouseCursors,
                                new Rectangle(384, 396, 15, 15),
                                galleryButtons[index].bounds.X,
                                galleryButtons[index].bounds.Y,
                                galleryButtons[index].bounds.Width,
                                galleryButtons[index].bounds.Height,
                                Color.White,
                                4f,
                                false,
                                -1f
                            );

                            b.Draw(
                                Mod.instance.iconData.relicsTexture,
                                new Vector2(galleryButtons[index].bounds.Center.X - 40f, galleryButtons[index].bounds.Center.Y - 40f),
                                Mod.instance.iconData.RelicRectangles(relic.relic),
                                Microsoft.Xna.Framework.Color.Black * 0.35f,
                                0f,
                                Vector2.Zero,
                                4f, 0, 0.900f);

                        }

                    }

                }

            }

        }

        public void drawDetail(SpriteBatch b)
        {

            List<string> titles = new();

            List<string> descriptions = new();

            List<string> instructions = new();

            List<string> objectives = new();

            List<string> transcripts = new();

            if (dragonMenu)
            {


            }
            else
            if (type == journalTypes.effects)
            {

                string[] effectParts = pages[currentPage][questPage].Split("|");

                titles.Add(Mod.instance.questHandle.effects[effectParts[0]][Convert.ToInt32(effectParts[1])].title);

                descriptions.Add(Mod.instance.questHandle.effects[effectParts[0]][Convert.ToInt32(effectParts[1])].description);

                instructions.Add(Mod.instance.questHandle.effects[effectParts[0]][Convert.ToInt32(effectParts[1])].instruction);

                objectives = Mod.instance.questHandle.effects[effectParts[0]][Convert.ToInt32(effectParts[1])].details;

            }
            else
            if (type == journalTypes.relics)
            {

                titles.Add(Mod.instance.relicsData.reliquary[pages[currentPage][questPage]].title);

                descriptions.Add(Mod.instance.relicsData.reliquary[pages[currentPage][questPage]].description);

                objectives = Mod.instance.relicsData.reliquary[pages[currentPage][questPage]].narrative;

            }
            else
            {
                string questId = pages[currentPage][questPage];

                Quest questRecord = Mod.instance.questHandle.quests[questId];

                bool isActive = Mod.instance.save.progress[questId].status == 1 || Mod.instance.save.progress[questId].status == 4;

                titles.Add(questRecord.title);

                descriptions.Add(questRecord.description);

                int questReward = questRecord.reward;

                if (isActive)
                {

                    if (Context.IsMultiplayer && !Context.IsMainPlayer && questRecord.type != Quest.questTypes.lesson)
                    {

                        instructions.Add(DialogueData.Strings(stringkeys.hostOnly));

                    }
                    else if (Mod.instance.save.progress[questId].status == 4)
                    {

                        instructions.Add(DialogueData.Strings(stringkeys.questReplay));

                        questReward *= 3;

                        questReward /= 2;

                        if(questRecord.replay != null)
                        {

                            objectives.Add(DialogueData.Strings(DialogueData.stringkeys.replayReward) + ": "+questRecord.replay);

                        }

                    }

                    instructions.Add(questRecord.instruction);

                    float adjustReward = 1.2f - ((float)Mod.instance.ModDifficulty() * 0.1f);

                    if (questRecord.type == Quest.questTypes.lesson)
                    {

                        objectives.Add(Mod.instance.save.progress[questId].progress.ToString() + " "+DialogueData.Strings(stringkeys.outOf)+" " + questRecord.requirement.ToString() + " " + questRecord.progression);

                    }

                    if (questReward > 0)
                    {

                        questReward = (int)(questReward * adjustReward);

                        objectives.Add(DialogueData.Strings(stringkeys.reward) + ": " + questReward.ToString() + "g");

                    }

                }
                else
                {

                    if (questRecord.explanation != null)
                    {

                        instructions = new() { questRecord.explanation, };

                    }

                    if (questRecord.type == Quest.questTypes.lesson)
                    {

                        objectives.Add(questRecord.requirement.ToString() + " " + DialogueData.Strings(stringkeys.outOf) + " " + questRecord.requirement.ToString() + " " + questRecord.progression);

                    }

                    objectives = questRecord.details;

                    if (questRecord.type == Quest.questTypes.challenge)
                    {

                        Dictionary<int, Dictionary<int, string>> dialogueScene = DialogueData.DialogueScene(questId);

                        if (dialogueScene.Count > 0)
                        {

                            Dictionary<int, string> narrator = DialogueData.DialogueNarrators(questId);

                            foreach (KeyValuePair<int, string> sceneNarrator in narrator)
                            {

                                transcripts.Add(DialogueData.Strings(stringkeys.transcript) +" " + sceneNarrator.Value);

                                foreach (KeyValuePair<int, Dictionary<int, string>> sceneEntry in dialogueScene)
                                {

                                    if (sceneEntry.Key > 900)
                                    {
                                        continue;
                                    }

                                    if (sceneEntry.Value.ContainsKey(sceneNarrator.Key))
                                    {

                                        transcripts.Add(sceneEntry.Value[sceneNarrator.Key]);

                                    }

                                }

                            }


                        }

                    }

                    if (Mod.instance.questHandle.loresets.ContainsKey(questId))
                    {

                        foreach (LoreData.stories story in Mod.instance.questHandle.loresets[questId])
                        {

                            if (Mod.instance.questHandle.lores.ContainsKey(story))
                            {

                                transcripts.Add(Mod.instance.questHandle.lores[story].answer);

                                transcripts.Add("(" + CharacterHandle.CharacterTitle(Mod.instance.questHandle.lores[story].character) + ")");

                            }

                        }

                    }

                }

                if (Context.IsMainPlayer)
                {

                    drawQuestingButton(b,questId);

                }

            }

            int heightOffset = 32;

            if (titles.Count > 0)
            {

                SpriteText.drawStringHorizontallyCenteredAt(b, titles.First(), xPositionOnScreen + width / 2, yPositionOnScreen + 32, 999999, -1, 999999, 1f, 0.88f, false, null, 99999);

                heightOffset = 96;

            }

            Rectangle scissorRectangle = b.GraphicsDevice.ScissorRectangle;

            Rectangle rectangle = new Rectangle()
            {
                X = xPositionOnScreen + 32,
                Y = yPositionOnScreen + heightOffset
            };

            rectangle.Height = height - 32 - heightOffset;

            rectangle.Width = width - 64;

            _scissorRectHeight = rectangle.Height;

            Rectangle screen = Utility.ConstrainScissorRectToScreen(rectangle);

            b.End();

            SpriteBatch spriteBatch2 = b;

            BlendState alphaBlend = BlendState.AlphaBlend;

            SamplerState pointClamp = SamplerState.PointClamp;

            RasterizerState rasterizerState = new RasterizerState();

            rasterizerState.ScissorTestEnable = true;

            Matrix? nullable = new Matrix?();

            spriteBatch2.Begin(0, alphaBlend, pointClamp, null, rasterizerState, null, nullable);

            Game1.graphics.GraphicsDevice.ScissorRectangle = screen;

            float textHeight = (float)yPositionOnScreen - scrollAmount + 32f;

            // -------------------------------------------------------
            // description

            if (descriptions.Count > 0)
            {

                string descriptionText = Game1.parseText(descriptions.First(), Game1.dialogueFont, width - 128);

                Vector2 vector2 = Game1.dialogueFont.MeasureString(descriptionText);

                b.DrawString(Game1.dialogueFont, descriptionText, new Vector2(xPositionOnScreen + 64, (float)(yPositionOnScreen - (double)scrollAmount + 96.0)), Game1.textColor, 0f, Vector2.Zero, 1, SpriteEffects.None, -1f);

                b.DrawString(Game1.dialogueFont, descriptionText, new Vector2(xPositionOnScreen + 64 - 1.5f, (float)(yPositionOnScreen - (double)scrollAmount + 96.0) + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1, SpriteEffects.None, -1.1f);

                textHeight += 96 + vector2.Y;

            }

            // -------------------------------------------------------
            // instruction / explanation

            if (instructions.Count > 0)
            {

                for (int index = 0; index < instructions.Count; index++)
                {

                    string text2 = Game1.parseText(instructions[index], Game1.dialogueFont, width - 128);

                    b.DrawString(Game1.dialogueFont, text2, new Vector2(xPositionOnScreen + 64, textHeight - 8f), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.dialogueFont, text2, new Vector2(xPositionOnScreen + 64 - 1.5f, textHeight - 8f + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    textHeight += Game1.dialogueFont.MeasureString(text2).Y;

                }

            }

            // ------------------------------------------------------
            // extra details

            if (objectives.Count > 0)
            {

                textHeight += 16;

                for (int index = 0; index < objectives.Count; index++)
                {

                    string objectiveParse = Game1.parseText(objectives[index], Game1.dialogueFont, width - 128);

                    b.DrawString(Game1.dialogueFont, objectiveParse, new Vector2(xPositionOnScreen + 64, textHeight - 8f), Color.DarkGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.dialogueFont, objectiveParse, new Vector2(xPositionOnScreen + 64 - 1.5f, textHeight - 8f + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    textHeight += Game1.dialogueFont.MeasureString(objectiveParse).Y;

                }

            }


            // ------------------------------------------------------
            // transcripts

            if (transcripts.Count > 0)
            {


                textHeight += 16;

                for (int index = 0; index < transcripts.Count; ++index)
                {

                    string transcriptParse = Game1.parseText(transcripts[index], Game1.dialogueFont, width - 128);

                    b.DrawString(Game1.dialogueFont, transcriptParse, new Vector2(xPositionOnScreen + 64, textHeight - 8f), Color.DarkBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.dialogueFont, transcriptParse, new Vector2(xPositionOnScreen + 64 - 1.5f, textHeight - 8f + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    textHeight += Game1.dialogueFont.MeasureString(transcriptParse).Y;

                }

            }

            if (dragonMenu)
            {

                drawDragonMenu(b, (int)textHeight);

                textHeight += 544;

            }


            _contentHeight = textHeight + scrollAmount - screen.Y;

            b.End();

            b.GraphicsDevice.ScissorRectangle = scissorRectangle;

            b.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, new Matrix?());

            if (NeedsScroll())
            {
                if (scrollAmount > 0.0)
                {
                    b.Draw(Game1.staminaRect, new Rectangle(screen.X, screen.Top, screen.Width, 4), Color.Black * 0.15f);
                }

                if (scrollAmount < _contentHeight - (double)_scissorRectHeight)
                {
                    b.Draw(Game1.staminaRect, new Rectangle(screen.X, screen.Bottom - 4, screen.Width, 4), Color.Black * 0.15f);
                }

            }

        }

        public void drawHoverDetail(SpriteBatch b)
        {

            string title;

            string description;

            List<string> details = new();

            Dictionary<string, int> items = new();

            Dictionary<HerbalData.herbals, int> potions = new();

            if (pages[currentPage].Count <= hoverDetail)
            {

                hoverDetail = -1;

                return;

            }

            if (type == journalTypes.herbalism)
            {
                
                if (pages[currentPage][hoverDetail] == "configure")
                {

                    HerbalData.herbals potion = herbals.ligna;

                    switch (hoverDetail)
                    {

                        case 11:

                            potion = herbals.impes;
                            break;

                        case 17:

                            potion = herbals.celeri;
                            break;

                    }

                    Herbal herbal = Mod.instance.herbalData.herbalism[potion.ToString()];

                    title = herbal.title;

                    description = DialogueData.Strings(stringkeys.acEnabled);

                    if (Mod.instance.save.potions.ContainsKey(potion))
                    {

                        switch (Mod.instance.save.potions[potion])
                        {

                            case 0:

                                description = DialogueData.Strings(stringkeys.acDisabled);

                                break;

                            case 2:

                                description = DialogueData.Strings(stringkeys.acPriority);

                                break;


                        }

                    }


                }
                else
                {

                    Herbal herbal = Mod.instance.herbalData.herbalism[pages[currentPage][hoverDetail]];

                    title = herbal.title;

                    int amount = 0;

                    if (Mod.instance.save.herbalism.ContainsKey(herbal.herbal))
                    {

                        amount = Mod.instance.save.herbalism[herbal.herbal];

                        title += " x" + amount.ToString();

                    }

                    int baseAmount = 0;

                    if (herbal.bases.Count > 0)
                    {

                        foreach (herbals basePotion in herbal.bases)
                        {

                            int potionAmount = 0;

                            if (Mod.instance.save.herbalism.ContainsKey(basePotion))
                            {

                                potionAmount = Mod.instance.save.herbalism[basePotion];

                                if (baseAmount == 0)
                                {

                                    baseAmount = potionAmount;

                                }
                                else
                                {
                                    
                                    baseAmount = Math.Min(potionAmount, baseAmount);

                                }

                            }

                            potions.Add(basePotion, potionAmount);

                        }

                    }

                    if (herbal.status == 3)
                    {

                        title += " "+DialogueData.Strings(stringkeys.MAX);

                    }
                    else if (herbal.status == 1)
                    {

                        int craftable = 0;

                        foreach (KeyValuePair<string, int> ingredient in herbal.amounts)
                        {

                            craftable += ingredient.Value;

                        }

                        craftable = Math.Min(999 - amount, craftable);

                        if (herbal.bases.Count > 0)
                        {

                            craftable = Math.Min(craftable,baseAmount);

                        }

                        title += " (" + craftable.ToString() + ")";

                    }

                    description = herbal.description;

                    details = new(herbal.details);

                    foreach (KeyValuePair<string, string> ingredient in herbal.ingredients)
                    {

                        int herbalAmount = 0;

                        if (herbal.amounts.ContainsKey(ingredient.Key))
                        {

                            herbalAmount = herbal.amounts[ingredient.Key];

                        }

                        items.Add(ingredient.Key, herbalAmount);

                    }

                }

            }
            else
            {
                if (pages[currentPage][hoverDetail] == "blank")
                {
                    return;
                }

                string relicId = pages[currentPage][hoverDetail];

                Relic relic = Mod.instance.relicsData.reliquary[relicId];

                if (Mod.instance.save.reliquary.ContainsKey(relicId))
                {

                    title = relic.title;

                    description = relic.description;

                    details = new(relic.details);

                    if (relic.function)
                    {

                        string relicInstruction = Mod.instance.relicsData.RelicInstruction(relic.relic.ToString());

                        if (relicInstruction != null)
                        {

                            details.Add(relicInstruction);

                        }

                    }

                }
                else
                {

                    title = DialogueData.Strings(stringkeys.relicUnknown);

                    description = DialogueData.Strings(stringkeys.relicNotFound);

                    details.Add(relic.hint);

                    //details.AddRange(relic.details);

                }

            }

            float contentHeight = 16;

            // -------------------------------------------------------
            // title

            string titleText = Game1.parseText(title, Game1.dialogueFont, 476);

            Vector2 titleSize = Game1.dialogueFont.MeasureString(titleText);

            contentHeight += 24 + titleSize.Y;

            // -------------------------------------------------------
            // description

            string descriptionText = Game1.parseText(description, Game1.smallFont, 476);

            Vector2 descriptionSize = Game1.smallFont.MeasureString(descriptionText);

            contentHeight += 24 + descriptionSize.Y;

            if (details.Count > 0)
            {
                
                foreach (string detail in details)
                {

                    string detailText = Game1.parseText(detail, Game1.smallFont, 476);

                    Vector2 detailSize = Game1.smallFont.MeasureString(detailText);

                    contentHeight += detailSize.Y;

                }

                contentHeight += 24;

            }

            if (potions.Count > 0 || items.Count > 0)
            {

                contentHeight += (32 * potions.Count);

                contentHeight += (32 * items.Count);

                contentHeight += 24;
            
            }

            // -------------------------------------------------------
            // texturebox

            int cornerX = Game1.getMouseX() + 32;

            int cornerY = Game1.getMouseY() + 32;

            if (cornerX > Game1.graphics.GraphicsDevice.Viewport.Width - 512)
            {

                int tryCorner = cornerX - 576;

                cornerX = tryCorner < 0 ? 0 : tryCorner;

            }

            if(cornerY > Game1.graphics.GraphicsDevice.Viewport.Height - contentHeight - 48)
            {

                int tryCorner = cornerY - (int)(contentHeight + 64f);

                cornerY = tryCorner < 0 ? 0 : tryCorner;

            }

            Vector2 corner = new(cornerX, cornerY);

            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), (int)corner.X, (int)corner.Y, 512, (int)(contentHeight), Color.White, 1f, true, -1f);

            float textPosition = corner.Y + 16;

            float textMargin = corner.X + 16;

            // -------------------------------------------------------
            // title

            b.DrawString(Game1.dialogueFont, titleText, new Vector2(textMargin, textPosition), Game1.textColor, 0f, Vector2.Zero, 1, SpriteEffects.None, -1f);

            b.DrawString(Game1.dialogueFont, titleText, new Vector2(textMargin - 1.5f, textPosition + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1, SpriteEffects.None, -1.1f);

            textPosition += 8 + titleSize.Y;

            Color outerTop = new(167, 81, 37);

            Color outerBot = new(139, 58, 29);

            Color inner = new(246, 146, 30);

            // --------------------------------
            // top

            b.Draw(Game1.staminaRect, new Rectangle((int)textMargin - 4, (int)textPosition, 488, 2), outerTop);
            b.Draw(Game1.staminaRect, new Rectangle((int)textMargin - 4, (int)textPosition + 2, 488, 3), inner);

            //Utility.drawLineWithScreenCoordinates((int)textMargin, (int)textPosition, (int)textMargin + 476, (int)textPosition, b, Color.SaddleBrown * 0.9f);
            //Utility.drawLineWithScreenCoordinates((int)textMargin, (int)textPosition + 1, (int)textMargin + 476 + 1, (int)textPosition, b, Microsoft.Xna.Framework.Color.Brown * 0.35f);

            textPosition += 12;

            // -------------------------------------------------------
            // description

            b.DrawString(Game1.smallFont, descriptionText, new Vector2(textMargin, textPosition), Game1.textColor, 0f, Vector2.Zero, 1, SpriteEffects.None, -1f);

            b.DrawString(Game1.smallFont, descriptionText, new Vector2(textMargin - 1.5f, textPosition + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1, SpriteEffects.None, -1.1f);

            textPosition += 12 + descriptionSize.Y;

            // -------------------------------------------------------
            // details

            if(details.Count > 0)
            {
                b.Draw(Game1.staminaRect, new Rectangle((int)textMargin - 4, (int)textPosition, 488, 2), outerTop);
                b.Draw(Game1.staminaRect, new Rectangle((int)textMargin - 4, (int)textPosition + 2, 488, 3), inner);

                //Utility.drawLineWithScreenCoordinates((int)textMargin, (int)textPosition, (int)textMargin + 476, (int)textPosition, b, Color.SaddleBrown * 0.9f);
                //Utility.drawLineWithScreenCoordinates((int)textMargin, (int)textPosition + 1, (int)textMargin + 476 + 1, (int)textPosition, b, Microsoft.Xna.Framework.Color.Brown * 0.35f);

                textPosition += 12;

                foreach (string detail in details)
                {

                    string detailText = Game1.parseText(detail, Game1.smallFont, 476);

                    Vector2 detailSize = Game1.smallFont.MeasureString(detailText);

                    b.DrawString(Game1.smallFont, detailText, new Vector2(textMargin, textPosition), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.smallFont, detailText, new Vector2(textMargin - 1.5f, textPosition + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    textPosition += detailSize.Y;

                }

                textPosition += 12;

            }

            // ---------------------------------------------------------
            // items

            if (potions.Count > 0 || items.Count > 0)
            {
                b.Draw(Game1.staminaRect, new Rectangle((int)textMargin - 4, (int)textPosition, 488, 2), outerTop);
                b.Draw(Game1.staminaRect, new Rectangle((int)textMargin - 4, (int)textPosition + 2, 488, 3), inner);

                //Utility.drawLineWithScreenCoordinates((int)textMargin, (int)textPosition, (int)textMargin + 476, (int)textPosition, b, Color.SaddleBrown * 0.9f);
                //Utility.drawLineWithScreenCoordinates((int)textMargin, (int)textPosition + 1, (int)textMargin + 476 + 1, (int)textPosition, b, Microsoft.Xna.Framework.Color.Brown * 0.35f);

                textPosition += 12;

                foreach (KeyValuePair<herbals, int> potion in potions)
                {

                    Herbal basePotion = Mod.instance.herbalData.herbalism[potion.Key.ToString()];

                    Microsoft.Xna.Framework.Color colour = Mod.instance.iconData.SchemeColour(basePotion.scheme);

                    b.Draw(Mod.instance.iconData.relicsTexture, new Vector2(textMargin, textPosition), Mod.instance.iconData.RelicRectangles(basePotion.container), Color.White, 0f, Vector2.Zero, 1.5f, 0, 0.901f);

                    b.Draw(Mod.instance.iconData.relicsTexture, new Vector2(textMargin, textPosition), Mod.instance.iconData.RelicRectangles(basePotion.content), colour, 0f, Vector2.Zero, 1.5f, 0, 0.902f);

                    b.DrawString(Game1.smallFont, basePotion.title, new Vector2(textMargin + 40, textPosition + 2), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.smallFont, basePotion.title, new Vector2(textMargin + 40 - 1.5f, textPosition + 2 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    string amount = "(" + potion.Value + ")";

                    b.DrawString(Game1.smallFont, amount, new Vector2(textMargin + 476 - (amount.Length * 16), textPosition + 2), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.smallFont, amount, new Vector2(textMargin + 476 - (amount.Length * 16) - 1.5f, textPosition + 2 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    textPosition += 32;

                }

                foreach (KeyValuePair<string, int> item in items)
                {

                    ParsedItemData dataOrErrorItem = ItemRegistry.GetDataOrErrorItem(item.Key);

                    b.Draw(dataOrErrorItem.GetTexture(), new Vector2(textMargin, textPosition), dataOrErrorItem.GetSourceRect(), Color.White, 0f, Vector2.Zero, 2f, 0, 1f);

                    b.DrawString(Game1.smallFont, dataOrErrorItem.DisplayName, new Vector2(textMargin + 40, textPosition + 2), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.smallFont, dataOrErrorItem.DisplayName, new Vector2(textMargin + 40 - 1.5f, textPosition + 2 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    string amount = "(" + item.Value + ")";

                    b.DrawString(Game1.smallFont, amount, new Vector2(textMargin + 476 - (amount.Length * 16), textPosition + 2), Game1.textColor * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1f);

                    b.DrawString(Game1.smallFont, amount, new Vector2(textMargin + 476 - (amount.Length * 16) - 1.5f, textPosition + 2 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1f, SpriteEffects.None, -1.1f);

                    textPosition += 32;
                
                }

            }

        }

        public void drawStats(SpriteBatch b)
        {
            
            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.HP)+" " + Game1.player.health + "/" + Game1.player.maxHealth, new Vector2(xPositionOnScreen + width - 304, yPositionOnScreen - 64), Color.Wheat, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.88f);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.STM) +" " + (int)Game1.player.Stamina + " /" + Game1.player.MaxStamina, new Vector2(xPositionOnScreen + width - 304, yPositionOnScreen - 32), Color.Wheat, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.88f);

        }

        public void drawQuestingButton(SpriteBatch b, string questId)
        {

            if (!Mod.instance.save.progress.ContainsKey(questId))
            {
                return;
            }

            if (!Mod.instance.questHandle.IsReplayable(questId) && Mod.instance.save.progress[questId].status > 1)
            {
                return;
            }

            IconData.displays questIcon;

            switch (Mod.instance.save.progress[questId].status)
            {

                default:
                case 1:

                    questIcon = IconData.displays.end;

                    break;
                case 2:

                    questIcon = IconData.displays.replay;

                    break;
                case 3:

                    questIcon = IconData.displays.flag;

                    break;
                case 4:

                    questIcon = IconData.displays.active;

                    break;

            }

            b.Draw(
                Mod.instance.iconData.displayTexture,
                new Vector2(questingButton.bounds.X, questingButton.bounds.Y) - ((questingButton.scale - 4f) * new Vector2(16, 16)),
                Mod.instance.iconData.DisplayRect(questIcon),
                Color.White,
                0f,
                Vector2.Zero,
                questingButton.scale,
                0,
                -1f
            );


        }

        public void drawDragonMenu(SpriteBatch b, int startHeight)
        {

            int center = xPositionOnScreen + (width / 2);

            IClickableMenu.drawTextureBox(
                b,
                Game1.mouseCursors,
                new Rectangle(384, 396, 15, 15),
                dragonPortrait.bounds.X,
                dragonPortrait.bounds.Y,
                dragonPortrait.bounds.Width,
                dragonPortrait.bounds.Height,
                Color.Wheat,
                4f,
                false,
                -1.2f
            );

            List<Color> embers = Mod.instance.iconData.gradientColours[(IconData.schemes)breathScheme];

            for (int e = 0; e < 4; e++)
            {

                Vector2 emberVector = new Vector2(dragonPortrait.bounds.Left, dragonPortrait.bounds.Bottom) + new Vector2(16 + 64 * e, -96);

                b.Draw(Mod.instance.iconData.emberTexture, emberVector, new(32 * e, 0, 32, 28), embers[2] * 0.65f, 0f, Vector2.Zero, 3f, 0, -1.1f);

                b.Draw(Mod.instance.iconData.emberTexture, emberVector, new(32 * e, 96, 32, 28), embers[1] * 0.65f, 0f, Vector2.Zero, 3f, 0, -1.1f);

                b.Draw(Mod.instance.iconData.emberTexture, emberVector, new(32 * e, 192, 32, 28), embers[0] * 0.65f, 0f, Vector2.Zero, 3f, 0, -1.1f);

                b.Draw(Mod.instance.iconData.emberTexture, emberVector, new(32 * e, 288, 32, 28), Color.White * 0.65f, 0f, Vector2.Zero, 3f, 0, -1.1f);

            }

            dragonRender.drawWalk(b, new(dragonPortrait.bounds.X + 128, dragonPortrait.bounds.Y + 224), new() { direction = dragonDirection, frame = 0, layer = -1f, flip = dragonDirection == 3 });

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.primaryColour), new Vector2(center + 32, yPositionOnScreen + 64), Game1.textColor, 0f, Vector2.Zero, 1, SpriteEffects.None, -1f);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.primaryColour), new Vector2(center + 32 - 1.5f, yPositionOnScreen + 64 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1, SpriteEffects.None, -1.1f);

            dragonPrimary.draw(b);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.secondaryColour), new Vector2(center + 32, yPositionOnScreen + 168), Game1.textColor, 0f, Vector2.Zero, 1, SpriteEffects.None, -1f);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.secondaryColour), new Vector2(center + 32 - 1.5f, yPositionOnScreen + 168 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1, SpriteEffects.None, -1.1f);

            dragonSecondary.draw(b);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.tertiaryColour), new Vector2(center + 32, yPositionOnScreen + 272), Game1.textColor, 0f, Vector2.Zero, 1, SpriteEffects.None, -1f);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.tertiaryColour), new Vector2(center + 32 - 1.5f, yPositionOnScreen + 272 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1, SpriteEffects.None, -1.1f);

            dragonTertiary.draw(b);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.dragonScheme), new Vector2(dragonPortrait.bounds.X, yPositionOnScreen + 372), Game1.textColor, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1f);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.dragonScheme), new Vector2(dragonPortrait.bounds.X - 1.5f, yPositionOnScreen + 372 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1.1f);

            foreach (KeyValuePair<IconData.schemes, ClickableComponent> dragonComponent in dragonSchemes)
            {

                IconData.schemes scheme = dragonComponent.Key;

                ClickableComponent component = dragonComponent.Value;

                Color highlight = Color.Wheat;

                if (Mod.instance.Config.dragonScheme == (int)scheme)
                {

                    highlight = Color.White;

                }

                IClickableMenu.drawTextureBox(
                    b,
                    Game1.mouseCursors,
                    new Rectangle(384, 396, 15, 15),
                    component.bounds.X,
                    component.bounds.Y,
                    component.bounds.Width,
                    component.bounds.Height,
                    highlight,
                    2f,
                    false,
                    -1.1f
                );

                b.Draw(Game1.staminaRect, new Vector2(component.bounds.X + 9, component.bounds.Y + 9), new Rectangle(component.bounds.X + 9, component.bounds.Y + 9, component.bounds.Width - 18, component.bounds.Height - 18), Mod.instance.iconData.gradientColours[scheme][0], 0f, Vector2.Zero, 1f, 0, -1f);

            }

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.breathScheme), new Vector2(dragonPortrait.bounds.X, yPositionOnScreen + 480), Game1.textColor, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1f);

            b.DrawString(Game1.smallFont, DialogueData.Strings(stringkeys.breathScheme), new Vector2(dragonPortrait.bounds.X - 1.5f, yPositionOnScreen + 480 + 1.5f), Microsoft.Xna.Framework.Color.Brown * 0.35f, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, -1.1f);

            foreach (KeyValuePair<IconData.schemes, ClickableComponent> breathComponent in breathSchemes)
            {

                IconData.schemes scheme = breathComponent.Key;

                ClickableComponent component = breathComponent.Value;

                Color highlight = Color.Wheat;

                if (Mod.instance.Config.dragonBreath == (int)scheme)
                {

                    highlight = Color.White;

                }

                IClickableMenu.drawTextureBox(
                    b,
                    Game1.mouseCursors,
                    new Rectangle(384, 396, 15, 15),
                    component.bounds.X,
                    component.bounds.Y,
                    component.bounds.Width,
                    component.bounds.Height,
                    highlight,
                    2f,
                    false,
                    -1.1f
                );

                b.Draw(Game1.staminaRect, new Vector2(component.bounds.X + 9, component.bounds.Y + 9), new Rectangle(component.bounds.X + 9, component.bounds.Y + 9, component.bounds.Width - 18, component.bounds.Height - 18), Mod.instance.iconData.gradientColours[scheme][1], 0f, Vector2.Zero, 1f, 0, -1f);

            }

            b.Draw(Mod.instance.iconData.displayTexture, new Vector2(resetButton.bounds.X, resetButton.bounds.Y) - ((resetButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.replay), Color.White, 0f, Vector2.Zero, resetButton.scale, 0, -1f);

            b.Draw(Mod.instance.iconData.displayTexture, new Vector2(saveButton.bounds.X, saveButton.bounds.Y) - ((saveButton.scale - 4f) * new Vector2(16, 16)), Mod.instance.iconData.DisplayRect(Data.IconData.displays.complete), Color.White, 0f, Vector2.Zero, saveButton.scale, 0, -1f);

        }

    }

}
