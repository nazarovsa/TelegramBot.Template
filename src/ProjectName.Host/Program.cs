using ProjectName.Host.Infrastructure;

var host = new HostBuilderFactory<Startup>()
    .CreateHostBuilder(args)
    .Build();

await host.RunAsync();