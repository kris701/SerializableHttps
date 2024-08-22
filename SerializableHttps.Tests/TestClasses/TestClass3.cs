using SerializableHttps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.Tests.TestClasses
{
	public class TestClass3 : FileDataModel
	{
		public TestClass3(MemoryStream stream) : base(stream)
		{
		}

		public Guid ID { get; set; }
	}
}
