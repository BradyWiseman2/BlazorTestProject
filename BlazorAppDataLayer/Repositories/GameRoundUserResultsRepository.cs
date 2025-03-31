using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer.Repositories
{
    public class GameRoundUserResultsRepository : IRepository<GameRoundUserResult, Tuple<int, int>>
    {
        #region StandardCRUD
        public GameRoundUserResult GetByID(Tuple<int, int> id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.GameRoundUserResults.Where(c => c.UserID == id.Item1 && c.GameRoundID == id.Item2).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GameRoundUserResult GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<GameRoundUserResult> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.GameRoundUserResults.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GameRoundUserResult GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(GameRoundUserResult gameResult)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.GameRoundUserResults.Add(gameResult);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("GameResult added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in GameRoundUserResult Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(GameRoundUserResult gameResult)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var existingResult = context.GameRoundUserResults.Where(c => c.UserID == gameResult.UserID && c.GameRoundID == gameResult.GameRoundID).First();
                        if (existingResult == null)
                        {
                            Console.WriteLine("GameResult not found.");
                            return;
                        }

                        existingResult = gameResult;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("GameResult updated successfully.");
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
        public void Delete(Tuple<int, int> id)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Find the category to delete
                        var existingResult = context.GameRoundUserResults.Where(c => c.UserID == id.Item1 && c.GameRoundID == id.Item2).First();
                        if (existingResult == null)
                        {
                            Console.WriteLine("GameResult not found.");
                            return;
                        }

                        // Remove the category from the context
                        context.GameRoundUserResults.Remove(existingResult);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("GameResult deleted successfully.");
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
