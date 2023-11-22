using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class NodeExtensions
{
  public static IEnumerable<T> GetChildByType<T>(this Node node, bool recursive = true) where T : Node
  {
    List<T> children = new();
    int childCount = node.GetChildCount();
    for (int i = 0; i < childCount; i++)
    {
      Node child = node.GetChild(i);

      if (child is T childT)
      {
        children.Add(childT);
        continue;
      }

      if (recursive && child.GetChildCount() > 0)
      {
        children.AddRange(child.GetChildByType<T>(true));
      }
    }

    return children.AsEnumerable();
  }
}
