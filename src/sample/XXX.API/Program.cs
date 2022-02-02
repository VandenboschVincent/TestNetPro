//using Serilog;

Environment.SetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES", "NetPro.Startup");

var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    ApolloClientHelper.ApolloConfig(hostingContext, config, args);
                    //Serilog.Log.Logger = new Serilog.LoggerConfiguration()
                    // .ReadFrom.Configuration(config.Build())
                    // .CreateLogger(); //������Ҫ��װSerilog������ע�ͣ����serilog nuget�����ڳ����������cspro�����ļ���
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    //webBuilder.UseSerilog();
                    webBuilder.ConfigureKestrel(options =>
                    {
                    });
                });

host.Build().Run();
