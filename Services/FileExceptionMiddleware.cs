using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ReadBookLib.Services
{
	public class FileExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		public FileExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			await _next(context);
			if (context.Response.StatusCode == 404)
			{
				context.Response.Redirect("/Home");
			}
			if (context.Response.StatusCode == 400)
			{
				context.Response.Redirect("/Error");
			}
		}
	}
}
