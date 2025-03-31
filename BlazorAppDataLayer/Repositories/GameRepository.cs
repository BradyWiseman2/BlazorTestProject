using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer.Repositories
{
    public class GameRepository : IRepository<Game, int>
    {
        #region StandardCRUD
        public Game GetByID(int id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Games.Where(c => c.GameID == id).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Games GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<Game> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Games.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Games GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(Game game)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.Games.Add(game);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("Game added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in Games Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(Game game)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var existingGame = context.Games.Find(game.GameID);
                        if (existingGame == null)
                        {
                            Console.WriteLine("Game not found.");
                            return;
                        }

                        existingGame = game;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("Game updated successfully.");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        transaction.Rollback();

                        Console.WriteLine("Validation errors occurred:");
                        foreach (var entityValidationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                            }
                        }
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        transaction.Rollback();

                        Console.WriteLine("SQL error occurred:");
                        Console.WriteLine($"Error Number: {sqlEx.Number}, Message: {sqlEx.Message}");
                        if (sqlEx.InnerException != null)
                        {
                            Console.WriteLine($"Inner Exception: {sqlEx.InnerException.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        Console.WriteLine($"Error occurred: {ex.Message}");
                    }
                }
            }
        }
        public void Delete(int id)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Find the category to delete
                        var existingGame = context.Games.Find(id);
                        if (existingGame == null)
                        {
                            Console.WriteLine("Game not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.Games.Remove(existingGame);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("Game deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Handle or log the error
                        Console.WriteLine($"Error occurred: {ex.Message}");
                    }
                }
            }
        }
        #endregion
    }
}
