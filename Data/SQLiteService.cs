using Microsoft.Data.Sqlite;
using WpfApp1.Models;
namespace WpfApp1.Data;

public class SQLiteService
{
    private readonly string _connectionString =
        "Data Source=snippets.db";

    public void Initialize()
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS snippets (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                keyword TEXT UNIQUE,
                content TEXT,
                enabled INTEGER DEFAULT 1
            );
        ";

        command.ExecuteNonQuery();
    }
    public bool AddSnippet(
    string keyword,
    string content)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        // CHECK DUPLICATE
        var checkCommand =
            connection.CreateCommand();

        checkCommand.CommandText =
        @"
        SELECT COUNT(*)
        FROM snippets
        WHERE keyword = $keyword
    ";

        checkCommand.Parameters.AddWithValue(
            "$keyword",
            keyword);

        long count =
            (long)checkCommand.ExecuteScalar()!;

        if (count > 0)
        {
            return false;
        }

        // INSERT
        var command =
            connection.CreateCommand();

        command.CommandText =
        @"
        INSERT INTO snippets
        (keyword, content)
        VALUES
        ($keyword, $content)
    ";

        command.Parameters.AddWithValue(
            "$keyword",
            keyword);

        command.Parameters.AddWithValue(
            "$content",
            content);

        command.ExecuteNonQuery();

        return true;
    }
    public List<Snippet> GetAllSnippets()
    {
        var snippets =
            new List<Snippet>();

        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command =
            connection.CreateCommand();

        command.CommandText =
        @"
        SELECT
            id,
            keyword,
            content,
            enabled
        FROM snippets
        ORDER BY id DESC
    ";

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            snippets.Add(
                new Snippet
                {
                    Id = reader.GetInt32(0),
                    Keyword = reader.GetString(1),
                    Content = reader.GetString(2),
                    Enabled = reader.GetBoolean(3)
                });
        }

        return snippets;
    }
    public void UpdateSnippet(
    string keyword,
    string content)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command =
            connection.CreateCommand();

        command.CommandText =
        @"
        UPDATE snippets
        SET content = $content
        WHERE keyword = $keyword
    ";

        command.Parameters.AddWithValue(
            "$keyword",
            keyword);

        command.Parameters.AddWithValue(
            "$content",
            content);

        command.ExecuteNonQuery();
    }
    public bool KeywordExists(
    string keyword)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command =
            connection.CreateCommand();

        command.CommandText =
        @"
        SELECT COUNT(*)
        FROM snippets
        WHERE keyword = $keyword
    ";

        command.Parameters.AddWithValue(
            "$keyword",
            keyword);

        long count =
            (long)command.ExecuteScalar()!;

        return count > 0;
    }
    public void DeleteSnippet(
    string keyword)
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command =
            connection.CreateCommand();

        command.CommandText =
        @"
        DELETE FROM snippets
        WHERE keyword = $keyword
    ";

        command.Parameters.AddWithValue(
            "$keyword",
            keyword);

        command.ExecuteNonQuery();
    }
    public List<Snippet> SearchSnippets(
    string keyword)
    {
        var snippets =
            new List<Snippet>();

        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command =
            connection.CreateCommand();

        command.CommandText =
        @"
        SELECT
            id,
            keyword,
            content,
            enabled
        FROM snippets
        WHERE keyword LIKE $keyword
           OR content LIKE $keyword
        ORDER BY id DESC
    ";

        command.Parameters.AddWithValue(
            "$keyword",
            $"%{keyword}%");

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            snippets.Add(
                new Snippet
                {
                    Id = reader.GetInt32(0),
                    Keyword = reader.GetString(1),
                    Content = reader.GetString(2),
                    Enabled = reader.GetBoolean(3)
                });
        }

        return snippets;
    }
}