using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Domain.Core.Models;
using Domain.Enums;
using Domain.Exceptions;

public class ClinicalCase : Entity<Guid>, IAggregateRoot
{
    public Guid RequestedByUserId { get; private set; }
    public Guid? AssignedSpecialistId{ get; private set; }
    public string Title { get; private set; }
    public string ClinicalContext { get; private set; }
    public string DiagnosticHypothesis { get; private set; }
    public string Question { get; private set; }
    public SpecialityType SpecialityRequired { get; private set; }
    public CaseStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    #region collections
    private readonly List<Opinion> _opinions = new();
    public IReadOnlyCollection<Opinion> Opinions => _opinions.AsReadOnly();

    private readonly List<CaseReply> _replies = new();
    public IReadOnlyCollection<CaseReply> Replies => _replies.AsReadOnly();

    private readonly List<CaseStatusHistory> _statusHistory = new();
    public IReadOnlyCollection<CaseStatusHistory> StatusHistory => _statusHistory.AsReadOnly();

    private readonly List<CaseExam> _caseExams = new();
    public IReadOnlyCollection<CaseExam> CaseExams => _caseExams.AsReadOnly();
    #endregion

    private ClinicalCase() { }

    private ClinicalCase(Guid requestedByUserId, string title,
        string clinicalContext, string diagnosticHypothesis, string question, SpecialityType specialityRequired)
    {
        Id = Guid.NewGuid();
        RequestedByUserId = requestedByUserId;
        Title = title;
        ClinicalContext = clinicalContext;
        DiagnosticHypothesis = diagnosticHypothesis;
        Question = question;
        SpecialityRequired = specialityRequired;
        Status = CaseStatus.Draft;
        CreatedAt = DateTime.UtcNow;

        _statusHistory.Add(CaseStatusHistory.CreateCaseStatusHistory(Id, null, CaseStatus.Draft, requestedByUserId)); ;
    }

    public static ClinicalCase CreateClinicalCase(Guid requestedByUserId, string title,
        string clinicalContext, string diagnosticHypothesis, string question, SpecialityType specialityRequired)
    {
        if (requestedByUserId == Guid.Empty)
            throw new DomainException("Requester id cannot be null");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be null");

        if (string.IsNullOrWhiteSpace(clinicalContext))
            throw new DomainException("Clinical context cannot be null");

        if (string.IsNullOrWhiteSpace(diagnosticHypothesis))
            throw new DomainException("Diagnotic Hypothesis cannot be null");

        if (string.IsNullOrWhiteSpace(question))
            throw new DomainException("Questio cannot be null");

        if (!Enum.IsDefined(typeof(SpecialityType), specialityRequired))
            throw new DomainException("Specility not valid");


        return new ClinicalCase(requestedByUserId, title, clinicalContext, diagnosticHypothesis, question, specialityRequired);
    }

    #region Update Methods
    public void UpdateTitle(string updatedTitle)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");

        if (string.IsNullOrWhiteSpace(updatedTitle))
            throw new DomainException("Title cannot be empty");

        Title = updatedTitle;
    }

    public void UpdateClinicalContext(string updatedClinicalContext)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");

        if (string.IsNullOrWhiteSpace(updatedClinicalContext))
            throw new DomainException("Clinical context cannot be null");

        ClinicalContext = updatedClinicalContext;
    }

    public void UpdateDiagnosticHypothesis(string updatedDiagnosticHypothesis)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");

        if (string.IsNullOrWhiteSpace(updatedDiagnosticHypothesis))
            throw new DomainException("Diagnostic Hypothesis cannot be null");

        DiagnosticHypothesis = updatedDiagnosticHypothesis;
    }

    public void UpdateQuestion(string updatedQuestion)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");

        if (string.IsNullOrWhiteSpace(updatedQuestion))
            throw new DomainException("Question cannot be null");

        Question = updatedQuestion; 
    }
    #endregion

    #region Status transition
    public void Submit(Guid UserId)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");

        if (string.IsNullOrEmpty(Title))
            throw new DomainException("Title cannot be empty during the submit action");

        if(string.IsNullOrEmpty(ClinicalContext))
            throw new DomainException("Clinical context cannot be empty during the submit action");

        if (string.IsNullOrEmpty(DiagnosticHypothesis))
            throw new DomainException("Diagnostic Hypothesis cannot be empty during the submit action");

        if (string.IsNullOrEmpty(Question))
            throw new DomainException("Question cannot be empty during the submit action");

        RegisterStatusChange(Status, CaseStatus.Open, UserId);

        Status = CaseStatus.Open;
    }
    public void AssignSpecialist(Guid specialistId)
    {
        if (Status == CaseStatus.UnderReview)
            throw new CaseAlreadyAssignedException();

        if (Status != CaseStatus.Open)
            throw new InvalidCaseStatusTransitionException();

        if (specialistId == Guid.Empty)
            throw new DomainException("Specialist id cannot be empty.");

        if (specialistId == RequestedByUserId)
            throw new DomainException("Specialist cannot be the same as the requester.");

        AssignedSpecialistId = specialistId;

        RegisterStatusChange(Status, CaseStatus.UnderReview, specialistId);

        Status = CaseStatus.UnderReview;
    }

    public void Cancel(Guid UserId)
    {
        if (Status == CaseStatus.Cancelled)
            throw new DomainException("Case is already cancelled.");

        if(Status != CaseStatus.Draft && Status != CaseStatus.Open)
            throw new DomainException("Only cases with Draft or Open status can be cancelled.");

        RegisterStatusChange(Status, CaseStatus.Cancelled, UserId);

        Status = CaseStatus.Cancelled;
    }

    #endregion

    #region Opnion and Reply Methods

    public void AddOpinion(Guid specialistId, string content, OpinionAgreement agreement, string conductRecommendation)
    {
        if (Status != CaseStatus.UnderReview)
            throw new DomainException("Only cases under review can receive opinions.");

        if(specialistId == Guid.Empty)
            throw new DomainException("Specialist id cannot be empty.");

        if(specialistId != AssignedSpecialistId)
            throw new DomainException("Only the assigned specialist can add an opinion to this case.");

        if(string.IsNullOrWhiteSpace(content))
            throw new DomainException("Opinion content cannot be empty.");

        if(!Enum.IsDefined(typeof(OpinionAgreement), agreement))
            throw new DomainException("Invalid opinion agreement value.");

        if(string.IsNullOrWhiteSpace(conductRecommendation))
            throw new DomainException("Conduct recommendation cannot be empty.");


        RegisterStatusChange(Status, CaseStatus.Answered, specialistId);

        Status = CaseStatus.Answered;

        var opinion = Opinion.CreateOpinion(Id, specialistId, content, agreement, conductRecommendation);

        _opinions.Add(opinion);
    }

    public void AddReply(Guid userId, string content)
    {
        if (Status != CaseStatus.Answered)
            throw new DomainException("Only cases with Answered status can receive replies.");

        if (userId == Guid.Empty)
            throw new DomainException("User id cannot be empty.");

        if(userId != RequestedByUserId)
            throw new DomainException("Only the requester can add a reply to this case.");

        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Reply content cannot be empty.");

        RegisterStatusChange(Status, CaseStatus.UnderReview, userId);

        Status = CaseStatus.UnderReview;

        var reply = CaseReply.CreateCaseReply(Id, userId, content);

        _replies.Add(reply);
    }

    public void CloseCase(Guid userId)
    {
        if (Status != CaseStatus.Answered)
            throw new DomainException("Only cases with Answered status can be closed.");

        if (userId == Guid.Empty)
            throw new DomainException("User id cannot be empty.");

        if (userId != RequestedByUserId)
            throw new DomainException("Only the requester can close this case.");

        RegisterStatusChange(Status, CaseStatus.Closed, userId);
        Status = CaseStatus.Closed;
    }

    #endregion

    #region Case exam methods
    public void LinkExam(Guid examId)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");
        if(examId == Guid.Empty)
            throw new DomainException("Exam id cannot be empty.");
        if(_caseExams.Any(ce => ce.ExamId == examId))
            throw new DomainException("This exam is already linked to the case.");

        _caseExams.Add(CaseExam.CreateCaseExam(Id, examId));
    }

    public void UnlinkExam(Guid examId)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edition is not available after draft status");
        if (examId == Guid.Empty)
            throw new DomainException("Exam id cannot be empty.");

        var caseExam = _caseExams.FirstOrDefault(ce => ce.ExamId == examId);

        if (caseExam == null)
            throw new DomainException("This exam is not linked to the case.");

        _caseExams.Remove(caseExam);
    }
    #endregion

    private void RegisterStatusChange(CaseStatus? from, CaseStatus to, Guid changedByUserId)
    {
        _statusHistory.Add(CaseStatusHistory.CreateCaseStatusHistory(Id, from, to, changedByUserId));
    }
}