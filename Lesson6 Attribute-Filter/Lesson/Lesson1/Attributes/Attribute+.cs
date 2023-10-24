using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Lesson1.Attributes;

[Guid("5d7b68f8-bcbe-4125-b07c-2721efe0adfe")]
[Serializable]
[ReadOnly(true)]
public class Attribute_
{
    [ThreadStatic]
    [NonSerialized]
    public static string Field = "4657";
}