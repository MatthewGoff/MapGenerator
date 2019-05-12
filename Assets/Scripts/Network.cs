using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Network
{
    public List<CelestialBodies.CelestialBody[]> Edges;

    public Network(List<CelestialBodyIdentifier[]> edges, CelestialBodies.CelestialBody Map)
    {
        Edges = new List<CelestialBodies.CelestialBody[]>();
        foreach (CelestialBodyIdentifier[] edge in edges)
        {
            CelestialBodies.CelestialBody body1 = Map.GetCelestialBody(edge[0]);
            CelestialBodies.CelestialBody body2 = Map.GetCelestialBody(edge[1]);
            Edges.Add(new CelestialBodies.CelestialBody[] { body1, body2 });
        }
    }

    public Network(byte[] bytes, int startIndex, CelestialBodies.CelestialBody Map)
    {
        Edges = new List<CelestialBodies.CelestialBody[]>();
        int offset = 4;
        int numberOfEdges = BitConverter.ToInt32(bytes, startIndex + offset);
        offset += 4;
        for (int i = 0; i < numberOfEdges; i++)
        {
            CelestialBodyIdentifier id1 = new CelestialBodyIdentifier(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);
            CelestialBodyIdentifier id2 = new CelestialBodyIdentifier(bytes, startIndex + offset);
            offset += BitConverter.ToInt32(bytes, startIndex + offset);

            CelestialBodies.CelestialBody body1 = Map.GetCelestialBody(id1);
            CelestialBodies.CelestialBody body2 = Map.GetCelestialBody(id2);
            Edges.Add(new CelestialBodies.CelestialBody[] { body1, body2 });
        }
    }

    public Util.LinkedList<byte> Serialize()
    {
        Util.LinkedList<byte> bytes = new Util.LinkedList<byte>();
        bytes.Append(BitConverter.GetBytes(Edges.Count));

        foreach (CelestialBodies.CelestialBody[] edge in Edges)
        {
            bytes.Append(edge[0].ID.Serialize());
            bytes.Append(edge[1].ID.Serialize());
        }

        bytes.Prepend(BitConverter.GetBytes(bytes.Length + 4));
        return bytes;
    }
}
