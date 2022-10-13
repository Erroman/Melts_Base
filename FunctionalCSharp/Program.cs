// See https://aka.ms/new-console-template for more information
using System;
using static Kuku;

Console.WriteLine("Hello, World!");
decimal x = -12;
decimal y = 0;
decimal z = 12;
Console.WriteLine("deltaf({0}) = {1}", x, deltaf(x));
Console.WriteLine("deltaf({0}) = {1}", y, deltaf(y));
Console.WriteLine("deltaf({0}) = {1}", z, deltaf(z));

Func<decimal, bool> DF = deltaf;

Console.WriteLine("Nagate(deltaf({0})) = {1}", x, DF.de);
Console.WriteLine("deltaf({0}) = {1}", y, deltaf(y));
Console.WriteLine("deltaf({0}) = {1}", z, deltaf(z));



bool deltaf(decimal i) 
{
    var j = i switch
    {
        <  0 => false,
        >  0 => false,
        _    => true,
    };
    return j; 
}
static class Kuku 
{
   public static Func<T, bool> Negate<T>(this Func<T, bool> pred)
         => t => !pred(t);
}