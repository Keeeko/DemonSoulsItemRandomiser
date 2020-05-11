using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DemonSoulsItemRandomiser.MSB
{
    public class MSBData
    {
        public struct ColumnStyle
        {
            public Color backgroundColor;
            public Color textColor;

            public ColumnStyle(Color c1, Color c2)
            {
                backgroundColor = c1;
                textColor = c2;
            }
        }

        private static ColumnStyle known = new ColumnStyle(Colors.White, Colors.Black);
        private static ColumnStyle advanced = new ColumnStyle(Colors.Orange, Colors.Black);
        private static ColumnStyle unknown = new ColumnStyle(Colors.LightGray, Colors.Black);
        private static ColumnStyle important = new ColumnStyle(Colors.LightYellow, Colors.Black);

        private List<string> fieldNames;
        private List<string> fieldTypes;
        private List<ColumnStyle> fieldStyles;


        private int nameIdx;
        private int pointIndex1 = -1;
        private int pointIndex2 = -1;
        private int partIndex1 = -1;
        private int partIndex2 = -1;
        private int partIndex3 = -1;


        public void Add(string name, string type, ColumnStyle style)
        {
            fieldNames.Add(name);
            fieldTypes.Add(type);
            fieldStyles.Add(style);
        }

        public string RetrieveName(int i)
        {
            return fieldNames[i];
        }

        public string RetrieveType(int i)
        {
            return fieldTypes[i];
        }

        public Color RetrieveBackColor(int i)
        {
            return fieldStyles[i].backgroundColor;
        }

        public Color RetrieveForeColor(int i)
        {
            return fieldStyles[i].textColor;
        }

        public bool IsUnknown(int i)
        {
            //Should be some sort of type?
            //return fieldStyles[i] is unknown;
            return true;
        }

        public bool IsAdvanced(int i)
        {
            //Should be some sort of type?
            //return fieldStyles[i] is advanced;
            return true;
        }

        public int FieldCount()
        {
            return fieldNames.Count;
        }

        public int GetNameIndex()
        {
            return nameIdx;
        }

        public void SetNameIndex(int idx)
        {
            nameIdx = idx;
        }


        public int GetFieldIndex(string name)
        {
            return fieldNames.IndexOf(name);
        }

        public List<int> GetPointIndices()
        {
            List<int> result = new List<int>();

            if (pointIndex1 != -1) result.Add(pointIndex1);
            if (pointIndex2 != -1) result.Add(pointIndex2);

            return result;
        }

        public List<int> GetPartIndices()
        {
            List<int> result = new List<int>();

            if (partIndex1 != -1) result.Add(partIndex1);
            if (partIndex2 != -1) result.Add(partIndex2);
            if (partIndex3 != -1) result.Add(partIndex3);

            return result;
        }


        private static void EventTemplate(MSBData layout)
        {
            layout.Add("&Name", "i32", advanced);
            layout.Add("Event Index", "i32", known);
            layout.Add("Type", "i32", advanced);
            layout.Add("Index", "i32", known);
            layout.Add("&BaseData", "i32", known);
            layout.Add("&TypeData", "i32", known);
            layout.Add("x18", "i32", unknown);
            layout.SetNameIndex(layout.FieldCount());
            layout.Add("Name", "string", known);
            layout.partIndex1 = layout.FieldCount();
            layout.Add("PartIndex1", "i32", important);
            layout.pointIndex1 = layout.FieldCount();
            layout.Add("RegionIndex1", "i32", important);
            layout.Add("EventEntityID", "i32", important);
            layout.Add("p+0x0C", "i32", unknown);
        }

    public static MSBData Generate(string name)
        {
            MSBData layout = new MSBData();

            if(name == "models")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("Instance Count", "i32", known);
                layout.Add("x14", "i32", unknown);
                layout.Add("x18", "i32", unknown);
                layout.Add("x1C", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
            } 
            else if(name  == "events0")
            {
                EventTemplate(layout);
                layout.Add("p+0x10", "i32", unknown);
            }
            else if(name == "events1")
            {
                EventTemplate(layout);
                layout.Add("Sound Type", "i32", known);
                layout.Add("Sound ID", "i32", known);
            }
            else if(name == "events2")
            {
                EventTemplate(layout);
                layout.Add("ParticleEffectId", "i32", known);
            }
            else if(name == "events3")
            {
                EventTemplate(layout);
                layout.Add("p+0x10", "f32", unknown);
                layout.Add("p+0x14", "f32", unknown);
                layout.Add("p+0x18", "f32", unknown);
                layout.Add("p+0x1C", "f32", unknown);
                layout.Add("p+0x20", "f32", unknown);
                layout.Add("p+0x24", "f32", unknown);
                layout.Add("p+0x28", "f32", unknown);
                layout.Add("p+0x2C", "f32", unknown);
                layout.Add("p+0x30", "f32", unknown);
                layout.Add("p+0x34", "f32", unknown);
                layout.Add("p+0x38", "f32", unknown);
                layout.Add("p+0x3C", "f32", unknown);
                layout.Add("p+0x40", "f32", unknown);
                layout.Add("p+0x44", "f32", unknown);
                layout.Add("p+0x48", "f32", unknown);
                layout.Add("p+0x4C", "f32", unknown);
            }
            else if(name == "events4")
            {
                EventTemplate(layout);;
                layout.Add("p+0x10", "i32", unknown);
                layout.partIndex2 = layout.FieldCount();
                layout.Add("PartIndex2", "i32", important);
                layout.Add("ItemLot1", "i32", known);
                layout.Add("p+0x1C", "i32", unknown);
                layout.Add("ItemLot2", "i32", known);
                layout.Add("p+0x24", "i32", unknown);
                layout.Add("ItemLot3", "i32", known);
                layout.Add("p+0x2C", "i32", unknown);
                layout.Add("ItemLot4", "i32", known);
                layout.Add("p+0x34", "i32", unknown);
                layout.Add("ItemLot5", "i32", known);
                layout.Add("p+0x3C", "i32", unknown);
                layout.Add("p+0x40", "i32", unknown);
            }
            else if(name == "events5")
            {
                //Enemy spawner, like for the blighttown mosquitoes
                EventTemplate(layout);;

                layout.Add("p+0x10", "i16", unknown);;
                layout.Add("p+0x12", "i16", unknown);;
                layout.Add("p+0x14", "i16", unknown);;
                layout.Add("p+0x16", "i16", unknown);;

                layout.Add("p+0x18", "f32", unknown);;
                layout.Add("p+0x1C", "f32", unknown);;

                layout.Add("p+0x20", "i32", unknown);;
                layout.Add("p+0x24", "i32", unknown);;
                layout.Add("p+0x28", "i32", unknown);;
                layout.Add("p+0x2C", "i32", unknown);;
                layout.Add("p+0x30", "i32", unknown);;
                layout.Add("p+0x34", "i32", unknown);;
                layout.Add("p+0x38", "i32", unknown);;
                layout.Add("p+0x3C", "i32", unknown);;
                layout.pointIndex2 = layout.FieldCount();;
                layout.Add("RegionIndex2", "i32", important);;
                layout.Add("p+0x44", "i32", unknown);
                layout.Add("p+0x48", "i32", unknown);
                layout.Add("p+0x4C", "i32", unknown);
                layout.partIndex2 = layout.FieldCount();;
                layout.Add("PartIndex2", "i32", important);
                layout.partIndex3 = layout.FieldCount();
                layout.Add("PartIndex3", "i32", important);
                layout.Add("p+0x58", "i32", unknown);
                layout.Add("p+0x5C", "i32", unknown);
                layout.Add("p+0x60", "i32", unknown);
                layout.Add("p+0x64", "i32", unknown);
                layout.Add("p+0x68", "i32", unknown);
                layout.Add("p+0x6C", "i32", unknown);
                layout.Add("p+0x70", "i32", unknown);
                layout.Add("p+0x74", "i32", unknown);
                layout.Add("p+0x78", "i32", unknown);
                layout.Add("p+0x7C", "i32", unknown);
                layout.Add("p+0x80", "i32", unknown);
                layout.Add("p+0x84", "i32", unknown);
                layout.Add("p+0x88", "i32", unknown);
                layout.Add("p+0x8C", "i32", unknown);
                layout.Add("p+0x90", "i32", unknown);
                layout.Add("p+0x94", "i32", unknown);
                layout.Add("p+0x98", "i32", unknown);
                layout.Add("p+0x9C", "i32", unknown);
                layout.Add("p+0xA0", "i32", unknown);
                layout.Add("p+0xA4", "i32", unknown);
                layout.Add("p+0xA8", "i32", unknown);
                layout.Add("p+0xAC", "i32", unknown);
                layout.Add("p+0xB0", "i32", unknown);
                layout.Add("p+0xB4", "i32", unknown);
                layout.Add("p+0xB8", "i32", unknown);
                layout.Add("p+0xBC", "i32", unknown);
                layout.Add("p+0xC0", "i32", unknown);
                layout.Add("p+0xC4", "i32", unknown);
                layout.Add("p+0xC8", "i32", unknown);
                layout.Add("p+0xCC", "i32", unknown);
                layout.Add("p+0xD0", "i32", unknown);
                layout.Add("p+0xD4", "i32", unknown);
                layout.Add("p+0xD8", "i32", unknown);
                layout.Add("p+0xDC", "i32", unknown);
                layout.Add("p+0xE0", "i32", unknown);
                layout.Add("p+0xE4", "i32", unknown);
                layout.Add("p+0xE8", "i32", unknown);
                layout.Add("p+0xEC", "i32", unknown);
                layout.Add("p+0xF0", "i32", unknown);
                layout.Add("p+0xF4", "i32", unknown);
                layout.Add("p+0xF8", "i32", unknown);
                layout.Add("p+0xFC", "i32", unknown);
                layout.Add("p+0x100", "i32", unknown);
                layout.Add("p+0x104", "i32", unknown);
                layout.Add("p+0x108", "i32", unknown);
                layout.Add("p+0x10C", "i32", unknown);
            }
            else if(name == "events6")
            {
                //Magic blood characters and the tutorial message near Petrus? Sounds like orange soapstone messages.
                EventTemplate(layout);
                //layout.Add("p+0x10", "i32", unknown);
                //layout.Add("p+0x14", "i32", unknown);
                layout.Add("Msg ID", "i16", known);
                layout.Add("p+0x12", "i16", unknown);
                layout.Add("p+0x14", "i16", unknown);
                layout.Add("p+0x16", "i16", unknown);
            }
            else if(name == "events7") //ObjAct
            {
                EventTemplate(layout);
                layout.Add("Entity ID", "i32", known);
                layout.partIndex2 = layout.FieldCount();
                layout.Add("PartIndex2", "i32", important);
                layout.Add("Parameter ID", "i16", known);
                layout.Add("p+0x1A", "i16", unknown);
                layout.Add("Event Flag ID", "i32", known);

            }
            else if(name == "events8")
            {
                EventTemplate(layout);
                layout.pointIndex2 = layout.FieldCount();
                layout.Add("RegionIndex2", "i32", important); //MEOWTODO: Check that RegionIndex2 updates right here
                layout.Add("p+0x14", "i32", unknown);
                layout.Add("p+0x18", "i32", unknown);
                layout.Add("p+0x1C", "i32", unknown);
            }
            else if(name == "events9")
            {
                EventTemplate(layout);
                layout.Add("p+0x10", "i32", unknown);
                layout.Add("p+0x14", "i32", unknown);
                layout.Add("p+0x18", "i32", unknown);
                layout.Add("p+0x1C", "i32", unknown);
            }

            else if(name == "events10")
            {
                EventTemplate(layout);
                layout.pointIndex2 = layout.FieldCount();
                layout.Add("RegionIndex2", "i32", important);
                layout.Add("p+0x14", "i32", unknown);
                layout.Add("p+0x18", "i32", unknown);
                layout.Add("p+0x1C", "i32", unknown);
            }
            else if(name == "events11")
            {
                EventTemplate(layout);
                layout.Add("p+0x10", "i32", unknown);
                layout.Add("p+0x14", "f32", unknown);
                layout.Add("p+0x18", "f32", unknown);
                layout.Add("p+0x1C", "f32", unknown);
                layout.Add("p+0x20", "f32", unknown);
                layout.Add("p+0x24", "f32", unknown);
                layout.Add("p+0x28", "i32", unknown);
                layout.Add("p+0x2C", "i32", unknown);
            }

            else if(name == "events12")
            {
                // Only used once, in the Painted World
                // Rough translation: "The world of NPC (a warrior);"
                // Point name: "Event: Initial position of the boss"
                EventTemplate(layout);
                layout.Add("NPC Host EventEntityID", "i32", known);
                layout.Add("EventEntityID", "i32", important);
                layout.pointIndex2 = layout.FieldCount();
                layout.Add("RegionIndex2", "i32", important);
                layout.Add("p+0x1C", "i32", unknown);
            }
            else if(name == "points0")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("x04", "i32", unknown);
                layout.Add("Index", "i32", known);
                layout.Add("Type", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("x28", "i32", advanced);
                layout.Add("x2C", "i32", advanced);
                layout.Add("x30", "i32", advanced);
                layout.Add("x34", "i32", advanced);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("p+0x00", "i32", unknown);
                layout.Add("p+0x04", "i32", unknown);
                layout.Add("EventEntityID", "i32", important);
            }
            else if(name == "points2")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("x04", "i32", unknown);
                layout.Add("Index", "i32", known);
                layout.Add("Type", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("x28", "i32", advanced);
                layout.Add("x2C", "i32", advanced);
                layout.Add("x30", "i32", advanced);
                layout.Add("x34", "i32", advanced);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("p+0x00", "i32", unknown);
                layout.Add("p+0x04", "i32", unknown);
                layout.Add("Radius", "f32", known);
                layout.Add("EventEntityID", "i32", important);
            }
            else if(name == "points3")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("x04", "i32", unknown);
                layout.Add("Index", "i32", known);
                layout.Add("Type", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("x28", "i32", advanced);
                layout.Add("x2C", "i32", advanced);
                layout.Add("x30", "i32", advanced);
                layout.Add("x34", "i32", advanced);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("p+0x00", "i32", unknown);
                layout.Add("p+0x04", "i32", unknown);
                layout.Add("Radius", "f32", known);
                layout.Add("Height", "f32", known);
                layout.Add("EventEntityID", "i32", important);
            }
            else if(name == "points5")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("x04", "i32", unknown);
                layout.Add("Index", "i32", known);
                layout.Add("Type", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("x28", "i32", advanced);
                layout.Add("x2C", "i32", advanced);
                layout.Add("x30", "i32", advanced);
                layout.Add("x34", "i32", advanced);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("p+0x00", "i32", unknown);
                layout.Add("p+0x04", "i32", unknown);
                layout.Add("Length", "f32", known); //MEOWTODO: Check if this is width, length, height instead of XYZ
                layout.Add("Width", "f32", known);
                layout.Add("Height", "f32", known);
                layout.Add("EventEntityID", "i32", important);
            }
            else if(name == "mapPieces0")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("DrawGroup1", "h32", known);
                layout.Add("DrawGroup2", "h32", known);
                layout.Add("DrawGroup3", "h32", known);
                layout.Add("DrawGroup4", "h32", known);
                layout.Add("x48", "i32", unknown);
                layout.Add("x4C", "i32", unknown);
                layout.Add("x50", "i32", unknown);
                layout.Add("x54", "i32", unknown);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("LightId", "i8", known);
                layout.Add("FogId", "i8", known);
                layout.Add("ScatId", "i8", known);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i8", unknown);
                layout.Add("p+x09", "i8", unknown);
                layout.Add("p+x0A", "i8", unknown);
                layout.Add("p+x0B", "i8", unknown);

                layout.Add("p+x0C", "i16", unknown);
                layout.Add("p+x0E", "i16", unknown);
                layout.Add("p+x10", "i16", unknown);
                layout.Add("p+x12", "i16", unknown);

                layout.Add("p+x14", "i32", unknown);
                layout.Add("p+x18", "i32", unknown);
                layout.Add("p+x1C", "i32", unknown);
            }
            else if(name == "objects1" || name == "objects9")
                {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("DrawGroup1", "h32", known);
                layout.Add("DrawGroup2", "h32", known);
                layout.Add("DrawGroup3", "h32", known);
                layout.Add("DrawGroup4", "h32", known);
                layout.Add("DispGroup1", "h32", known);
                layout.Add("DispGroup2", "h32", known);
                layout.Add("DispGroup3", "h32", known);
                layout.Add("DispGroup4", "h32", known);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("LightId", "i8", known);
                layout.Add("FogId", "i8", known);
                layout.Add("ScatId", "i8", known);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i32", unknown);
                layout.Add("p+x0C", "i8", unknown);
                layout.Add("p+x0D", "i8", unknown);
                layout.Add("p+x0E", "i8", unknown);
                layout.Add("p+x0F", "i8", unknown);
                layout.Add("p+x10", "i8", unknown);
                layout.Add("p+x11", "i8", unknown);
                layout.Add("p+x12", "i8", unknown);
                layout.Add("p+x13", "i8", unknown);
                layout.Add("p+x14", "i32", unknown);
                layout.Add("p+x18", "i32", unknown);
                layout.partIndex1 = layout.FieldCount();
                layout.Add("PartIndex", "i32", important);
                layout.Add("p+x20", "i32", unknown);

                //layout.Add("p+x24", "i32", unknown);

                layout.Add("p+x24", "i16", unknown);
                layout.Add("p+x26", "i16", unknown);

                layout.Add("p+x28", "i32", unknown);
                layout.Add("p+x2C", "i32", unknown);
                }
            else if(name == "creatures2" || name == "creatures10")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("x38", "i32", unknown);
                layout.Add("x3C", "i32", unknown);
                layout.Add("x40", "i32", unknown);
                layout.Add("x44", "i32", unknown);
                layout.Add("x48", "i32", unknown);
                layout.Add("x4C", "i32", unknown);
                layout.Add("x50", "i32", unknown);
                layout.Add("x54", "i32", unknown);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("LightId", "i8", known);
                layout.Add("FogId", "i8", known);
                layout.Add("ScatId", "i8", known);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i32", unknown);
                layout.Add("p+x0C", "i32", unknown);
                layout.Add("p+x10", "i32", unknown);
                layout.Add("p+x14", "i32", unknown);
                layout.Add("p+x18", "i32", unknown);
                layout.Add("p+x1C", "i32", unknown);
                layout.Add("ThinkParam", "i32", known);
                layout.Add("NPCParam", "i32", known);
                layout.Add("TalkID", "i32", known);
                layout.Add("p+x2C", "i32", unknown);
                layout.Add("ChrInitParam", "i32", known);
                layout.partIndex1 = layout.FieldCount();
                layout.Add("PartIndex", "i32", important);
                layout.Add("p+x38", "i32", unknown);
                layout.Add("p+x3C", "i32", unknown);

                //layout.Add("p+x40", "i32", unknown);
                //layout.Add("p+x44", "i32", unknown);

                layout.Add("p+x40", "i16", unknown);
                layout.Add("p+x42", "i16", unknown);
                layout.Add("p+x44", "i16", unknown);
                layout.Add("p+x46", "i16", unknown);

                layout.Add("p+x48", "i32", unknown);
                layout.Add("p+x4C", "i32", unknown);
                layout.Add("InitAnimID", "i32", known);
                layout.Add("p+x54", "i32", unknown);
            }

            else if(name == "creatures4")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("x38", "i32", unknown);
                layout.Add("x3C", "i32", unknown);
                layout.Add("x40", "i32", unknown);
                layout.Add("x44", "i32", unknown);
                layout.Add("x48", "i32", unknown);
                layout.Add("x4C", "i32", unknown);
                layout.Add("x50", "i32", unknown);
                layout.Add("x54", "i32", unknown);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("LightId", "i8", known);
                layout.Add("FogId", "i8", known);
                layout.Add("ScatId", "i8", known);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i32", unknown);
                layout.Add("p+x0C", "i32", unknown);
                layout.Add("p+x10", "i32", unknown);
                layout.Add("p+x14", "i32", unknown);
                layout.Add("p+x18", "i32", unknown);
                layout.Add("p+x1C", "i32", unknown);
                layout.Add("p+x20", "i32", unknown);
                layout.Add("p+x24", "i32", unknown);
            }
            else if(name == "collision5")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("DrawGroup1", "h32", known);
                layout.Add("DrawGroup2", "h32", known);
                layout.Add("DrawGroup3", "h32", known);
                layout.Add("DrawGroup4", "h32", known);
                layout.Add("DispGroup1", "h32", known);
                layout.Add("DispGroup2", "h32", known);
                layout.Add("DispGroup3", "h32", known);
                layout.Add("DispGroup4", "h32", known);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("p+x04", "i8", unknown);
                layout.Add("p+x05", "i8", unknown);
                layout.Add("p+x06", "i8", unknown);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i8", unknown);
                layout.Add("p+x09", "i8", unknown);
                layout.Add("p+x0A", "i8", unknown);
                layout.Add("p+x0B", "i8", unknown);
                layout.Add("p+x0C", "i8", unknown);
                layout.Add("p+x0D", "i8", unknown);
                layout.Add("p+x0E", "i8", unknown);
                layout.Add("p+x0F", "i8", unknown);
                layout.Add("p+x10", "i8", unknown);
                layout.Add("p+x11", "i8", unknown);
                layout.Add("p+x12", "i8", unknown);
                layout.Add("p+x13", "i8", unknown);
                layout.Add("p+x14", "i8", unknown);
                layout.Add("p+x15", "i8", unknown);
                layout.Add("p+x16", "i8", unknown);
                layout.Add("p+x17", "i8", unknown);
                layout.Add("p+x18", "i8", unknown);
                layout.Add("p+x19", "i8", unknown);
                layout.Add("p+x1A", "i8", unknown);
                layout.Add("p+x1B", "i8", unknown);
                layout.Add("p+x1C", "i8", unknown);
                layout.Add("p+x1D", "i8", unknown);
                layout.Add("p+x1E", "i8", unknown);
                layout.Add("p+x1F", "i8", unknown);
                layout.Add("p+x20", "i32", unknown);
                layout.Add("p+x24", "i32", unknown);
                layout.Add("p+x28", "i32", unknown);
                layout.Add("p+x2C", "i32", unknown);
                layout.Add("Vagrant Entity ID 1", "i32", known);
                layout.Add("Vagrant Entity ID 2", "i32", known);
                layout.Add("Vagrant Entity ID 3", "i32", known);
                layout.Add("p+x3C", "i16", unknown);
                layout.Add("p+x3E", "i16", unknown);
                layout.Add("Bonfire Entity ID", "i32", known);
                layout.Add("p+x44", "i32", unknown);
                layout.Add("p+x48", "i32", unknown);
                layout.Add("p+x4C", "i32", unknown);
                layout.Add("Multiplayer ID", "i32", known);
                layout.Add("p+x54", "i16", unknown);
                layout.Add("p+x56", "i16", unknown);
                layout.Add("p+x58", "i32", unknown);
                layout.Add("p+x5C", "i32", unknown);
                layout.Add("p+x60", "i32", unknown);
                layout.Add("p+x64", "i32", unknown);
            }
            else if(name == "navimesh8")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("x38", "i32", unknown);
                layout.Add("DrawGroup1", "h32", known);
                layout.Add("DrawGroup2", "h32", known);
                layout.Add("DrawGroup3", "h32", known);
                layout.Add("DrawGroup4", "h32", known);
                layout.Add("x4C", "i32", unknown);
                layout.Add("x50", "i32", unknown);
                layout.Add("x54", "i32", unknown);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("LightId", "i8", known);
                layout.Add("FogId", "i8", known);
                layout.Add("ScatId", "i8", known);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i8", unknown);
                layout.Add("p+x09", "i8", unknown);
                layout.Add("p+x0A", "i8", unknown);
                layout.Add("p+x0B", "i8", unknown);
                layout.Add("p+x0C", "i32", unknown);
                layout.Add("p+x10", "i32", unknown);
                layout.Add("p+x14", "i32", unknown);
                layout.Add("NaviMeshGroup1", "h32", known);
                layout.Add("NaviMeshGroup2", "h32", known);
                layout.Add("NaviMeshGroup3", "h32", known);
                layout.Add("NaviMeshGroup4", "h32", known);
                layout.Add("p+x28", "i32", unknown);
                layout.Add("p+x2C", "i32", unknown);
                layout.Add("p+x30", "i32", unknown);
                layout.Add("p+x34", "i32", unknown);
            }
            else if(name == "collision11")
            {
                layout.Add("&Name", "i32", advanced);
                layout.Add("Type", "i32", advanced);
                layout.Add("Index", "i32", known);
                layout.Add("Model", "i32", known);
                layout.Add("&Placeholder Model", "i32", advanced);
                layout.Add("X", "f32", known);
                layout.Add("Y", "f32", known);
                layout.Add("Z", "f32", known);
                layout.Add("RX", "f32", known);
                layout.Add("RY", "f32", known);
                layout.Add("RZ", "f32", known);
                layout.Add("SX", "f32", known);
                layout.Add("SY", "f32", known);
                layout.Add("SZ", "f32", known);
                layout.Add("DrawGroup1", "h32", known);
                layout.Add("DrawGroup2", "h32", known);
                layout.Add("DrawGroup3", "h32", known);
                layout.Add("DrawGroup4", "h32", known);
                layout.Add("DispGroup1", "h32", known);
                layout.Add("DispGroup2", "h32", known);
                layout.Add("DispGroup3", "h32", known);
                layout.Add("DispGroup4", "h32", known);
                layout.Add("x58", "i32", unknown);
                layout.Add("x5C", "i32", unknown);
                layout.Add("x60", "i32", unknown);
                layout.SetNameIndex(layout.FieldCount());
                layout.Add("Name", "string", known);
                layout.Add("Placeholder Model", "string", advanced);
                layout.Add("EventEntityID", "i32", important);
                layout.Add("p+x04", "i8", unknown);
                layout.Add("p+x05", "i8", unknown);
                layout.Add("p+x06", "i8", unknown);
                layout.Add("p+x07", "i8", unknown);
                layout.Add("p+x08", "i8", unknown);
                layout.Add("p+x09", "i8", unknown);
                layout.Add("p+x0A", "i8", unknown);
                layout.Add("p+x0B", "i8", unknown);
                layout.Add("p+x0C", "i8", unknown);
                layout.Add("p+x0D", "i8", unknown);
                layout.Add("p+x0E", "i8", unknown);
                layout.Add("p+x0F", "i8", unknown);
                layout.Add("p+x10", "i8", unknown);
                layout.Add("p+x11", "i8", unknown);
                layout.Add("p+x12", "i8", unknown);
                layout.Add("p+x13", "i8", unknown);
                layout.Add("p+x14", "i8", unknown);
                layout.Add("p+x15", "i8", unknown);
                layout.Add("p+x16", "i8", unknown);
                layout.Add("p+x17", "i8", unknown);
                layout.Add("p+x18", "i8", unknown);
                layout.Add("p+x19", "i8", unknown);
                layout.Add("p+x1A", "i8", unknown);
                layout.Add("p+x1B", "i8", unknown);
                layout.Add("p+x1C", "i16", unknown);
                layout.Add("p+x1E", "i16", unknown);
                layout.Add("p+x20", "i32", unknown);
                layout.Add("p+x24", "i32", unknown);
            }
            else
            {
                throw new Exception();
            }

            return layout;
        }
  
    }
}
