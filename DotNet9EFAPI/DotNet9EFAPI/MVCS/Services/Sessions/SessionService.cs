using DotNet9EFAPI.MVCS.Models._DB.Sessions.Get;
using DotNet9EFAPI.MVCS.Models.CRUD.Sessions.Get;
using DotNet9EFAPI.MVCS.Services._DB;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DotNet9EFAPI.MVCS.Services.Sessions;

public class SessionService
{
    private readonly TestDBContext _dbContext;
    public SessionService(TestDBContext dbContext)
    {
        _dbContext = dbContext ?? throw new Exception(nameof(dbContext));
    }

    /// <summary>
    /// GENERATES A SESSION FROM THE SESSION MODEL PARAMETER
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static Session GenerateSession(Session model)
    {
        if (model == null) { return new Session(); }

        return new()
        {
            UId = model?.UId,
            Email = model?.Email
        };
    }

    /// <summary>
    /// GETS A SESSION FROM DATABASE BY ID
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<GetSessionResponse> GetSessionById(GetSessionRequest model)
    {
        try
        {
            // NULL CHECK
            if (model == null) { return new GetSessionResponse { IsSuccessful = false, Message = AppMessages.NullModel }; }
            if (model.Session == null) { return new GetSessionResponse { IsSuccessful = false, Message = AppMessages.NullSession }; }

            // TODO: WHY USE .AsTracking()? What does it do?
#pragma warning disable CS8604 // Possible null reference argument.
            Session? session = await _dbContext.Sessions.AsTracking().FirstOrDefaultAsync(s => s.UId == model.Session.UId);
#pragma warning restore CS8604 // Possible null reference argument.

            if (session == null) return new GetSessionResponse { Session = null, IsSuccessful = false, Message = AppMessages.SessionNotFound };

            GetSessionResponse response = new GetSessionResponse
            {
                IsSuccessful = true,
                Session = GenerateSession(session),
                Message = AppMessages.RetrievedSessionSuccess
            };

            return response;
        }
        catch (Exception ex)
        {
            return new GetSessionResponse { Session = null, IsSuccessful = false, Message = ex.Message };
        }

    }
}
