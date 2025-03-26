using BlazorAppDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppDataLayer
{
    public interface IRepository<T, I> //T is the database object, like Category or Product. I refers to the datatype of the Primary Key, usually int or string
    {
        T GetByID(I ID);       
        IEnumerable<T> GetAll();
        void Add(T Item);
        void Update(T Item);
        void Delete(I ID);
    }
    public class UserRepository : IRepository<User, int>
    {
        #region StandardCRUD
        public User GetByID(int id)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Users.Where(c => c.UserID == id).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Users GetByID: {ex.Message}");
                throw ex;
            }
        }
        public IEnumerable<User> GetAll()
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Users GetAll: {ex.Message}");
                throw ex;
            }
        }
        public void Add(User user)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction()) // Transaction handling
                {
                    try
                    {
                        // Add the category to the context
                        context.Users.Add(user);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction if successful
                        transaction.Commit();

                        Console.WriteLine("User added successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the error
                        Console.WriteLine($"Error occurred in Users Add: {ex.Message}");
                    }
                }
            }
        }
        public void Update(User user)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        
                        var existingUser = context.Users.Find(user.UserID);
                        if (existingUser == null)
                        {
                            Console.WriteLine("User not found.");
                            return;
                        }

                        existingUser = user;

                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine("User updated successfully.");
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
        public void Delete(int userID)
        {
            using (var context = new BlazorCasinoAppEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Find the category to delete
                        var existingUser = context.Users.Find(userID);
                        if (existingUser == null)
                        {
                            Console.WriteLine("Product not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.Users.Remove(existingUser);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("Products deleted successfully.");
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
        public User GetByASPID(string aspID)
        {
            try
            {
                using (var context = new BlazorCasinoAppEntities())
                {
                    return context.Users.Where(c => c.ASPNETID == aspID).First();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Users GetByID: {ex.Message}");
                throw ex;
            }
        }

    }
}
