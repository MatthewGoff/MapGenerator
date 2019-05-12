using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CelestialBodyIdentifier
{
    public CelestialBodyType Type;
    public Dictionary<CelestialBodyType, int> Address;

    public CelestialBodyIdentifier(CelestialBodyType typeParam)
    {
        Type = typeParam;
        Address = new Dictionary<CelestialBodyType, int>();
        foreach (CelestialBodyType type in Enum.GetValues(typeof(CelestialBodyType)))
        {
            if (type == typeParam)
            {
                Address.Add(type, 0);
            }
            else
            {
                Address.Add(type, -1);
            }
        }
    }

    public CelestialBodyIdentifier(CelestialBodyIdentifier parentID, CelestialBodyType childType, int childIndex)
    {
        Type = childType;
        Address = new Dictionary<CelestialBodyType, int>();
        foreach (CelestialBodyType type in Enum.GetValues(typeof(CelestialBodyType)))
        {
            Address.Add(type, parentID.Address[type]);
        }
        Address[childType] = childIndex;
    }

    public CelestialBodyIdentifier(byte[] bytes, int startIndex)
    {
        Address = new Dictionary<CelestialBodyType, int>();
        int offset = 4;
        Type = (CelestialBodyType)BitConverter.ToInt32(bytes, startIndex + offset);
        offset += 4;
        foreach (CelestialBodyType type in Enum.GetValues(typeof(CelestialBodyType)))
        {
            Address.Add(type, BitConverter.ToInt32(bytes, startIndex + offset));
            offset += 4;
        }
    }

    public Util.LinkedList<byte> Serialize()
    {
        Util.LinkedList<byte> bytes = new Util.LinkedList<byte>();
        bytes.Append(BitConverter.GetBytes((int)Type));

        foreach (CelestialBodyType type in Enum.GetValues(typeof(CelestialBodyType)))
        {
            bytes.Append(BitConverter.GetBytes(Address[type]));
        }

        bytes.Prepend(BitConverter.GetBytes(bytes.Length + 4));
        return bytes;
    }

    public static int LargestCelestialBodyType()
    {
        return Enum.GetValues(typeof(CelestialBodyType)).Length - 1;
    }

    public override string ToString()
    {
        string returnString = "(";
        returnString += NameOfType(Type);
        returnString += " : ";
        for (int i = LargestCelestialBodyType(); i >= 0; i--)
        {
            returnString += Address[(CelestialBodyType)i].ToString();
            returnString += ",";
        }
        returnString = returnString.Substring(0, returnString.Length - 1);
        returnString += ")";
        return returnString;
    }

    private string NameOfType(CelestialBodyType type)
    {
        switch (type)
        {
            case (CelestialBodyType.Universe):
                return "Universe";
            case (CelestialBodyType.Expanse):
                return "Expanse";
            case (CelestialBodyType.Galaxy):
                return "Galaxy";
            case (CelestialBodyType.Sector):
                return "Sector";
            case (CelestialBodyType.SolarSystem):
                return "SolarSystem";
            case (CelestialBodyType.Star):
                return "Star";
            case (CelestialBodyType.Planet):
                return "Planet";
            default:
                return "Unknown";
        }
    }
}