
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

using Back.Entities;
using Back.Models;
using Back.Exceptions;

namespace Back.Services
{
    //todo errorhandling middleware
    //todo logging
    public interface ICharacterService
    {
        void Create(CreateCharacterDto dto);
        Character Get(int id);
        PagedResult<Character> GetAll(CharacterQuery query);
        void Update(int id, UpdateCharacterDto dto);
        void Delete(int id);


    }
    public class CharacterService : ICharacterService
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CharacterService> _logger;
        public CharacterService(ApiDbContext dbContext, IMapper mapper, ILogger<CharacterService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public void Create(CreateCharacterDto dto)
        {
            _logger.LogInformation("Character CREATE action invoked");
            var character = _mapper.Map<Character>(dto);
            _dbContext.Characters.Add(character);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Character with id: {character.Id} CREATED");
        }

        public Character Get(int id)
        {
            var character = _dbContext.Characters
                .FirstOrDefault(x => x.Id == id);

            if (character is null)
                throw new CharNotFoundException("Character Not Found");

            return character;
        }

        public PagedResult<Character> GetAll(CharacterQuery query)
        {
            var baseQuery = _dbContext
                .Characters
                .Where(r => query.SearchPhrase == null || (r.Name.ToLower()
                                                              .Contains(query.SearchPhrase.ToLower())
                                                           || r.Location.ToLower()
                                                                  .Contains(query.SearchPhrase.ToLower())
                                                                ||r.Class.ToLower()
                                                                     .Contains(query.SearchPhrase.ToLower())
                                                                     ||r.Race.ToLower()
                                                                        .Contains(query.SearchPhrase.ToLower())));


            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Character, object>>>
                {
                    { nameof(Character.Name), r => r.Name },
                    { nameof(Character.Location), r => r.Location },
                    { nameof(Character.Class), r => r.Class },
                    { nameof(Character.Race), r => r.Race},
                    { nameof(Character.Level), r => r.Level},
                    { nameof(Character.Money), r => r.Money}
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var characters = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var result = new PagedResult<Character>(characters, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public void Update(int id, UpdateCharacterDto dto)
        {
            _logger.LogInformation($"Character with id: {id} UPDATE action invoked");
            var character = _dbContext.Characters
                .FirstOrDefault(x => x.Id == id);

            if (character is null)
            {
                _logger.LogWarning($"Character with id: {id} NOT FOUND");
                throw new CharNotFoundException("Character Not Found");
            }
                

            if (character.Name != dto.Name)
            {
                if (_dbContext.Characters.Any(x => x.Name == dto.Name))
                {
                    _logger.LogWarning($"Character with id: {id} Name: {dto.Name} ALREADY TAKEN");
                    throw new NameAlreadyExistsException("Name already exists");
                }
            }
                
                

            character.Name = dto.Name;
            character.Class = dto.Class;
            character.Level = dto.Level;
            character.Location = dto.Location;
            character.Level = dto.Level;
            character.Money = dto.Money;
            character.Race = dto.Race;

            _dbContext.SaveChanges();
            _logger.LogInformation($"Character with id: {id} UPDATED");

        }
        public void Delete(int id)
        {
            _logger.LogInformation($"Character with id: {id} DELETE action invoked");
            var character = _dbContext.Characters
                .FirstOrDefault(x => x.Id == id);

            if (character is null)
            {
                _logger.LogWarning($"Character with id: {id} NOT FOUND");
                throw new CharNotFoundException("Character Not Found");
            }

            _dbContext.Characters.Remove(character);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Character with id: {id} DELETED");
        }
    }
}
