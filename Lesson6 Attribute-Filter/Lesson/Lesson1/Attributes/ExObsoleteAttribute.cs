using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Lesson1.Attributes;

//[Obsolete("Используй версию v2", error: true)]
public class FirstClass
{

}

public class SecondClass : FirstClass
{
    
}

public class ThirdClass : SecondClass
{
    
}