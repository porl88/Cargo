namespace Cargo.Controllers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using Cargo.Models.Mapping;
	using Domain.Classes.CsvMapper;
	using Services.Mapping;

	public class FeedController : Controller
	{
		private string[] permittedFileTypes = { "text/plain", "application/vnd.ms-excel" };

		private string[] permittedFileExtensions = { ".txt", ".csv" };

		private readonly MappingServiceFactory mappingServiceFactory;

		public FeedController()
		{
			this.mappingServiceFactory = new MappingServiceFactory();
		}

		// GET: /feed
		public ViewResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(string feed, HttpPostedFileBase[] files)
		{
			var filePaths = new List<string>();

			foreach (var file in files)
			{
				if (this.ValidateFile(file))
				{
					var filePath = Path.GetTempFileName();
					file.SaveAs(filePath);
					filePaths.Add(filePath);
				}
				else
				{
					TempData["Error"] = string.Format("You have tried to upload an invalid file type. Allowed file types: {0}.", string.Join(", ", permittedFileExtensions));
					return View();
				}
			}

			TempData["FilePaths"] = filePaths;
			return this.RedirectToAction("Map", new { feed = feed });
		}

		// GET: /feed/map
		public ActionResult Map(string feed)
		{
			var filePaths = TempData["FilePaths"] as List<string>;

			if (filePaths != null && filePaths.Any())
			{
				TempData.Keep();

				var mapper = this.mappingServiceFactory.GetMappingService(feed);

				if (mapper != null)
				{
					var model = new ColumnMapViewModel
					{
						Columns = mapper.GetHeaders(filePaths[0]),
						FieldLabels = mapper.GetFieldLabels()
					};

					return View(model);
				}
			}

			return this.RedirectToAction("Index", "Home");
		}

		// POST: /feed/map
		[HttpPost]
		public ActionResult Map(string feed, ColumnMapViewModel model)
		{
			var filePaths = TempData["FilePaths"] as List<string>;
			if (filePaths != null && filePaths.Any())
			{
				var mapper = this.mappingServiceFactory.GetMappingService(feed);

				var results = new MapResultsViewModel
				{
					Results = mapper.Map(filePaths, this.Request.Form)
				};

				TempData["Errors"] = results.Results.Log;

				return View("Results", results);
			}

			return this.RedirectToAction("Index", "Home");
		}

		public FileContentResult DownloadErrors(string fileName)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new StreamWriter(stream))
				{
					var errors = TempData["Errors"] as List<LogEntry>;

					if (errors != null)
					{
						foreach (var error in errors)
						{
							writer.WriteLine(string.Join(",", error.RowCount, error.ColumnName, error.Value, error.Message, error.ErrorType));
						}

						writer.Flush();

						var contents = stream.ToArray();

						return new FileContentResult(contents, "application/octet-stream")
						{
							FileDownloadName = fileName
						};
					}
					{
						return null;
					}
				}
			}
		}

		public FilePathResult Download(string filePath, string fileName)
		{
			FilePathResult file;

			var extension = Path.GetExtension(fileName).ToLower();

			switch (extension)
			{
				case ".sql":
					//file = new FilePathResult(filePath, "text/plain");
					file = new FilePathResult(filePath, "application/octet-stream");
					break;
				default:
					file = new FilePathResult(filePath, "application/octet-stream");
					break;
			}

			file.FileDownloadName = fileName;

			return file;
		}

		private bool ValidateFile(HttpPostedFileBase file)
		{
			return permittedFileTypes.Contains(file.ContentType) && permittedFileExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
		}
	}
}
