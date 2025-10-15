using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet9EFAPI.Statics.Messages.App;

public static class AppMessages
{
    #region GENERAL_RESPONSE_MESSAGES
    public const string NullModel = "MODEL IS NULL!";
    public const string NullParameter = "THE PARAMETER IS NULL: ";
    public const string NullResponse = "RESPONSE WAS NULL!";
    public const string responseSuccessful = "RESPONSE SUCCESSFULLY RETRIEVED!";
    #endregion

    #region EMAIL_MESSAGES
    public const string EmailSuccess = "EMAIL SENT SUCCESSFULLY.";
    
    public const string NullEmail = "EMAIL IS NULL!";
    public const string NullEmailAddress = "EMAIL ADDRESS IS NULL: ";
    public const string NullEmailSubject = "I AM SUBJECT.";
    public const string EmailBody = "THANK YOU. I AM BODY.";
    #endregion

    #region TOKEN_MESSAGES
    public const string CreatedTokenSuccess = "TOKEN WAS CREATED SUCCESSFULLY.";
    public const string CreatedTokenFailed = "TOKEN WAS NOT CREATED!";
    #endregion

    #region IDENTITY_MESSAGES
        #region IDENTITY_MESSAGES_GENERAL
        public const string QueryAllItemsSuccess = "QUERIED ALL ITEMS WAS SUCCESSFUL.";
        public const string QueryAllItemsFailed = "QUERYING ALL ITEMS FAILED!";
        public const string CreatedUserSuccess = "USER WAS CREATED.";
        public const string CreatingUserFailed = "CREATING USER FAILED!";
        public const string LoggingInUserSuccess = "USER IS LOGGED IN.";
        public const string LoggingInUserFailed = "CANNOT LOGIN USER!";
        #endregion
        
        #region IDENTITY_MESSAGES_UPDATE_USER
        public const string FindUserFailed = "FINDING USER FAILED!";
        public const string EmailChangeFailed = "EMAIL CHANGE FAILED!";
        public const string PhoneNumberChangeFailed = "PHONE NUMBER CHANGE FAILED!";
        public const string EmailChangeSuccess = "EMAIL CHANGE SUCCESSFUL!";
        public const string PhoneNumberChangeSuccess= "PHONE NUMBER CHANGED SUCCESSFULLY.";
        public const string UserChangeSuccess = "USER CHANGE SUCCESSFUL!";
        public const string UserChangeFailed = "USER CHANGE FAILED!";
        public const string UpdateUserPasswordSuccess = "USER PASSWORD WAS UPDATED SUCCESSFUL.";
        public const string UpdateUserPasswordFailed = "USER PASSWORD UPDATE FAILED!";
        public const string GenerateTokenToChangeEmailFailed = "CANNOT GENERATE TOKEN TO CHANGE EMAIL!";
        public const string GenerateTokenToChangePhoneFailed = "CANNOT GENERATE TOKEN TO CHANGE PHONE!";
        public const string CannotFindUserToUpdatePasswordFailed = "CANNOT FIND USER TO UPDATE PASSWORD!";
        public const string CannotFindUserToResetPasswordFailed = "CANNOT FIND USER TO RESET PASSWORD!";
        public const string CannotFindUserToUpdateEmailFailed = "CANNOT FIND USER TO UPDATE EMAIL!";
        public const string GeneratePasswordResetTokenSuccess = "PASSWORD RESET TOKEN GENERATED SUCCESSFULLY.";
        public const string GeneratePasswordResetTokenFailed = "PASSWORD RESET TOKEN GENERATED FAILED!";

        #endregion


    #endregion

    #region SESSION_MESSAGES
    public const string NullSession = "SESSION IS NULL!";
    public const string RetrievedSessionSuccess = "SESSION WAS RETRIEVED SUCCESSFULLY.";
    public const string SessionNotFound = "SESSION WAS NOT FOUND!";
    #endregion

}
