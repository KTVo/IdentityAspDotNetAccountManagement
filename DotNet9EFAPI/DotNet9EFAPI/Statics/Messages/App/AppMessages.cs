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
    public const string QueryAllItemsSuccess = "QUERIED ALL ITEMS WAS SUCCESSFUL.";
    public const string QueryAllItemsFailed = "QUERYING ALL ITEMS FAILED!";
    public const string CreatedUserSuccess = "USER WAS CREATED.";
    public const string CreatingUserFailed = "CREATING USER FAILED!";
    public const string LoggingInUserSuccess = "USER IS LOGGED IN.";
    public const string LoggingInUserFailed = "CANNOT LOGIN USER!";


    #endregion

    #region SESSION_MESSAGES
    public const string NullSession = "SESSION IS NULL!";
    public const string RetrievedSessionSuccess = "SESSION WAS RETRIEVED SUCCESSFULLY.";
    public const string SessionNotFound = "SESSION WAS NOT FOUND!";
    #endregion

}
