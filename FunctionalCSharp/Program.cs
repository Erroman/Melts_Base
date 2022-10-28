// See https://aka.ms/new-console-template for more information
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeT = System.UInt32;
//Console.WriteLine(new PrimesBird().ElementAt(1000000 - 1)); // zero based indexing...
var f = GetFibonacci(10).GetEnumerator();
Console.WriteLine(f.Current);
f.MoveNext();
Console.WriteLine(f.Current);
//foreach (var g in GetFibonacci(10)) Console.WriteLine(g);
static IEnumerator<int> GetInts()
{
    Console.WriteLine("first");
    yield return 1;

    Console.WriteLine("second");
    yield return 2;
    Console.WriteLine("Something that would  never be printed...");
}
IEnumerable<int> GetFibonacci(int maxValue)
{
    int previous = 0;
    int current = 1;

    while (current <= maxValue)
    {
        yield return current;

        int newCurrent = previous + current;
        previous = current;
        current = newCurrent;
    }
}
class PrimesBird : IEnumerable<PrimeT>
{
    private struct CIS<T>
    {
        public T v; public Func<CIS<T>> cont;
        public CIS(T v, Func<CIS<T>> cont)
        {
            this.v = v; this.cont = cont;
        }
    }
    private CIS<PrimeT> pmlts(PrimeT p)
    {
        Func<PrimeT, CIS<PrimeT>> fn = null;
        fn = (c) => new CIS<PrimeT>(c, () => fn(c + p));
        return fn(p * p);
    }
    private CIS<CIS<PrimeT>> allmlts(CIS<PrimeT> ps)
    {
        return new CIS<CIS<PrimeT>>(pmlts(ps.v), () => allmlts(ps.cont()));
    }
    private CIS<PrimeT> merge(CIS<PrimeT> xs, CIS<PrimeT> ys)
    {
        var x = xs.v; var y = ys.v;
        if (x < y) return new CIS<PrimeT>(x, () => merge(xs.cont(), ys));
        else if (y < x) return new CIS<PrimeT>(y, () => merge(xs, ys.cont()));
        else return new CIS<PrimeT>(x, () => merge(xs.cont(), ys.cont()));
    }
    private CIS<PrimeT> cmpsts(CIS<CIS<PrimeT>> css)
    {
        return new CIS<PrimeT>(css.v.v, () => merge(css.v.cont(), cmpsts(css.cont())));
    }
    private CIS<PrimeT> minusat(PrimeT n, CIS<PrimeT> cs)
    {
        var nn = n; var ncs = cs;
        for (; ; ++nn)
        {
            if (nn >= ncs.v) ncs = ncs.cont();
            else return new CIS<PrimeT>(nn, () => minusat(++nn, ncs));
        }
    }
    private CIS<PrimeT> prms()
    {
        return new CIS<PrimeT>(2, () => minusat(3, cmpsts(allmlts(prms()))));
    }
    public IEnumerator<PrimeT> GetEnumerator()
    {
        for (var ps = prms(); ; ps = ps.cont()) yield return ps.v;
    }
    IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }

}