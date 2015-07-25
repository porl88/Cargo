namespace Domain.Tests.Classes
{
	using System;
	using System.Collections.Specialized;
	using System.Linq;
	using Domain.Classes.CsvMapper;
	using Domain.Entities;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class CsvMapperTests
	{
		private string TranslationLabelFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\classes\test-files\translation-labels.txt";

		private string CurryOrderFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\classes\test-files\curry.csv";

		[TestMethod]
		public void GetHeadersByFilePath()
		{
			// arrange
			var mapper = new CsvMapper<TranslationLabel>();

			// act
			var result = mapper.GetHeaders(this.TranslationLabelFilePath);

			// assert
			Assert.IsNotNull(result);
			Assert.AreEqual(6, result.Length);
			Assert.AreEqual("Italian", result[4]);
		}

		[TestMethod]
		public void MapByFilePath()
		{
			// arrange
			var mapper = new CsvMapper<TranslationLabel>();
			var mappings = this.GetTranslationLabelMappings();

			// make sure calling GetHeaders does not affect the result
            mapper.GetHeaders(this.TranslationLabelFilePath);
            mapper.GetHeaders(this.TranslationLabelFilePath);
            mapper.GetHeaders(this.TranslationLabelFilePath);

			// act
            var result = mapper.Map(this.TranslationLabelFilePath, mappings);

			// assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count > 0);
			Assert.AreEqual(22, result.Count);
			var firstRow = result.FirstOrDefault();
			Assert.IsNotNull(firstRow);
			Assert.AreEqual("InfinitiMonthlyPayment", firstRow.LabelName);
		}

		[TestMethod]
		public void MapByFilePathParseValues()
		{
			// arrange
			var mapper = new CsvMapper<CurryOrder>();
			var mappings = new NameValueCollection();
			mappings.Add("Curry", "0");
			mappings.Add("Quantity", "1");
			mappings.Add("Price", "2");
			mappings.Add("Total", "3");

			// make sure calling GetHeaders does not affect the result
            mapper.GetHeaders(this.CurryOrderFilePath);
            mapper.GetHeaders(this.CurryOrderFilePath);
            mapper.GetHeaders(this.CurryOrderFilePath);

			// act
            var result = mapper.Map(this.CurryOrderFilePath, mappings);

			// assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count > 0);
			Assert.AreEqual(3, result.Count);
			var lastRow = result.LastOrDefault();
			Assert.IsNotNull(lastRow);
			Assert.AreEqual("Korma", lastRow.Curry);
			Assert.AreEqual(2, lastRow.Quantity);
			Assert.AreEqual(12.5M, lastRow.Price);
			Assert.AreEqual(25M, lastRow.Total);
		}

        [TestMethod]
        public void MapByFilePathCustomMappings()
        {
            // arrange
            var mapper = new CsvMapper<CurryOrder>();

            // make sure calling GetHeaders does not affect the result
            mapper.GetHeaders(this.CurryOrderFilePath);
            mapper.GetHeaders(this.CurryOrderFilePath);
            mapper.GetHeaders(this.CurryOrderFilePath);

            // act
            mapper.OnItemDataBound += this.CurryOrder_OnItemDataBound;
            var result = mapper.Map(this.CurryOrderFilePath);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(3, result.Count);
            var lastRow = result.LastOrDefault();
            Assert.IsNotNull(lastRow);
            Assert.AreEqual("Korma", lastRow.Curry);
            Assert.AreEqual(2, lastRow.Quantity);
            Assert.AreEqual(12.5M, lastRow.Price);
            Assert.AreEqual(25M, lastRow.Total);
        }

        private CurryOrder CurryOrder_OnItemDataBound(object sender, CsvMapperEventArgs e)
        {
            return new CurryOrder
            {
                Curry = e.MapColumn<string>(0),
                Quantity = e.MapColumn<int>(1),
                Price = e.ParseColumn<decimal>(2),
                Total = e.ParseColumn<decimal>(3)
            };
        }

		private NameValueCollection GetTranslationLabelMappings()
		{
			var mappings = new NameValueCollection();
			mappings.Add("LabelName", "0");
			mappings.Add("German", "1");
			mappings.Add("English", "2");
			mappings.Add("French", "3");
			mappings.Add("Italian", "4");
			mappings.Add("Polish", "5");
			return mappings;
		}
	}
}
