using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;
        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = GetNeighborhood(reader.GetInt32(reader.GetOrdinal("NeighborhoodId")))
                            };
                            walkers.Add(walker);
                        }
                        return walkers;
                    }
                }
            }
        }
        public Neighborhood GetNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT *
                        FROM NeighborHood
                        WHERE Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", neighborhoodId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Neighborhood n = new Neighborhood()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            return n;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = GetNeighborhood(reader.GetInt32(reader.GetOrdinal("NeighborhoodId")))
                            };

                            return walker;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };

                            walkers.Add(walker);
                        }
                        return walkers;
                    }
                }
            }
        }
        public List<Walk> GetWalks(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Walks.*, Dog.Id'DogId', Owner.Name'OwnerName', Dog.OwnerId, Walker.Id'WalkerId', Walker.ImageUrl, Walker.Name'WalkerName', Walker.NeighborhoodId, Neighborhood.Name'nName' FROM Walks
                        JOIN Walker ON Walks.WalkerId = Walker.Id
                        JOIN Neighborhood ON Walker.NeighborhoodId = Neighborhood.Id
                        JOIN Dog ON Walks.DogId = Dog.Id
                        JOIN Owner ON Dog.OwnerId = Owner.Id
                        WHERE WalkerId = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", walkerId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Dog = new Dog()
                                {
                                    Owner = new Owner()
                                    {
                                        Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                                    }
                                },
                                Walker = new Walker()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                    Name = reader.GetString(reader.GetOrdinal("WalkerName")),
                                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                    Neighborhood = new Neighborhood()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                        Name = reader.GetString(reader.GetOrdinal("nName"))
                                    }
                                }
                            };
                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
    }
}
