namespace MyBlog.Code
{
	public struct MyPoint
	{
		public int Width;
        public int Height;
    }

	public class StructRefs
	{
		public StructRefs()
		{
			CalcSize(new MyPoint() { Height = 10, Width = 20 });

        }

		public int CalcSize(MyPoint point)
		{
			return point.Width * point.Height;
		}
	}
}

