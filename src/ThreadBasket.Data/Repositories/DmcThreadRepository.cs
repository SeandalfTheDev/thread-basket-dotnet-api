using Dapper;
using ThreadBasket.Domain.Contracts;
using ThreadBasket.Domain.Entities;

namespace ThreadBasket.Data.Repositories;

public class DmcThreadRepository(DapperContext context) : IDmcThreadRepository
{
    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        SELECT id
                        FROM dmc_thread
                        WHERE id = @Id;
                        """;
        
        var parameters = new { Id = id };
        var result = await connection.QueryFirstOrDefaultAsync<int>(statement, parameters);
        return result != default;
    }

    public async Task<bool> ExistsAsync(string floss)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        SELECT id
                        FROM dmc_thread
                        WHERE floss = @Floss;
                        """;
        
        var parameters = new { Floss = floss };
        var result = await connection.QueryFirstOrDefaultAsync<int>(statement, parameters);
        return result != default;
    }

    public async Task<int> CountAsync()
    {
        using var connection = context.CreateConnection();

        var statement = """
                        SELECT COUNT(*)
                        FROM dmc_thread;
                        """;

        return await connection.ExecuteScalarAsync<int>(statement);
    }

    public async Task<DmcThread?> GetThreadAsync(int id)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        SELECT *
                        FROM dmc_thread
                        WHERE id = @Id;
                        """;
        
        var parameters = new { Id = id };
        return await connection.QueryFirstOrDefaultAsync<DmcThread>(statement, parameters);
    }

    public async Task<IEnumerable<DmcThread>> GetThreadListAsync(int page, int size)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        SELECT *
                        FROM dmc_thread
                        LIMIT @Limit
                        OFFSET @Offset;
                        """;
        
        var parameters = new { Limit = size, Offset = (page - 1) * size };
        return await connection.QueryAsync<DmcThread>(statement, parameters);
    }

    public async Task<int?> AddThreadAsync(DmcThread thread)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        INSERT INTO dmc_thread (name, floss, web_color, created_at, updated_at)
                        VALUES (@Name, @Floss, @WebColor, @CreatedAt, @UpdatedAt)
                        RETURNING id;
                        """;
        
        return await connection.ExecuteScalarAsync<int?>(statement, thread);
    }

    public async Task<bool> UpdateThreadAsync(DmcThread thread)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        UPDATE dmc_thread
                        SET name = @Name, floss = @Floss, web_color = @WebColor, updated_at = @UpdatedAt
                        WHERE id = @Id
                        RETURNING id;
                        """;

        return await connection.ExecuteScalarAsync<int?>(statement, thread) > 0;
    }

    public async Task<bool> DeleteThreadAsync(int id)
    {
        using var connection = context.CreateConnection();

        var statement = """
                        DELETE 
                        FROM dmc_thread
                        WHERE id = @Id
                        RETURNING id;
                        """;
        
        return await connection.ExecuteScalarAsync<int?>(statement, new { Id = id }) > 0;
    }
}