using System.Threading.Tasks;

namespace DataIngestion.Ingest.Interfaces
{
	public interface IDownloadDataService
	{
		bool DownloadZipFile();
	}
}