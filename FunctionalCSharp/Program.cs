// See https://aka.ms/new-console-template for more information
using System;
using System.Reflection.Metadata.Ecma335;
using static StringExt;
using NUnit.Framework;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using static System.Console;

Option<string> _ = None;
Option<string> john = Some("John");
Option<string> ku = None;
ku = Some("Хуй");
WriteLine(ku);

public static class StringExt
{
    public static string ToSentenceCase(this string s)
       => s == string.Empty
          ? string.Empty
          : char.ToUpperInvariant(s[0]) + s.ToLower()[1..];
}
class ListFormatter
{
    int counter;

    string PrependCounter(string s) => $"{++counter}. {s}";

    public List<string> Format(List<string> list)
       => list
          .Select(StringExt.ToSentenceCase)
          .Select(PrependCounter)
          .ToList();
}