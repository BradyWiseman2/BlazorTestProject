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
                            Console.WriteLine("User not found.");
                            return; // Exit if the category doesn't exist
                        }

                        // Remove the category from the context
                        context.Users.Remove(existingUser);

                        // Save changes to the database
                        context.SaveChanges();

                        // Commit the transaction
                        transaction.Commit();

                        Console.WriteLine("User deleted successfully.");
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
    public class GameRoundUserResultsRepository : IRepository<GameRoundUserResult, Tuple<int, int>>
    {
        #region StandardCRUD
        public GameRoundUserResult GetByID(Tuple<int,int> id)
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
        public void Delete(Tuple<int,int> id)
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
