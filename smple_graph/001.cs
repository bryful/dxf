
//外部コマンドを呼ぶサンプル
//Processオブジェクトを作成する
//using System.Diagnostics;
System.Diagnostics.Process p = new System.Diagnostics.Process();
p.StartInfo.FileName = "notepad.exe";
p.StartInfo.Arguments = @"""C:\test\1.txt""";

Console.WriteLine("notepad起動");
 bool result = p.Start();

Console.WriteLine("dirを実行");
string mes =　Call(new string[] {"cmd.exe", "/c", "dir" ,@"C:\Program Files"});
Console.WriteLine(mes);