using Movie_StructureCode.Application.Common;
using Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetAllSeatTypesForAdmin;
using Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypeById;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieById;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovies;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieWithShowings;
using Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovieWithTheaterCount;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatCount;

namespace Movie_StructureCode.Application.Abstractions.Services.Cache
{
    public static class CacheDependencyRegistry
    {
        private static readonly Dictionary<Type, List<CacheDependency>> _dependencies = new()
        {
            #region SeatType
            [typeof(GetSeatTypeById.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.SeatType)),
                    IdSelector = request => ((GetSeatTypeById.Query)request).Id
                }
            },
            
            [typeof(GetSeatTypeById.AdminQuery)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.SeatType)),
                    IdSelector = request => ((GetSeatTypeById.AdminQuery)request).Id
                }
            },

            [typeof(GetAllSeatTypesForAdmin.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.SeatType)),
                    IsGlobal = true
                }
            },
            #endregion

            #region Movie
            [typeof(GetMovieById.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.Movie)),
                    IdSelector = request => ((GetMovieById.Query)request).Id
                }
            },

            [typeof(GetMovieById.AdminQuery)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.Movie)),
                    IdSelector = request => ((GetMovieById.AdminQuery)request).Id
                }
            },

            [typeof(GetMovies.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.Movie)),
                    IsGlobal = true
                }
            },

            [typeof(GetMovieWithShowings.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.Movie)),
                    IdSelector = request => ((GetMovieWithShowings.Query)request).MovieId
                }
            }
            #endregion

            #region Showing
            ,
            [typeof(GetShowingsByMovieWithTheaterCount.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.Showing)),
                    IsGlobal = true
                }
            }
            #endregion

            #region ShowingSeat
            ,
            [typeof(GetSeatCount.Query)] = new List<CacheDependency>
            {
                new CacheDependency
                {
                    Entity = CacheEntityNameResolver.Resolve(typeof(Domain.Entities.ShowingSeat)),
                    IsGlobal = true
                }
            }
            #endregion
        };

        public static List<CacheDependency> GetDependencies(object request)
        {
            var key = request.GetType();
            return _dependencies.TryGetValue(key,out var deps)? deps : new List<CacheDependency>();
        } 

    }
}
