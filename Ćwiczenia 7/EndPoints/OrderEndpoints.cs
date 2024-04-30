using System.Data.SqlClient;
using Ćwiczenia_7.DTOs;
using Ćwiczenia_7.Services;
using Ćwiczenia_7.Validators;
using FluentValidation;

namespace Ćwiczenia_7.EndPoints;

public static class OrderEndpoints
{
    public static void RegisterOrderEndpoints(this WebApplication app)
    {
        app.MapPost("/AddOrder", AddProduct);
    }

    private static async Task<IResult> AddProduct(GetProductResponse request,
        IConfiguration configuration
        , IValidator<GetProductResponse> validator,
        IDbService dbService)
    {
        var validate = await validator.ValidateAsync(request);
        if (validate.IsValid)
            return Results.ValidationProblem(validate.ToDictionary());
        var warehouse = await dbService.GetWarehouseDetails(request.IdWarehouae);
        if (warehouse is null)
            return Results.NotFound();
        var product = await dbService.GetProductDetails(request.IdProduct);
        if (product is null)
            return Results.NotFound();
        if (request.Amount < 1)
            return Results.BadRequest();
        var order = await dbService.GetOrderDetails(request.IdProduct, request.Amount);
        if (order is null)
            return Results.NotFound();
        if (order.CreatedAt > request.CreatedAt)
            return Results.BadRequest();
        var warehouseById = await dbService.GetByOrderId(order.IdOrder);
        if (warehouseById is not null)
            return Results.Conflict();
        dbService.UpdateTable(DateTime.Now);

        return Results.Created();
    }
}