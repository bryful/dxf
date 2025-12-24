/*
 * 
 *  graph.exe 002.cs -arg 1 2 3
 * 
 */

Console.WriteLine($"引数の数は{CommandLineArgs.Length}");
Console.WriteLine($"*****");
foreach (string arg in CommandLineArgs)
{
	Console.WriteLine($"{arg}");
}
Console.WriteLine($"*****");


