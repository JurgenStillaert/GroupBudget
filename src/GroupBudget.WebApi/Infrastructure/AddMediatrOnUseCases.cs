using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GroupBudget.WebApi.Infrastructure
{
	public static class MediatorExtensions
	{
		public static IServiceCollection AddMediatrOnUseCases(this IServiceCollection services)
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

			var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.UseCases.dll").ToList();
			referencedPaths.AddRange(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.Business.dll").ToList());

			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

			toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

			var useCaseAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.EndsWith(".UseCases") || x.GetName().Name.EndsWith(".Business")).ToArray();

			services.AddMediatR(useCaseAssemblies);

			return services;
		}
	}
}