using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonSoulsItemRandomiser.Models
{
    public class ItemLot
    {
        public int ID { get; set; }
        public int Rarity { get; set; }
        public string Description { get; set; }

        public int Pad1 { get; set; }
        public int Category01 { get; set; }
        public int ItemID { get; set; }
        public int ItemNum01 { get; set; }
        public int ItemBasePoint { get; set; }
        public int QWCBasePoint { get; set; }
        public int QWCAppliesPoint { get; set; }
        public bool EnablesLuck01 { get; set; }

        public int Pad2 { get; set; }
        public int Category02 { get; set; }
        public int ItemID02 { get; set; }
        public int ItemNum02 { get; set; }
        public int ItemBasePoint02 { get; set; }
        public int QWCBasePoint02 { get; set; }
        public int QWCAppliesPoint02 { get; set; }
        public bool EnablesLuck02 { get; set; }

        public int Pad3 { get; set; }
        public int Category03 { get; set; }
        public int ItemID03 { get; set; }
        public int ItemNum03 { get; set; }
        public int ItemBasePoint03 { get; set; }
        public int QWCBasePoint03 { get; set; }
        public int QWCAppliesPoint03 { get; set; }
        public bool EnablesLuck0103 { get; set; }

        public int Pad4 { get; set; }
        public int Category04 { get; set; }
        public int ItemID04 { get; set; }
        public int ItemNum04 { get; set; }
        public int ItemBasePoint04 { get; set; }
        public int QWCBasePoint04 { get; set; }
        public int QWCAppliesPoint04 { get; set; }
        public bool EnablesLuck04 { get; set; }

        public int Pad5 { get; set; }
        public int Category05 { get; set; }
        public int ItemID05 { get; set; }
        public int ItemNum05 { get; set; }
        public int ItemBasePoint05 { get; set; }
        public int QWCBasePoint05 { get; set; }
        public int QWCAppliesPoint05 { get; set; }
        public bool EnablesLuck05 { get; set; }

        public int Pad6 { get; set; }
        public int Category06 { get; set; }
        public int ItemID06 { get; set; }
        public int ItemNum06 { get; set; }
        public int ItemBasePoint06 { get; set; }
        public int QWCBasePoint06 { get; set; }
        public int QWCAppliesPoint06 { get; set; }
        public bool EnablesLuck06 { get; set; }

        public int Pad7 { get; set; }
        public int Category07 { get; set; }
        public int ItemID07 { get; set; }
        public int ItemNum07 { get; set; }
        public int ItemBasePoint07 { get; set; }
        public int QWCBasePoint07 { get; set; }
        public int QWCAppliesPoint07 { get; set; }
        public bool EnablesLuck07 { get; set; }

        public int Pad8 { get; set; }
        public int Category08 { get; set; }
        public int ItemID08 { get; set; }
        public int ItemNum08 { get; set; }
        public int ItemBasePoint08 { get; set; }
        public int QWCBasePoint08 { get; set; }
        public int QWCAppliesPoint08 { get; set; }
        public bool EnablesLuck08 { get; set; }

        public int Pad9 { get; set; }
        public int Category09 { get; set; }
        public int ItemID09 { get; set; }
        public int ItemNum09 { get; set; }
        public int ItemBasePoint09 { get; set; }
        public int QWCBasePoint09 { get; set; }
        public int QWCAppliesPoint09 { get; set; }
        public bool EnablesLuck09 { get; set; }

        public int Pad10 { get; set; }
        public int Category010 { get; set; }
        public int ItemID010 { get; set; }
        public int ItemNum010 { get; set; }
        public int ItemBasePoint010 { get; set; }
        public int QWCBasePoint010 { get; set; }
        public int QWCAppliesPoint010 { get; set; }
        public bool EnablesLuck010 { get; set; }

        public ItemLot(int iD, int rarity, string description, int pad1, int category01, int itemID, 
            int itemNum01, int itemBasePoint, int qWCBasePoint, int qWCAppliesPoint, bool enablesLuck01, 
            int pad2, int category02, int itemID02, int itemNum02, int itemBasePoint02, int qWCBasePoint02, int qWCAppliesPoint02, bool enablesLuck02, 
            int pad3, int category03, int itemID03, int itemNum03, int itemBasePoint03, int qWCBasePoint03, int qWCAppliesPoint03, bool enablesLuck0103, 
            int pad4, int category04, int itemID04, int itemNum04, int itemBasePoint04, int qWCBasePoint04, int qWCAppliesPoint04, bool enablesLuck04, 
            int pad5, int category05, int itemID05, int itemNum05, int itemBasePoint05, int qWCBasePoint05, int qWCAppliesPoint05, bool enablesLuck05, 
            int pad6, int category06, int itemID06, int itemNum06, int itemBasePoint06, int qWCBasePoint06, int qWCAppliesPoint06, bool enablesLuck06, 
            int pad7, int category07, int itemID07, int itemNum07, int itemBasePoint07, int qWCBasePoint07, int qWCAppliesPoint07, bool enablesLuck07, 
            int pad8, int category08, int itemID08, int itemNum08, int itemBasePoint08, int qWCBasePoint08, int qWCAppliesPoint08, bool enablesLuck08, 
            int pad9, int category09, int itemID09, int itemNum09, int itemBasePoint09, int qWCBasePoint09, int qWCAppliesPoint09, bool enablesLuck09, 
            int pad10, int category010, int itemID010, int itemNum010, int itemBasePoint010, int qWCBasePoint010, int qWCAppliesPoint010, bool enablesLuck010)
        {
            ID = iD;
            Rarity = rarity;
            Description = description;
            Pad1 = pad1;
            Category01 = category01;
            ItemID = itemID;
            ItemNum01 = itemNum01;
            ItemBasePoint = itemBasePoint;
            QWCBasePoint = qWCBasePoint;
            QWCAppliesPoint = qWCAppliesPoint;
            EnablesLuck01 = enablesLuck01;
            Pad2 = pad2;
            Category02 = category02;
            ItemID02 = itemID02;
            ItemNum02 = itemNum02;
            ItemBasePoint02 = itemBasePoint02;
            QWCBasePoint02 = qWCBasePoint02;
            QWCAppliesPoint02 = qWCAppliesPoint02;
            EnablesLuck02 = enablesLuck02;
            Pad3 = pad3;
            Category03 = category03;
            ItemID03 = itemID03;
            ItemNum03 = itemNum03;
            ItemBasePoint03 = itemBasePoint03;
            QWCBasePoint03 = qWCBasePoint03;
            QWCAppliesPoint03 = qWCAppliesPoint03;
            EnablesLuck0103 = enablesLuck0103;
            Pad4 = pad4;
            Category04 = category04;
            ItemID04 = itemID04;
            ItemNum04 = itemNum04;
            ItemBasePoint04 = itemBasePoint04;
            QWCBasePoint04 = qWCBasePoint04;
            QWCAppliesPoint04 = qWCAppliesPoint04;
            EnablesLuck04 = enablesLuck04;
            Pad5 = pad5;
            Category05 = category05;
            ItemID05 = itemID05;
            ItemNum05 = itemNum05;
            ItemBasePoint05 = itemBasePoint05;
            QWCBasePoint05 = qWCBasePoint05;
            QWCAppliesPoint05 = qWCAppliesPoint05;
            EnablesLuck05 = enablesLuck05;
            Pad6 = pad6;
            Category06 = category06;
            ItemID06 = itemID06;
            ItemNum06 = itemNum06;
            ItemBasePoint06 = itemBasePoint06;
            QWCBasePoint06 = qWCBasePoint06;
            QWCAppliesPoint06 = qWCAppliesPoint06;
            EnablesLuck06 = enablesLuck06;
            Pad7 = pad7;
            Category07 = category07;
            ItemID07 = itemID07;
            ItemNum07 = itemNum07;
            ItemBasePoint07 = itemBasePoint07;
            QWCBasePoint07 = qWCBasePoint07;
            QWCAppliesPoint07 = qWCAppliesPoint07;
            EnablesLuck07 = enablesLuck07;
            Pad8 = pad8;
            Category08 = category08;
            ItemID08 = itemID08;
            ItemNum08 = itemNum08;
            ItemBasePoint08 = itemBasePoint08;
            QWCBasePoint08 = qWCBasePoint08;
            QWCAppliesPoint08 = qWCAppliesPoint08;
            EnablesLuck08 = enablesLuck08;
            Pad9 = pad9;
            Category09 = category09;
            ItemID09 = itemID09;
            ItemNum09 = itemNum09;
            ItemBasePoint09 = itemBasePoint09;
            QWCBasePoint09 = qWCBasePoint09;
            QWCAppliesPoint09 = qWCAppliesPoint09;
            EnablesLuck09 = enablesLuck09;
            Pad10 = pad10;
            Category010 = category010;
            ItemID010 = itemID010;
            ItemNum010 = itemNum010;
            ItemBasePoint010 = itemBasePoint010;
            QWCBasePoint010 = qWCBasePoint010;
            QWCAppliesPoint010 = qWCAppliesPoint010;
            EnablesLuck010 = enablesLuck010;
        }
    }
}
