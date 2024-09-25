using ReadBookLib.Interfaces;
using System.Runtime.Intrinsics.Arm;
using Host = Microsoft.AspNetCore.Hosting;

namespace ReadBookLib.Services
{
	public class LocalFileService : IFileService
	{
		private Host.IHostingEnvironment Environment { get; set; }
		public LocalFileService(Host.IHostingEnvironment env) {
			Environment = env;
		}
		public async Task<string> UploadFile(IFormFile file)
		{
            var fileName = file.FileName + DateTime.Now.ToString();
            using (var sha = new System.Security.Cryptography.SHA256Managed())
			{
				byte[] textData = System.Text.Encoding.UTF8.GetBytes(fileName);
				byte[] hash = sha.ComputeHash(textData);
				fileName = BitConverter.ToString(hash).Replace("-","");
			}
			var filePath = Path.Combine(Environment.ContentRootPath, "BookFiles",fileName);
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}
			return filePath;
		}
	}
}
