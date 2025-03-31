using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer.Repositories
{
    public class TriviaQuestionRepository : IRepository<TriviaQuestion, int>
    {
        #region StandardCRUD
        public TriviaQuestion GetByID(int id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.TriviaQuestions.Where(c => c.TriviaQuestionID == id).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TriviaQuestion GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<TriviaQuestion> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.TriviaQuestions.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TriviaQuestion GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(TriviaQuestion question)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.TriviaQuestions.Add(question);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("TriviaQuestion added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in TriviaQuestion Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(TriviaQuestion question)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var existingQuestion = context.TriviaQuestions.Find(question.TriviaQuestionID);
                        if (existingQuestion == null)
                        {
                            Console.WriteLine("TriviaQuestion not found.");
                            return;
                        }

                        existingQuestion = question;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("TriviaQuestion updated successfully.");
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
                        var existingQuestion = context.TriviaQuestions.Find(id);
                        if (existingQuestion == null)
                        {
                            Console.WriteLine("Game not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.TriviaQuestions.Remove(existingQuestion);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("TriviaQuestion deleted successfully.");
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
