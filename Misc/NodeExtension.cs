using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public static class NodeExtension
{
    public static Node[] GetAllChildren(this Node node)
    {
        if (node.GetChildCount() == 0)
        {
            return new []{node};
        }

        List<Node> result = new List<Node>();

        foreach(Node child in node.GetChildren())
        {
            result.AddRange(child.GetAllChildren());
        }

        return result.ToArray();
    }
}
