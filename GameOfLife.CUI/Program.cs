using System;
using System.Linq;
using System.Threading;

namespace GameOfLife.CUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //var board = new Board();
            var board = new TorusableBoard();
            int count = 10;
            int survivalRate = 20;
            bool isContinue = true;
            while (isContinue)
            {
                Console.WriteLine("コマンドを入力待ち");
                switch (Console.ReadLine())
                {
                    case "init":
                        Console.WriteLine("盤のサイズを整数で入力してください(行列数を個別に指定する場合は [行数 列数] のように半角スペースを挟んで入力)。");
                        var inputs = Console.ReadLine().Split(' ');
                        if (inputs.Length == 1 && int.TryParse(inputs[0], out int size))
                        {
                            board.Initialize(size, (x, y) => new Cell(x, y));
                        }
                        else if (inputs.Length == 2 && int.TryParse(inputs[0], out int inputColumnCount) && int.TryParse(inputs[1], out int inputRowCount))
                        {
                            board.Initialize(inputColumnCount, inputRowCount, (x, y) => new Cell(x, y));
                        }
                        else
                        {
                            Console.WriteLine("入力値が異常です。");
                            break;
                        }
                        board.Random(survivalRate);
                        Console.WriteLine($"第{board.Generation}世代");
                        Console.WriteLine(board.ToString());
                        break;
                    case "start":
                        if (!board.Cells.Any())
                        {
                            Console.WriteLine("初期化が完了していません。initコマンドで初期化してください。");
                            break;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            board.Next();
                            Console.WriteLine($"第{board.Generation}世代");
                            Console.WriteLine(board.ToString());
                            Thread.Sleep(100);
                        }
                        break;
                    case "count":
                        Console.WriteLine("startした際に進める世代数を入力してください。");
                        if (int.TryParse(Console.ReadLine(), out int inputCount))
                        {
                            if (1 <= inputCount)
                            {
                                count = inputCount;
                                break;
                            }
                        }
                        Console.WriteLine("1以上の整数値を入力してください。");
                        break;
                    case "reverse":
                        board.Reverse();
                        Console.WriteLine($"第{board.Generation}世代(反転)");
                        Console.WriteLine(board.ToString());
                        break;
                    case "random":
                        board.Random();
                        Console.WriteLine($"第{board.Generation}世代(ランダム)");
                        Console.WriteLine(board.ToString());
                        break;
                    case "exit":
                        isContinue = false;
                        break;
                    default:
                        if (!board.Cells.Any()) break;
                        board.Next();
                        Console.WriteLine($"第{board.Generation}世代");
                        Console.WriteLine(board.ToString());
                        break;
                }
            }
            Console.WriteLine("終了");
            Console.ReadKey();
        }
    }
}
