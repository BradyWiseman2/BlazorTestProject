using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer.Repositories
{
    public class ChatroomRepository : IRepository<Chatroom, int>
    {
        #region StandardCRUD
        public Chatroom GetByID(int id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Chatrooms.Where(c => c.ChatroomID == id).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Chatroom GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<Chatroom> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Chatrooms.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Chatroom GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(Chatroom chatroom)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.Chatrooms.Add(chatroom);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("Chatroom added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in Chatrooms Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(Chatroom chatroom)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var existingroom = context.Chatrooms.Find(chatroom.ChatroomID);
                        if (existingroom == null)
                        {
                            Console.WriteLine("Chatroom not found.");
                            return;
                        }

                        existingroom = chatroom;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("Chatroom updated successfully.");
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
                        var existingroom = context.Chatrooms.Find(id);
                        if (existingroom == null)
                        {
                            Console.WriteLine("Chatroom not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.Chatrooms.Remove(existingroom);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("Chatroom deleted successfully.");
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
