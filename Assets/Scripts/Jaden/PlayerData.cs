using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int Karma;
    public int CurrentDay;

    #region Items 

    public bool hasMap;

    public bool hasScarf;
    public bool returnedScarf;

    public bool hasOilLantern;

    public bool hasHeirloomAxe;
    public bool returnedHeirloomAxe;

    public bool hasJournal;

    public bool hasJeweleryBox;
    public bool returnedJeweleryBox;

    public bool hasWovenBasket;

    public bool hasWarMedal;

    public bool hasHairLocket;
    public bool returnedHairLocket;

    public bool hasWhiteSariScrap;

    public bool hasDogTags;

    public bool hasReligiousIcon;

    public bool hasDoll;
    public bool returnedDoll;

    public bool hasWaterloggedPistol;

    public bool hasFamilyPhoto;
    public bool returnedFamilyPhoto;

    public bool hasPrayerBeads;

    public bool hasWoodenAnimals;

    #endregion

    #region Returnable Item Functions
    public bool ScarfInInventory()
    {
        return (hasScarf && !returnedScarf);
    }

    public bool AxeInInventory()
    {
        return (hasHeirloomAxe && !returnedHeirloomAxe);
    }

    public bool JeweleryInInventory()
    {
        return (hasJeweleryBox && !returnedJeweleryBox);
    }

    public bool LocketInInventory()
    {
        return (hasHairLocket && !returnedHairLocket);
    }

    public bool DollInInventory()
    {
        return (hasDoll && !returnedDoll);
    }

    public bool PhotoInInventory()
    {
        return (hasFamilyPhoto && !returnedFamilyPhoto);
    }

    #endregion
}
