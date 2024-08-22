namespace SerializableHttps.Tests.TestClasses
{
	public class TestClass2 : TestClass1
	{
		public double DoubleValue { get; set; }
		public DateTime Time { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj is TestClass2 other)
			{
				if (other.Text != Text) return false;
				if (other.Value != Value) return false;
				if (other.DoubleValue != DoubleValue) return false;
				if (other.Time != Time) return false;
				return true;
			}
			return false;
		}
	}
}
