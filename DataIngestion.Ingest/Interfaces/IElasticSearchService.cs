using System.Threading.Tasks;

namespace DataIngestion.Ingest.Interfaces
{
	public interface IElasticSearchService
	{
		#region Public Methods

		Task<bool> Import();

		#endregion
	}
}