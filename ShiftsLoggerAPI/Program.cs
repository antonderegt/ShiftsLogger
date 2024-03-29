using ShiftsLoggerAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IShiftService>(new SqlServerShiftService(new ShiftsContext(builder.Configuration)));

var app = builder.Build();

app.MapGet("/shifts", ShiftController.GetShiftsAsync);

app.MapGet("/shifts/{id}", ShiftController.GetShiftByIdAsync);

app.MapGet("/shifts/employee/{id}", ShiftController.GetShiftsByEmployeeIdAsync);

app.MapPost("/shifts/start", ShiftController.StartShiftAsync).AddEndpointFilter(Validate.StartShiftIsValid);

app.MapPost("/shifts/end", ShiftController.EndShiftAsync).AddEndpointFilter(Validate.EndShiftIsValid);

app.MapDelete("/shifts/{id}", ShiftController.DeleteShiftAsync);

app.Run();
