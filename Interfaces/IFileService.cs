namespace ReadBookLib.Interfaces
{
	public interface IFileService
	{
		Task<string> UploadFile(IFormFile file);
	}
}
