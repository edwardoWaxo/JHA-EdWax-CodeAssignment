using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace JHA.WebServices.Repository.InMemory.UnitTest
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

		[TestCase(30)]
		[TestCase(60)]
		[TestCase(90)]
		[TestCase(300)]
		[TestCase(118)]
		[TestCase(0)]
		[TestCase(467)]
		[TestCase(3600)]
		[TestCase(7200)]
		[TestCase(8800)]
		public static void CalculateTwitterRates_Test(int sampleTimeInSeconds)
		{
			// Arrange
			const int hoursAsSeconds = (60 * 60);
			const int minutesAsSeconds = 60;
			float perSecond;
			double perMinute;
			double perHour;
			const int tweetCount = 500;

			// Act
			perSecond = sampleTimeInSeconds > 0 ? (float)tweetCount / sampleTimeInSeconds : 0;
			perMinute = sampleTimeInSeconds >= minutesAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / minutesAsSeconds) : 0;
			perHour = sampleTimeInSeconds >= hoursAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / hoursAsSeconds) : 0;

			double minuteDenominator = (double)(sampleTimeInSeconds / minutesAsSeconds);
			double hourDenominator = sampleTimeInSeconds / hoursAsSeconds;
			perMinute = (double)(tweetCount / minuteDenominator);
			perHour = (double)(tweetCount / (double)(sampleTimeInSeconds / hourDenominator));

			Console.WriteLine("CalculateTwitterRates_Test ...");
			Console.WriteLine($"tweets: " +
				$"{tweetCount}, sampleTime seconds: {sampleTimeInSeconds}, " +
				$"perSecond: {perSecond}, perMinute: {perMinute}, perHour: {perHour}");
			
			// Assert
		}

		[TestCase(30)]
		[TestCase(60)]
		[TestCase(90)]
		[TestCase(300)]
		[TestCase(118)]
		[TestCase(0)]
		[TestCase(467)]
		[TestCase(3600)]
		[TestCase(7200)]
		[TestCase(8800)]
		public static void CalculateTwitterRates_Foat_Test(int sampleTimeInSeconds)
		{
			// Arrange
			const int hoursAsSeconds = (60 * 60);
			const int minutesAsSeconds = 60;
			float perSecond;
			float perMinute;
			float perHour;
			const int tweetCount = 500;

			// Act
			
			//perMinute = sampleTimeInSeconds >= minutesAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / minutesAsSeconds) : 0;
			//perHour = sampleTimeInSeconds >= hoursAsSeconds ? (float)tweetCount / (sampleTimeInSeconds / hoursAsSeconds) : 0;

			float minuteDenominator = (float)(sampleTimeInSeconds / (float) minutesAsSeconds);
			float hourDenominator = (float) sampleTimeInSeconds / (float)hoursAsSeconds;

			perSecond = sampleTimeInSeconds > 0 ? (float)tweetCount / sampleTimeInSeconds : 0;
			perMinute = (float)(tweetCount / (float) minuteDenominator);
			perHour = (float)(tweetCount / (float) hourDenominator);

			// Assert
			Assert.AreNotEqual(0, sampleTimeInSeconds);
			Console.WriteLine("CalculateTwitterRates_Foat_Test ...");
			Console.WriteLine($"tweets: " +
				$"{tweetCount}, sampleTime seconds: {sampleTimeInSeconds}, " +
				$"perSecond: {perSecond}, perMinute: {perMinute}, perHour: {perHour}");

			// Assert
		}

		#endregion
	}
}