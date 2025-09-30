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
        public const string UpdateUserPasswordSuccess = "USER PASSWORD WAS UPDATED SUCCESSFUL.";
        public const string UpdateUserPasswordFailed = "USER PASSWORD UPDATE FAILED!";
        public const string CannotFindUserToUpdatePasswordFailed = "CANNOT UPDATE USER TO UPDATE PASSWORD!";
        #endregion


    #endregion

    #region SESSION_MESSAGES
    public const string NullSession = "SESSION IS NULL!";
    public const string RetrievedSessionSuccess = "SESSION WAS RETRIEVED SUCCESSFULLY.";
    public const string SessionNotFound = "SESSION WAS NOT FOUND!";
    #endregion

}
