namespace Services.Mapping
{
	using Domain.Entities;

	public class MappingServiceFactory
	{
		public IMappingService GetMappingService(string routeName)
		{
			switch (routeName)
			{
                case "curry":
                    return new MappingService<CurryOrder>();
                case "sales-versions":
					return new SalesVersionMappingService();
				case "translation-labels":
					return new TranslationLabelMappingService();
				case "ebay-classifications":
					return new EbayCarClassificationMappingService();
				//case "integrator":
				//	return new IntegratorTemplateMappingService();
                case "lambo-vehicles":
					return new LamboVehicleMappingService();
			}

			return null;
		}
	}
}
