using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer.Repositories
{
    public class GameRoundsRepository : IRepository<GameRound, int>
    {
        #region StandardCRUD
        public GameRound GetByID(int id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.GameRounds.Where(c => c.GameRoundID == id).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GameRounds GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<GameRound> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.GameRounds.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GameRounds GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(GameRound gameRound)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.GameRounds.Add(gameRound);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("GameRound added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in GameRounds Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(GameRound gameRound)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var existingRound = context.GameRounds.Find(gameRound.GameRoundID);
                        if (existingRound == null)
                        {
                            Console.WriteLine("GameRound not found.");
                            return;
                        }

                        existingRound = gameRound;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("GameRound updated successfully.");
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
                        var existingRound = context.GameRounds.Find(id);
                        if (existingRound == null)
                        {
                            Console.WriteLine("GameRound not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.GameRounds.Remove(existingRound);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("GameRound deleted successfully.");
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
