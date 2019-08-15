namespace CodeStage.Maintainer.Issues
{
	public enum IssueType:int
	{
		MissingComponent = 0,
		DuplicateComponent = 50,
		MissingReference = 100,
		EmptyArrayItem = 200,
		MissingPrefab = 300,
		DisconnectedPrefab = 400,
		EmptyMeshCollider = 500,
		EmptyMeshFilter = 510,
		EmptyAnimation = 520,
		EmptyRenderer = 600,
		UndefinedTag = 700,
		UnnamedLayer = 800,
		Error = 5000,
		Other = 100000
	}
}