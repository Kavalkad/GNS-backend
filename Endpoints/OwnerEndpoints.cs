using GNS.Services;
using Microsoft.AspNetCore.Mvc;
using GNS.Endpoints.Filters;
using GNS.Contracts.Requests;
using GNS.Services.Interfaces;

namespace GNS.Endpoints
{
    public static class OwnerEndpoints
    {
        public static IEndpointRouteBuilder MapOwnerEndpoints(this IEndpointRouteBuilder app)
        {
            var owner = app.MapGroup("owner")
                               .RequireAuthorization(policy =>
                               {
                                   policy.RequireClaim(CustomClaims.OwnerClaim.Type, CustomClaims.OwnerClaim.Value);
                               }); 
            owner.MapPost("register-owner", RegisterOwner);


            owner.MapPost("create-working-hours", CreateWorkingHours);
            owner.MapGet("get-cyberclub-working-hours", GetCCWorkingHours);
            owner.MapPut("update-wh-by-wh-id", UpdateWorkingHours)
                .AddEndpointFilter<UpdateWorkingHoursFilter>()
                .AddEndpointFilter<FinalValidationFilter>();
            owner.MapDelete("delete-wh-by-ccid", DeleteWHByCCId);
            owner.MapDelete("delete-wh-by-whid", DeleteWHById);

            owner.MapPost("add-cyberclub", AddCyberClub);
            owner.MapGet("get-my-cyberclubs", GetOwnersCyberClubs);
            owner.MapPut("update-cyberclub", UpdateClub);
            owner.MapDelete("delete-cyberclub-by-id", DeleteClubById);
            owner.MapDelete("delete-cyberclub-by-name", DeleteClubByName);

            owner.MapPost("register-employee", RegisterEmployee);
            owner.MapGet("get-employees-by-cc-id", GetCyberClubEmployeesByCCId);
            owner.MapGet("get-all-employees", GetAllEmployees);
            owner.MapGet("get-employees-with-bonus", GetEmployeesWithBonus);
            owner.MapGet("get-employees-with-penalty", GetEmployeesWithPenalty);
            owner.MapPut("update-employee", UpdateEmployee);
            owner.MapDelete("delete-employee", DeleteEmployee);

            owner.MapPost("add-gaming-places", AddGamingPlaces);
            owner.MapGet("get-cyberclub-gaming-places", GetCCGamingPlaces);
            owner.MapPut("update-cyberclub-gaming-places", UpdateCCGamingPlaces);
            owner.MapDelete("delete-cyberclub-gaming-places", DeleteCCGamingPlaces);

            owner.MapPost("add-game", AddGame);
            owner.MapPost("update-game", UpdateGame);
            owner.MapDelete("delete-game", DeleteGame);
            owner.MapPost("connect-game-with-gaming-places", ConnectGameWithGPs);

            return app;
        }

        public static async Task<IResult> RegisterOwner(
            RegisterOwnerRequest request,
            IOwnerService service
        )
        {
            await service.RegisterOwner(request);
            return Results.Ok();
        }


        #region WorkingHours Operations

        public static async Task<IResult> CreateWorkingHours(
            CreateWorkingHoursRequest request,
            IWorkingHoursService service
        )
        {
            await service.CreateWorkingHours(request);

            return Results.Ok($"WorkingHours for day {request.DayOfWeek} successfully created");
        }
        public static async Task<IResult> GetCCWorkingHours(
            Guid cyberClubId,
            IWorkingHoursService service
        )
        {
            var workingHours = await service.GetWorkingHours(cyberClubId);

            return TypedResults.Ok(workingHours);
        }

        public static async Task<IResult> UpdateWorkingHours(
            UpdateWorkingHoursRequest request,
            IWorkingHoursService service)
        {
            await service.UpdateWorkingHours(request);

            return Results.Ok();
        }

        // Delete WorkingHours
        public static async Task<IResult> DeleteWHById(
            Guid whId,
            IWorkingHoursService service
        )
        {
            await service.DeleteByWHId(whId);
            return Results.Ok();
        }
        public static async Task<IResult> DeleteWHByCCId(
           Guid ccId,
           IWorkingHoursService service
       )
        {
            await service.DeleteByWHId(ccId);
            return Results.Ok();
        }


        #endregion WorkingHours Operations

        #region CyberClubs Operations
        public static async Task<IResult> AddCyberClub(
            [FromBody] AddCyberClubRequest request,
            ICyberClubService cyberClubService
        )
        {
            await cyberClubService.Add(request);
            return Results.Ok();
        }

        public static async Task<IResult> GetOwnersCyberClubs(ICyberClubService service)
        {
            var cyberClubs = await service.GetMyCyberClubs();

            return TypedResults.Ok(cyberClubs);
        }

        public static async Task<IResult> UpdateClub(
                [FromBody] UpdateCyberClubRequest request,
                ICyberClubService cyberClubService
                )
        {
            await cyberClubService.Update(request);
            return Results.Ok();
        }


        public static async Task<IResult> DeleteClubById(
            Guid id,
            ICyberClubService cyberClubService)
        {
            await cyberClubService.DeleteById(id);
            return Results.Ok($"CyberClub with id: {id} had deleted");
        }
        public static async Task<IResult> DeleteClubByName(
                    string name,
                    ICyberClubService cyberClubService)
        {
            await cyberClubService.DeleteByName(name);
            return Results.Ok($"CyberClub with name: {name} had deleted");
        }
        #endregion CyberClubs Operations

        #region Employees  Operations

        public static async Task<IResult> RegisterEmployee(
            [FromBody] RegisterEmployeeRequest request,
            IEmployeeService employeeService
        )
        {
            await employeeService.Register(request);
            return Results.Ok("Employee successfully registered");
        }
        public static async Task<IResult> GetAllEmployees(
            IEmployeeService employeeService
        )
        {
            var employees = await employeeService.GetAll();
            return TypedResults.Ok(employees);
        }
        public static async Task<IResult> GetCyberClubEmployeesByCCId(
            Guid cyberClubId,
            IEmployeeService employeeService
        )
        {
            var employees = await employeeService.GetByCCId(cyberClubId);
            return TypedResults.Ok(employees);
        }
        public static async Task<IResult> GetEmployeesWithBonus(
            IEmployeeService service
        )
        {
            var employeesWithBonus = await service.GetWithBonus();
            return TypedResults.Ok(employeesWithBonus);
        }
        public static async Task<IResult> GetEmployeesWithPenalty(
            IEmployeeService service
        )
        {
            var employeesWithPenalty = await service.GetWithPenalty();
            return TypedResults.Ok(employeesWithPenalty);
        }

        public static async Task<IResult> UpdateEmployee(
                   [FromBody] UpdateEmployeeRequest request,
                   IEmployeeService employeeService
               )
        {
            await employeeService.UpdateEmployee(request);
            return Results.Ok("Employee successfully updated");
        }

        public static async Task<IResult> DeleteEmployee(
            [FromBody] DeleteEmployeeRequest request,
            IEmployeeService employeeService
        )
        {
            await employeeService.Delete(request);
            return Results.Ok("Employee successfully deleted");
        }
        #endregion Employee Operations

        #region GamingPlaces Operations
        public static async Task<IResult> AddGamingPlaces(
            [FromBody] AddGamingPlacesRequest request,
            IGamingPlaceService service
        )
        {
            await service.AddGamingPlaces(request);
            return Results.Ok("GamingPlaces added successfully");
        }
        public static async Task<IResult> GetCCGamingPlaces(
            Guid cyberClubId,
            IGamingPlaceService service)
        {
            var gamingPlaces = await service.GetCCGamingPlaces(cyberClubId);

            return TypedResults.Ok(gamingPlaces);
        }
        public static async Task<IResult> UpdateCCGamingPlaces(
            [FromBody] UpdateCCGamingPlacesRequest request,
            IGamingPlaceService service
        )
        {
            await service.UpdateCCGamingPlaces(request);

            return Results.Ok();
        }
        public static async Task<IResult> DeleteCCGamingPlaces(
            [FromBody] DeleteCCGamingPlacesRequest request,
            IGamingPlaceService service
        )
        {
            await service.DeleteCCGamingPlaces(request);
            return Results.Ok();
        }
        #endregion GamingPlaces Operations

        #region Games Operations

        public async static Task<IResult> ConnectGameWithGPs(
            [FromBody] CreateGameGPsRequest request,
            IGameGamingPlaceService service
        )
        {
            await service.Add(request);
            return Results.Ok();
        }
        public static async Task<IResult> AddGame(
            string title,
            IGameService gameService
        )
        {
            await gameService.Add(title);
            return Results.Ok();
        }

        public static async Task<IResult> UpdateGame(
            [FromBody] UpdateGameRequest request,
            IGameService service)
        {
            await service.Update(request);
            return Results.Ok("Game successfully updated");
        }

        public static async Task<IResult> DeleteGame(
            Guid gameId,
            IGameService service
        )
        {
            await service.Delete(gameId);
            return Results.Ok("Удалили");
        }
        #endregion Games Operations
    }
}