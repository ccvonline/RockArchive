//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Rock.Data;

namespace Rock.Cms
{
	/// <summary>
	/// User POCO Service class
	/// </summary>
    public partial class UserService : Service<User, UserDto>
    {
        private const string VALIDATION_KEY = "D42E08ECDE448643C528C899F90BADC9411AE07F74F9BA00A81BA06FD17E3D6BA22C4AE6947DD9686A35E8538D72B471F14CDB31BD50B9F5B2A1C26E290E5FC2";

        /// <summary>
		/// Gets Users by Api Key
		/// </summary>
		/// <param name="apiKey">Api Key.</param>
		/// <returns>An enumerable list of User objects.</returns>
	    public IEnumerable<User> GetByApiKey( string apiKey )
        {
            return Repository.Find( t => ( t.ApiKey == apiKey || ( apiKey == null && t.ApiKey == null ) ) );
        }
		
		/// <summary>
		/// Gets Users by Person Id
		/// </summary>
		/// <param name="personId">Person Id.</param>
		/// <returns>An enumerable list of User objects.</returns>
	    public IEnumerable<User> GetByPersonId( int? personId )
        {
            return Repository.Find( t => ( t.PersonId == personId || ( personId == null && t.PersonId == null ) ) );
        }
		
		/// <summary>
		/// Gets User by User Name
		/// </summary>
		/// <param name="userName">User Name.</param>
		/// <returns>User object.</returns>
	    public User GetByUserName( string userName )
        {
			return Repository
				.AsQueryable( "Person" )
				.Where( u => u.UserName == userName )
				.FirstOrDefault();
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isConfirmed">if set to <c>true</c> [is confirmed].</param>
        /// <param name="currentPersonId">The current person id.</param>
        /// <returns></returns>
        public User Create( Rock.Crm.Person person,
            AuthenticationType authenticationType,
            string username,
            string password,
            bool isConfirmed,
            int? currentPersonId )
        {
            User user = this.GetByUserName( username );
            if ( user != null )
                throw new ArgumentOutOfRangeException( "username", "Username already exists" );

            DateTime createDate = DateTime.Now;

            user = new User();
            user.UserName = username;
            user.Password = EncodePassword( password );
            user.IsConfirmed = isConfirmed;
            user.CreationDate = createDate;
            user.LastPasswordChangedDate = createDate;
            if ( person != null )
                user.PersonId = person.Id;
            user.AuthenticationType = authenticationType;

            this.Add( user, currentPersonId );
            this.Save( user, currentPersonId );

            return user;
        }

        /// <summary>
        /// Changes the password after first validating the existing password
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public bool ChangePassword( User user, string oldPassword, string newPassword )
        {
            if ( !Validate( user, oldPassword ) )
                return false;

            user.Password = EncodePassword( newPassword );
            user.LastPasswordChangedDate = DateTime.Now;

            return true;
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public void ChangePassword( User user, string password )
        {
            user.Password = EncodePassword( password );
            user.LastPasswordChangedDate = DateTime.Now;
        }

        /// <summary>
        /// Unlocks the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Unlock( User user )
        {
            user.IsLockedOut = false;
            this.Save( user, null );
        }

        /// <summary>
        /// Validates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Validate( User user, string password )
        {
            if ( EncodePassword( password ) == user.Password )
            {
                if ( user.IsConfirmed ?? false )
                    if ( !user.IsLockedOut.HasValue || !user.IsLockedOut.Value )
                    {
                        user.LastLoginDate = DateTime.Now;
                        this.Save( user, null );
                        return true;
                    }
                return false;
            }
            else
            {
                UpdateFailureCount( user );
                this.Save( user, null );
                return false;
            }
        }

        private void UpdateFailureCount( User user )
        {
            int passwordAttemptWindow = 0;
            int maxInvalidPasswordAttempts = int.MaxValue;

            Rock.Web.Cache.GlobalAttributes globalAttributes = Rock.Web.Cache.GlobalAttributes.Read();
            if ( !Int32.TryParse( globalAttributes.AttributeValue( "PasswordAttemptWindow" ), out passwordAttemptWindow ) )
                passwordAttemptWindow = 0;
            if ( !Int32.TryParse( globalAttributes.AttributeValue( "MaxInvalidPasswordAttempts" ), out maxInvalidPasswordAttempts ) )
                maxInvalidPasswordAttempts = int.MaxValue;

            DateTime firstAttempt = user.FailedPasswordAttemptWindowStart ?? DateTime.MinValue;
            int attempts = user.FailedPasswordAttemptCount ?? 0;

            TimeSpan window = new TimeSpan( 0, passwordAttemptWindow, 0 );
            if ( DateTime.Now.CompareTo( firstAttempt.Add( window ) ) < 0 )
            {
                attempts++;
                if ( attempts >= maxInvalidPasswordAttempts )
                {
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                }

                user.FailedPasswordAttemptCount = attempts;
            }
            else
            {
                user.FailedPasswordAttemptCount = 1;
                user.FailedPasswordAttemptWindowStart = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the user by the encrypted confirmation code.
        /// </summary>
        /// <param name="code">The encrypted confirmation code.</param>
        /// <returns></returns>
        public User GetByConfirmationCode( string code )
        {
            if ( !string.IsNullOrEmpty( code ) )
            {
                string identifier = string.Empty;
                try { identifier = Rock.Security.Encryption.DecryptString( code ); }
                catch { }

                if ( identifier.StartsWith( "ROCK|" ) )
                {
                    string[] idParts = identifier.Split( '|' );
                    if ( idParts.Length == 4 )
                    {
                        string publicKey = idParts[1];
                        string username = idParts[2];
                        long ticks = 0;
                        if ( !long.TryParse( idParts[3], out ticks ) )
                            ticks = 0;
                        DateTime dateTime = new DateTime( ticks );

                        // Confirmation Code is only valid for an hour
                        if ( DateTime.Now.Subtract( dateTime ).Hours > 1 )
                            return null;

                        User user = this.GetByEncryptedKey( publicKey );
                        if ( user.UserName == username )
                            return user;
                    }
                }
            }

            return null;
        }

        #region Static Methods

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns></returns>
        public static User GetCurrentUser()
        {
            return GetCurrentUser( true );
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <param name="userIsOnline">if set to <c>true</c> [user is online].</param>
        /// <returns></returns>
        public static User GetCurrentUser( bool userIsOnline )
        {
            string userName = User.GetCurrentUserName();
            if ( userName != string.Empty )
            {
                if ( userName.StartsWith( "rckipid=" ) )
                {
                    Rock.Crm.PersonService personService = new Crm.PersonService();
                    Rock.Crm.Person impersonatedPerson = personService.GetByEncryptedKey( userName.Substring( 8 ) );
                    if ( impersonatedPerson != null )
                        return impersonatedPerson.ImpersonatedUser;
                }
                else
                {
                    UserService userService = new UserService();
                    User user = userService.GetByUserName( userName );

                    if ( user != null && userIsOnline )
                    {
                        // Save last activity date
                        var transaction = new Rock.Transactions.UserLastActivityTransaction();
                        transaction.UserId = user.Id;
                        transaction.LastActivityDate = DateTime.Now;
                        Rock.Transactions.RockQueue.TransactionQueue.Enqueue( transaction );
                    }

                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool Validate( string username, string password )
        {
            UserService userService = new UserService();
            User user = userService.GetByUserName( username );
            if ( user != null )
                return userService.Validate( user, password );

            return false;
        }

        internal static string EncodePassword( string password )
        {
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = HexToByte( VALIDATION_KEY );
            return Convert.ToBase64String( hash.ComputeHash( Encoding.Unicode.GetBytes( password ) ) );
        }

        private static byte[] HexToByte( string hexString )
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for ( int i = 0; i < returnBytes.Length; i++ )
                returnBytes[i] = Convert.ToByte( hexString.Substring( i * 2, 2 ), 16 );
            return returnBytes;
        }

        #endregion

    }
}
