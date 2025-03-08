Install packages
Microsoft.EntityFrameworkCore.SqlServer
Swashbuckle.AspNetCore
Microsoft.EntityFrameworkCore.Tools

---Scaffold database
Scaffold-DbContext "Server=globetrotter.database.windows.net,1433;Database=Globetrotter;User Id=GlobetrotterUser;password=MyDbPassword1;Trusted_Connection=False;MultipleActiveResultSets=true;" "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir "Models" -ContextDir "Context" -Context "GlobetrotterContext" -Force