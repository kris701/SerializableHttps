using SerializableHttps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializableHttps.Tests.TestClasses
{
	public class TestClass3 : FileModel
	{
		public TestClass3(string fileName, MemoryStream stream) : base(fileName, stream)
		{
		}

		public Guid ID { get; set; }
	}
}
