using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer.Repositories
{
    public class ChatMessageRepository : IRepository<ChatMessage, int>
    {
        #region StandardCRUD
        public ChatMessage GetByID(int id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.ChatMessages.Where(c => c.MessageID == id).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ChatMessages GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<ChatMessage> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.ChatMessages.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ChatMessages GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(ChatMessage message)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.ChatMessages.Add(message);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("Message added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in ChatMessages Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(ChatMessage message)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var existingMessage = context.ChatMessages.Find(message.MessageID);
                        if (existingMessage == null)
                        {
                            Console.WriteLine("Message not found.");
                            return;
                        }

                        existingMessage = message;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("Message updated successfully.");
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
                        var existingMessage = context.ChatMessages.Find(id);
                        if (existingMessage == null)
                        {
                            Console.WriteLine("Message not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.ChatMessages.Remove(existingMessage);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("Message deleted successfully.");
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
