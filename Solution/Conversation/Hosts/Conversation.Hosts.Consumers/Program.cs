var builder = Host.CreateApplicationBuilder(args);

var app = builder.Build();

await app.RunAsync();
