public class StaticJsonPreviewData
{
	private static string name, path;
	private static int index, jsonMiB;
	private static bool selected;

	public static void SetProperties(string name, string path, int index, int size, bool selected)
	{
		Name = name;
		Path = path;
		Index = index;
		Selected = selected;
		JsonMiB = size;
	}

	public static string Name
	{
		get { return name; }
		set { name = value; }
	}

	public static string Path
	{
		get { return path; }
		set { path = value; }
	}

	public static int Index
	{
		get { return index; }
		set { index = value; }
	}

	public static int JsonMiB
	{
		get { return jsonMiB; }
		set { jsonMiB = value; }
	}

	public static bool Selected
	{
		get { return selected; }
		set { selected = value; }
	}
}
