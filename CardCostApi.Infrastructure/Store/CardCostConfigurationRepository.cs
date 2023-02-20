using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Models;
using CardCostApi.Infrastructure.Entities;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using static Dapper.SqlMapper;

namespace CardCostApi.Infrastructure.Store
{
    public class CardCostConfigurationRepository : ICardCostConfigurationRepository
    {
        private readonly DbConfiguration _dbConfiguration;
        public CardCostConfigurationRepository(IOptions<DbConfiguration> dbConfiguration)
        {
            _dbConfiguration = dbConfiguration.Value;
        }

        public async Task<CardCost> GetByCountryAsync(string id)
        {
            await using var connection = new NpgsqlConnection(_dbConfiguration.ConnectionString);
            await connection.OpenAsync();

            const string sqlCommand = @"Select * FROM CardCosts where Country = @id";
            
            var queryArgs = new { Id = id };
            var cardCostEntity = await connection.QueryFirstOrDefaultAsync<CardCost>(sqlCommand, queryArgs);
            
            if (cardCostEntity == null) return null;

            return new CardCost
            {
                Cost = cardCostEntity.Cost,
                Country = cardCostEntity.Country
            };
        }

        public async Task<List<CardCost>> GetAllAsync()
        {
            await using var connection = new NpgsqlConnection(_dbConfiguration.ConnectionString);
            await connection.OpenAsync();

            const string sqlCommand = @"SELECT * FROM CardCosts";
            var result = await connection.QueryAsync<CardCostEntity>(sqlCommand);

            return result.Select(s => new CardCost
            {
                Cost = s.Cost,
                Country = s.Country
            }).ToList();
        }

        public async Task AddAsync(CardCost cardCost)
        {
            var entity = new CardCostEntity
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };

            await using var connection = new NpgsqlConnection(_dbConfiguration.ConnectionString);
            await connection.OpenAsync();

            const string sqlCommand = @"INSERT INTO CardCosts (Cost, Country) 
                                        VALUES (@Cost, @Country)";

            var queryArgs = new DynamicParameters();
            queryArgs.Add("Cost", entity.Cost);
            queryArgs.Add("Country", entity.Country);

            await connection.ExecuteAsync(sqlCommand, queryArgs);
        }

        public async Task UpdateAsync(CardCost cardCost)
        {
            var entity = new CardCostEntity
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };

            await using var connection = new NpgsqlConnection(_dbConfiguration.ConnectionString);
            await connection.OpenAsync();

            const string sqlCommand = @"UPDATE CardCosts SET Cost=@Cost WHERE Country=@Country";

            var queryArgs = new DynamicParameters();
            queryArgs.Add("Cost", entity.Cost);
            queryArgs.Add("Country", entity.Country);

            await connection.ExecuteAsync(sqlCommand, queryArgs);
        }

        public async Task DeleteAsync(CardCost cardCost)
        {
            throw new NotImplementedException();
        }
    }
}