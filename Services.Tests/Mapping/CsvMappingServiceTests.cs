namespace UnitTests.Services
{
	//using System;
	//using System.ComponentModel.DataAnnotations;
	//using System.IO;
	//using Microsoft.VisualStudio.TestTools.UnitTesting;
	//using Services.Mapping;

	//[TestClass]
	//public class CsvMappingServiceTests
	//{
	//	private Stream fileStream { get; set; }

	//	private char delimiter { get; set; }

	//	[TestInitialize]
	//	public void Init()
	//	{
	//		var filePath = AppDomain.CurrentDomain.BaseDirectory + "services/test.csv";
	//		this.delimiter = ',';
	//		this.fileStream = File.Open(filePath, FileMode.Open);
	//	}

	//	[TestCleanup]
	//	public void Cleanup()
	//	{
	//		this.fileStream.Dispose();
	//	}

	//	[TestMethod]
	//	public void GetHeadersTest()
	//	{
	//		// arrange
	//		using (var csvService = new CsvMappingService<Vehicle>(this.fileStream, this.delimiter))
	//		{
	//			// act 
	//			var result = csvService.GetHeaders();

	//			// assert
	//			Assert.IsNotNull(result);
	//			Assert.AreEqual(7, result.Length);
	//			Assert.AreEqual("Doors", result[6]);
	//		}
	//	}

	//	[TestMethod]
	//	public void MapToClassTest()
	//	{
	//		// arrange
	//		using (var csvService = new CsvMappingService<Vehicle>(this.fileStream, this.delimiter))
	//		{
	//			csvService.OnItemDataBound += CsvService_OnItemDataBound;

	//			// act 
	//			var result = csvService.MapToClass();

	//			// assert
	//			Assert.IsNotNull(result);
	//			Assert.AreEqual(6, result.Count);
	//			Assert.AreEqual("GB15 16", result[1].Registration);
	//			Assert.AreEqual("8QU 98", result[2].Registration);
	//		}
	//	}

	//	private Vehicle CsvService_OnItemDataBound(object sender, CsvMapperEventArgs e)
	//	{
	//		return new Vehicle
	//		{
	//			DealerId = e.MapColumn<int>(0),
	//			VIN = e.MapColumn<string>(1),
	//			Registration = e.MapColumn<string>(2),
	//			RegistrationDate = e.MapColumn<DateTimeOffset?>(3),
	//			Make = e.MapColumn<string>(4),
	//			Model = e.MapColumn<string>(5),
	//			Doors = e.MapColumn<int?>(6)
	//		};
	//	}

	//	private class Vehicle
	//	{
	//		public int? DealerId { get; set; }

	//		[StringLength(4)]
	//		public string VIN { get; set; }

	//		[Required]
	//		public string Registration { get; set; }

	//		public DateTimeOffset? RegistrationDate { get; set; }

	//		[Required]
	//		public string Make { get; set; }

	//		public string Model { get; set; }

	//		public int? Doors { get; set; }
	//	}
	//}
}
