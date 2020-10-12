using Godot;

public class LatticeGenerator : Spatial
{
    // Lattice Variables
    [Export] public float LatticeSpacing = 5.0f;
	public int M;
	public int N;
    
    public MeshInstance[] ALattice;
    public MeshInstance[] BLattice;

    // Glowing Light object to instanciate
    private readonly PackedScene _lightDot = GD.Load<PackedScene>("res://Scenes/LightDot.tscn");

	[Signal]
	private delegate void SetDotIndices(int i, int j);
 
	public void _on_Dimensions_Loaded(int loadM, int loadN)
	{
		M = loadM;
		N = loadN;
		GenerateLattice();
	}

	public void _on_Lattice_Updated(float[] A, float[] B)
	{
		for (int i = 0; i < N; i++)
		{
			for (int j = 0; j < M; j++)
			{
				Vector3 scale;
				if (A[i+N*j] * 15 > 0.0005f) {
					scale = new Vector3(B[i+N*j], B[i+N*j], B[i+N*j]) * 15;
				}
				else
				{
					scale = Vector3.Zero;
				}
				ALattice[i+N*j].Scale = scale;

				if (B[i+N*j] * 15 > 0.0005f) {
					scale = new Vector3(B[i+N*j], B[i+N*j], B[i+N*j]) * 15;
				}
				else
				{
					scale = Vector3.Zero;
				}
				BLattice[i+N*j].Scale = scale;
			}
		}
	}

    private void GenerateLattice()
    {
        // define lattice parameters
        Vector2 v = new Vector2(3.0f / 2.0f, Mathf.Sqrt(3.0f) / 2.0f);
        Vector2 d = new Vector2(1, 0);
        v *= LatticeSpacing;
        d *= LatticeSpacing;

		ALattice = new MeshInstance[N*M];
		BLattice = new MeshInstance[N*M];

        // generate the 'a' lattice
        for (int n = 1, i = 0; n <= N; n++, i++)
        {
            for (int m = (n % 2 == 1) ? -M+2 : -M+1, j = 0; m <= M; m += 2, j++)
            {
                // instance the scene
                MeshInstance lightDotInstance = (MeshInstance)_lightDot.Instance();
                
                // position on a lattice point
                Vector3 position = new Vector3(n * v.x - d.x, 0, -m * v.y);
                lightDotInstance.TranslateObjectLocal(position);
                
                // store the node for now
                // idea for the future: connect all signals now so that you don't have to bother with storing at all
				ALattice[i+N*j] = lightDotInstance;
                
                // add the light dot to the scene
                AddChild(lightDotInstance);
            }
        }
        
        // generate the 'b' lattice
        for (int n = 0, i = 0; n < N; n++, i++)
        {
            for (int m = (n % 2 == 1) ? -M+2 : -M+1, j = 0; m <= M; m += 2, j++)
            {
                // instance the scene
                MeshInstance lightDotInstance = (MeshInstance)_lightDot.Instance();
                
                // position on a lattice point
                Vector3 position = new Vector3(n * v.x, 0, -m * v.y);
                lightDotInstance.TranslateObjectLocal(position);

				//lightDotInstance.Scale = new Vector3(2, 2, 2);

                //lightDotInstance.Scale = new Vector3(1.2f, 1.2f, 1.2f); 
                
                // store the node for now
                // idea for the future: connect all signals now so that you don't have to bother with storing at all
                BLattice[i+N*j] = lightDotInstance;
                
                // add the light dot to the scene
                AddChild(lightDotInstance);
            }
        }
    }
}
