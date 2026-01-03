using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guidex_Backend.Infrastructure.Interface;
using Npgsql;

namespace Guidex_Backend.Infrastructure.Client
{
    public sealed class PgVectorEmbeddingsAccessor : IEmbeddingsAccessor
    {
        private const int VectorDimension = 768;

        private readonly string _connectionString;
        private readonly IEmbeddingGenerator _embeddingGenerator;

        public PgVectorEmbeddingsAccessor(
            IConfiguration config,
            IEmbeddingGenerator embeddingGenerator)
        {
            _connectionString = config.GetConnectionString("PgVector")
                ?? throw new InvalidOperationException("PgVector connection string not found");

            _embeddingGenerator = embeddingGenerator;
        }

        public async Task SaveAsync(string source, string content)
        {
            var embedding = await _embeddingGenerator.GenerateAsync(content);
            Console.WriteLine($"Generated embedding of size {embedding} for content from source {content}");

            if (embedding.Length != VectorDimension)
                throw new InvalidOperationException(
                    $"Invalid embedding dimension: {embedding.Length}"
                );

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand("""
                INSERT INTO document_embeddings (id, source, content, embedding)
                VALUES (@id, @source, @content, @embedding::vector)
            """, conn);

            cmd.Parameters.AddWithValue("id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("source", source);
            cmd.Parameters.AddWithValue("content", content);
            cmd.Parameters.AddWithValue(
                "embedding",
                ToPgVector(embedding)
            );

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IReadOnlyList<string>> SearchAsync(string query, int topK = 5)
        {
            var queryEmbedding = await _embeddingGenerator.GenerateAsync(query);

            if (queryEmbedding.Length != VectorDimension)
                throw new InvalidOperationException(
                    $"Invalid embedding dimension: {queryEmbedding.Length}"
                );

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand("""
                SELECT content
                FROM document_embeddings
                ORDER BY embedding <-> @embedding::vector
                LIMIT @topK
            """, conn);

            cmd.Parameters.AddWithValue(
                "embedding",
                ToPgVector(queryEmbedding)
            );
            cmd.Parameters.AddWithValue("topK", topK);

            var results = new List<string>();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(reader.GetString(0));
            }

            return results;
        }

        // Converts float[] → pgvector literal: [0.1,0.2,0.3]
        private static string ToPgVector(float[] vector)
        {
            return "[" + string.Join(",", vector) + "]";
        }
    }
}
