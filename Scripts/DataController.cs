using Godot;
using System;
using System.Globalization;

public class DataController : Node
{
	public int M;
	public int N;

	public float[] A;
	public float[] B;

	private String _directory = "res://Data/Default/";
	private int    _fileNum   = 1;
    
	[Signal]
	public delegate void DimensionsLoaded(int M, int N);

	[Signal]
	public delegate void LatticeUpdated(float[,] A, float[,] B);

    public override void _Ready()
    {
		LoadDimensions();
	}

	public override void _Process(float delta) {
		UpdateLattice();
	}

	private void LoadDimensions()
	{
        File MFile = new File();
        MFile.Open(_directory + "dimensions/M.txt", File.ModeFlags.Read);
        //file should be 1 line long, if it isn't something is wrong and this code is broken lol
        M = Int32.Parse(MFile.GetLine(), NumberStyles.Any);
        
        File NFile = new File();
        NFile.Open(_directory + "dimensions/N.txt", File.ModeFlags.Read);
        N = Int32.Parse(NFile.GetLine(), NumberStyles.Any);

		A = new float[N*M];
		B = new float[N*M];

		MFile.Close();
		NFile.Close();

		EmitSignal(nameof(DimensionsLoaded), M, N);
    }

	private void UpdateLattice()
	{
		File AFile = new File();
		File BFile = new File();

		AFile.Open(_directory + "A/" + _fileNum.ToString("D5") + ".txt", File.ModeFlags.Read);
		BFile.Open(_directory + "B/" + _fileNum.ToString("D5") + ".txt", File.ModeFlags.Read);

		for (int j = 0; j < M; j++) {
			String Aline = AFile.GetLine();
			String Bline = BFile.GetLine();

			String[] Awords = Aline.Split("   ");
			String[] Bwords = Bline.Split("   ");

			for (int i = 0; i < N; i++)
			{
				A[i+N*j] = float.Parse(Awords[i], NumberStyles.Any);
				B[i+N*j] = float.Parse(Bwords[i], NumberStyles.Any);
			}
		}

		_fileNum++;

		AFile.Close();
		BFile.Close();

		EmitSignal(nameof(LatticeUpdated), A, B);
	}
}
