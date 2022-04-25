
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Back.Entities;
using Back.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Back.Services
{

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
        public CharacterService(ApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Create(CreateCharacterDto dto)
        {
            var character = _mapper.Map<Character>(dto);
            _dbContext.Characters.Add(character);
            _dbContext.SaveChanges();
        }

        public Character Get(int id)
        {
            var character = _dbContext.Characters
                .FirstOrDefault(x => x.Id == id);

            if (character is null)
                throw new KeyNotFoundException("Character Not Found");

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
            var character = _dbContext.Characters
                .FirstOrDefault(x => x.Id == id);

            if (character is null)
                throw new Exception("Character Not Found");

            character.Name = dto.Name;
            character.Class = dto.Class;
            character.Level = dto.Level;
            character.Location = dto.Location;
            character.Level = dto.Level;
            character.Money = dto.Money;
            character.Race = dto.Race;

            _dbContext.SaveChanges();

        }
        public void Delete(int id)
        {
            var character = _dbContext.Characters
                .FirstOrDefault(x => x.Id == id);

            if (character is null)
                throw new Exception("Character Not Found");

            _dbContext.Characters.Remove(character);
            _dbContext.SaveChanges();
        }
    }
}
