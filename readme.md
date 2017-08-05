# Adventure Game

This project of a simple adventure game was created by 4 college students of business information systems during their first semester at the University of Applied Sciences - Hochschule Karlsruhe. The game was programmed in c# with simple algorithms and object structures.

https://youtu.be/8LXClhxrSWE



## Impressions

GUI in Windows Forms

![WindowsFormsGUI](https://github.com/LangeJustin/AdventureGame/blob/master/description/windowsFormsGUI.PNG)

Starting the Game

![startGame](https://github.com/LangeJustin/AdventureGame/blob/master/description/startGame.PNG)

Attacking monsters

![attackMonsters](https://github.com/LangeJustin/AdventureGame/blob/master/description/attackMonsters.PNG)

Gambling with deamon powers

![gamblingHell](https://github.com/LangeJustin/AdventureGame/blob/master/description/gamblingHell.PNG)



## Code examples

#### Monsters moving towards hero to attack

```c#
public void MonsterAction()   
{
        int i = 0;

        for (i = 0; i < AllMonsters.Count; i++)
        {
            // Monsters next to Hero attack (<= 1 tile away)
            if (AllMonsters[i].xCoo - 1 <= hero.xCoo && 
                hero.xCoo <= AllMonsters[i].xCoo + 1 &&
                AllMonsters[i].yCoo - 1 <= hero.yCoo && 
                hero.yCoo <= AllMonsters[i].yCoo + 1 && 
                AllMonsters[i].Dead == false)
            {
                statdmgReceived += AllMonsters[i].CurrentDMG; // hinzugefügt
                AllMonsters[i].attackHero(hero);
                refreshStats();
                
                    // Display Danger Symbol at 25% HP
                    if (hero.MaxHP*0.25 > hero.CurrentHP)
                        lowHPalert(true);
                    else
                        lowHPalert(false);                            

                if (hero.CurrentHP < 1)
                {
                    playerDead();
                }

            }
            // Monsters near Hero move towards him (<= 6 tiles away)
            else if (AllMonsters[i].xCoo - 6 <= hero.xCoo && 
                     hero.xCoo <= AllMonsters[i].xCoo + 6 &&
                     AllMonsters[i].yCoo - 6 <= hero.yCoo && 
                     hero.yCoo <= AllMonsters[i].yCoo + 6 && 
                     AllMonsters[i].Dead == false)
            {

                clearMonster(AllMonsters[i]);

                // Random if monster moves or not
                int monsterMayMove = rand.Next(1, 3);

                if (monsterMayMove == 1)
                {
                    if (AllMonsters[i].yCoo > hero.yCoo)
                    {
                        monsterMoveUp(AllMonsters[i]);
                    }
                    if (AllMonsters[i].yCoo < hero.yCoo)
                    {
                        monsterMoveDown(AllMonsters[i]);
                    }
                }
                if (monsterMayMove == 2)
                {
                    if (AllMonsters[i].xCoo > hero.xCoo)
                    {
                        monsterMoveLeft(AllMonsters[i]);
                    }
                    if (AllMonsters[i].xCoo < hero.xCoo)
                    {
                        monsterMoveRight(AllMonsters[i]);
                    }
                }
            }
            if (AllMonsters[i].Dead == false)
                drawMonster(AllMonsters[i], AllMonsters[i].CurrentHP);
            else
                drawDeadMonster(AllMonsters[i]);
        }
    }`
```
#### Collecting loots by hero

```c#
private void buttonLoot_Click(object sender, EventArgs e)
{
        oncePerRound();

        for (int i = 0; i < AllMonsters.Count; i++)
        {
            if (AllMonsters[i].xCoo - 1 <= hero.xCoo && 
                hero.xCoo <= AllMonsters[i].xCoo + 1 && 
                AllMonsters[i].yCoo - 1 <= hero.yCoo && 
                hero.yCoo <= AllMonsters[i].yCoo + 1)
            {
                if (AllMonsters[i].Dead == true && AllMonsters[i].Looted == false)
                {
                    hero.addExpPlayer(AllMonsters[i].RewardExpPoints);
                    hero.Gold += AllMonsters[i].RewardGold;

                    statGoldCollected += AllMonsters[i].RewardGold;

                    int amount = rand.Next(1, 3);
                    if (amount == 2 && rand.Next(0, 4) == 0)
                    {
                        amount = 1;
                    }

                    switch(rand.Next(1,6))
                    {
                        case 1:
                            HPpotion.potCount += amount;
                            lootTextLog(amount, HPpotion);
                            break;
                        case 2:
                            ATKpotion.potCount += amount;
                            lootTextLog(amount, HPpotion);
                            break;
                        case 3:
                            if (amount == 2)
                            {
                                GMpotion.potCount++;
                                labelTextLog.Text = "You found " + GMpotion.Name;
                            }
                            break;
                        case 4:
                            Apple.foodCount += amount;
                            lootTextLog(amount, Apple);
                            break;
                        case 5:
                            Steak.foodCount += amount;
                            lootTextLog(amount, Steak);
                            break;
                        case 6:
                            Beer.foodCount += amount;
                            lootTextLog(amount, Beer);
                            break;
                    }

                    AllMonsters[i].Looted = true;

                    MonsterAction();
                    refreshStats();
                }
            }
        }
    }
```
## Authors

- **Tim Engel** 
- **Justin Lange**
- **Niklas Weber**
- **Simon Broß** 

## Acknowledgments

* Hat tip to anyone who's code was used
* Inspiration
* etc

