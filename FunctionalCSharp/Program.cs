// See https://aka.ms/new-console-template for more information
using System;

Console.WriteLine("Hello, World!");
var f = P;
Func<O, O, O> func = f;
O o1 = new() {n = "Хуй"};
O o2 = new() {n = "Пизда"};
P(o1, o2);
O P(O o1, O o2) { Console.WriteLine(o1.n);Console.WriteLine(o2.n); return new O(); }
var ff = func.SwapArgs();
ff(o1, o2);

//func1()
static class Kuku 
{
    public static Func<T2, T1, R> SwapArgs<T1, T2, R>(this Func<T1, T2, R> f)
 => (t2, t1) => f(t1, t2);
}
class O {
    public string n;
}
