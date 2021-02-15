using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using JHA.WebServices.Repository.Interface;

namespace JHA.WebServices.Repository.SqlServer.UnitTest
{
	[TestFixture]
	public static class UnitTest
	{
		#region SetUp / TearDown

		[OneTimeSetUp]
		public static void SetUp()
		{
		}

		[OneTimeTearDown]

		public static void TearDown()
		{
		}

		#endregion

		#region Unit Tests
		#endregion

		#region TwitterFeed Unit Tests

		[Test]
		public static void PersistTweet_Test()
		{
			// Arrange
			// Act
			// Assert

			Console.WriteLine("tweet persisted");
		}

		#endregion
	}
}