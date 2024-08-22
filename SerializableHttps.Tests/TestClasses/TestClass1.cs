namespace SerializableHttps.Tests.TestClasses
{
	public class TestClass1
	{
		public string Text { get; set; }
		public int Value { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj is TestClass1 other)
			{
				if (other.Text != Text) return false;
				if (other.Value != Value) return false;
				return true;
			}
			return false;
		}
	}
}
