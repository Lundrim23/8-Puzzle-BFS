using System;
using System.Collections.Generic;

public class PuzzleState
{
    public int[] Board; // Bordi i lojes, perfaqeson gjendjen e enigmes.
    public int EmptyTileIndex; // Indeksi i pllakes bosh ne bord.

    // Konstruktori i klases PuzzleState qe inicializon bordi dhe gjen indeksin e pllakes bosh.
    public PuzzleState(int[] board)
    {
        Board = board;
        EmptyTileIndex = Array.IndexOf(board, 0);
    }

    // Metoda qe kontrollon nese gjendja aktuale eshte zgjidhja e enigmes.
    public bool IsGoal()
    {
        for (int i = 0; i < 8; i++)
        {
            if (Board[i] != i + 1)
                return false;
        }
        return true;
    }

    // Metoda qe gjeneron gjendjet e ardhshme te mundshme bazuar ne levizjet e lejuara te pllakes bosh.
    public List<PuzzleState> GetNextStates()
    {
        List<PuzzleState> nextStates = new List<PuzzleState>();
        int row = EmptyTileIndex / 3;
        int col = EmptyTileIndex % 3;

        // Kontrollon levizjet e mundshme dhe shton gjendjet e reja ne listen e gjendjeve te ardhshme.
        if (row > 0) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex - 3));
        if (row < 2) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex + 3));
        if (col > 0) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex - 1));
        if (col < 2) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex + 1));

        return nextStates;
    }

    // Metoda private qe nderron vendet e dy pllakave ne bord dhe kthen nje gjendje te re te enigmes.
    private PuzzleState SwapTiles(int index1, int index2)
    {
        int[] newBoard = (int[])Board.Clone();
        int temp = newBoard[index1];
        newBoard[index1] = newBoard[index2];
        newBoard[index2] = temp;
        return new PuzzleState(newBoard);
    }
}

public class PuzzleSolver
{
    // Metoda statike qe implementon algoritmin e kerkimit me gjeresi te pare (BFS) per te gjetur zgjidhjen e enigmes.
    public static PuzzleState BFS(PuzzleState initialState)
    {
        Queue<PuzzleState> queue = new Queue<PuzzleState>(); // Radha qe permban gjendjet per te eksploruar.
        HashSet<string> visited = new HashSet<string>(); // Seti qe permban gjendjet e vizituara per te shmangur perseritjen.

        // Shton gjendjen fillestare ne radhe dhe e shenon si te vizituar.
        queue.Enqueue(initialState);
        visited.Add(string.Join(",", initialState.Board));

        // Kryen kerkimin BFS derisa radha te jete bosh ose te gjendet zgjidhja.
        while (queue.Count > 0)
        {
            PuzzleState currentState = queue.Dequeue(); // Merr gjendjen aktuale nga radha.
            if (currentState.IsGoal()) // Kontrollon nese gjendja aktuale eshte zgjidhja.
                return currentState;

            // Eksploron gjendjet e ardhshme dhe i shton ato ne radhe nese nuk jane vizituar me pare.
            foreach (var nextState in currentState.GetNextStates())
            {
                string stateString = string.Join(",", nextState.Board);
                if (!visited.Contains(stateString))
                {
                    queue.Enqueue(nextState);
                    visited.Add(stateString);
                }
            }
        }
        return null; // Kthehet null nese nuk gjendet zgjidhja.
    }
}

class Program
{
    static void Main()
    {
        // Definon gjendjen fillestare te enigmes dhe krijon nje instance te klases PuzzleState.
        int[] initialBoard = { 1, 2, 3, 4, 5, 6, 0, 7, 8 };
        PuzzleState initialState = new PuzzleState(initialBoard);

        // Perdor algoritmin BFS per te gjetur zgjidhjen e enigmes.
        PuzzleState solution = PuzzleSolver.BFS(initialState);

        // Afishon zgjidhjen ne konsole nese ajo gjendet.
        if (solution != null)
        {
            Console.WriteLine("Zgjidhja u gjet:");
            for (int i = 0; i < solution.Board.Length; i++)
            {
                Console.Write(solution.Board[i] + " ");
                if ((i + 1) % 3 == 0)
                    Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Nuk u gjet zgjidhje.");
        }
    }
}
