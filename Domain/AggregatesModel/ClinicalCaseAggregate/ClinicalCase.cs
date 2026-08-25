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

        _statusHistory.Add(CaseStatusHistory.CreateCaseStatusHistory(Id, null, CaseStatus.Draft, requestedByUserId));
    }

    public static ClinicalCase CreateClinicalCase(Guid requestedByUserId, string title,
        string clinicalContext, string diagnosticHypothesis, string question, SpecialityType specialityRequired)
    {
        if (requestedByUserId == Guid.Empty)
            throw new DomainException("O id do solicitante não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("O título não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(clinicalContext))
            throw new DomainException("O contexto clínico não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(diagnosticHypothesis))
            throw new DomainException("A hipótese diagnóstica não pode ser vazia.");

        if (string.IsNullOrWhiteSpace(question))
            throw new DomainException("A pergunta não pode ser vazia.");

        if (!Enum.IsDefined(typeof(SpecialityType), specialityRequired))
            throw new DomainException("Especialidade inválida.");


        return new ClinicalCase(requestedByUserId, title, clinicalContext, diagnosticHypothesis, question, specialityRequired);
    }

    #region Update Methods
    public void UpdateTitle(string updatedTitle)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");

        if (string.IsNullOrWhiteSpace(updatedTitle))
            throw new DomainException("O título não pode ser vazio.");

        Title = updatedTitle;
    }

    public void UpdateClinicalContext(string updatedClinicalContext)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");

        if (string.IsNullOrWhiteSpace(updatedClinicalContext))
            throw new DomainException("O contexto clínico não pode ser vazio.");

        ClinicalContext = updatedClinicalContext;
    }

    public void UpdateDiagnosticHypothesis(string updatedDiagnosticHypothesis)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");

        if (string.IsNullOrWhiteSpace(updatedDiagnosticHypothesis))
            throw new DomainException("A hipótese diagnóstica não pode ser vazia.");

        DiagnosticHypothesis = updatedDiagnosticHypothesis;
    }

    public void UpdateQuestion(string updatedQuestion)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");

        if (string.IsNullOrWhiteSpace(updatedQuestion))
            throw new DomainException("A pergunta não pode ser vazia.");

        Question = updatedQuestion;
    }
    #endregion

    #region Status transition
    public void Submit(Guid userId)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");

        if (string.IsNullOrEmpty(Title))
            throw new DomainException("O título não pode ser vazio ao submeter o caso.");

        if(string.IsNullOrEmpty(ClinicalContext))
            throw new DomainException("O contexto clínico não pode ser vazio ao submeter o caso.");

        if (string.IsNullOrEmpty(DiagnosticHypothesis))
            throw new DomainException("A hipótese diagnóstica não pode ser vazia ao submeter o caso.");

        if (string.IsNullOrEmpty(Question))
            throw new DomainException("A pergunta não pode ser vazia ao submeter o caso.");

        RegisterStatusChange(Status, CaseStatus.Open, userId);

        Status = CaseStatus.Open;
    }
    public void AssignSpecialist(Guid specialistId)
    {
        if (Status == CaseStatus.UnderReview)
            throw new CaseAlreadyAssignedException();

        if (Status != CaseStatus.Open)
            throw new InvalidCaseStatusTransitionException();

        if (specialistId == Guid.Empty)
            throw new DomainException("O id do especialista não pode ser vazio.");

        if (specialistId == RequestedByUserId)
            throw new DomainException("O especialista não pode ser o mesmo usuário que o solicitante.");

        AssignedSpecialistId = specialistId;

        RegisterStatusChange(Status, CaseStatus.UnderReview, specialistId);

        Status = CaseStatus.UnderReview;
    }

    public void Cancel(Guid userId)
    {
        if (Status == CaseStatus.Cancelled)
            throw new DomainException("O caso já está cancelado.");

        if(Status != CaseStatus.Draft && Status != CaseStatus.Open)
            throw new DomainException("Somente casos com status Rascunho ou Aberto podem ser cancelados.");

        RegisterStatusChange(Status, CaseStatus.Cancelled, userId);

        Status = CaseStatus.Cancelled;
    }

    #endregion

    #region Opnion and Reply Methods

    public void AddOpinion(Guid specialistId, string content, OpinionAgreement agreement, string conductRecommendation)
    {
        if (Status != CaseStatus.UnderReview)
            throw new DomainException("Somente casos em análise podem receber pareceres.");

        if(specialistId == Guid.Empty)
            throw new DomainException("O id do especialista não pode ser vazio.");

        if(specialistId != AssignedSpecialistId)
            throw new DomainException("Somente o especialista designado pode adicionar um parecer a este caso.");

        if(string.IsNullOrWhiteSpace(content))
            throw new DomainException("O conteúdo do parecer não pode ser vazio.");

        if(!Enum.IsDefined(typeof(OpinionAgreement), agreement))
            throw new DomainException("Valor de concordância do parecer inválido.");

        if(string.IsNullOrWhiteSpace(conductRecommendation))
            throw new DomainException("A recomendação de conduta não pode ser vazia.");


        RegisterStatusChange(Status, CaseStatus.Answered, specialistId);

        Status = CaseStatus.Answered;

        var opinion = Opinion.CreateOpinion(Id, specialistId, content, agreement, conductRecommendation);

        _opinions.Add(opinion);
    }

    public void AddReply(Guid userId, string content)
    {
        if (Status != CaseStatus.Answered)
            throw new DomainException("Somente casos com status Respondido podem receber réplicas.");

        if (userId == Guid.Empty)
            throw new DomainException("O id do usuário não pode ser vazio.");

        if(userId != RequestedByUserId)
            throw new DomainException("Somente o solicitante pode adicionar uma réplica a este caso.");

        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("O conteúdo da réplica não pode ser vazio.");

        RegisterStatusChange(Status, CaseStatus.UnderReview, userId);

        Status = CaseStatus.UnderReview;

        var reply = CaseReply.CreateCaseReply(Id, userId, content);

        _replies.Add(reply);
    }

    public void CloseCase(Guid userId)
    {
        if (Status != CaseStatus.Answered)
            throw new DomainException("Somente casos com status Respondido podem ser encerrados.");

        if (userId == Guid.Empty)
            throw new DomainException("O id do usuário não pode ser vazio.");

        if (userId != RequestedByUserId)
            throw new DomainException("Somente o solicitante pode encerrar este caso.");

        RegisterStatusChange(Status, CaseStatus.Closed, userId);
        Status = CaseStatus.Closed;
    }

    #endregion

    #region Case exam methods
    public void LinkExam(Guid examId)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");
        if(examId == Guid.Empty)
            throw new DomainException("O id do exame não pode ser vazio.");
        if(_caseExams.Any(ce => ce.ExamId == examId))
            throw new DomainException("Este exame já está vinculado ao caso.");

        _caseExams.Add(CaseExam.CreateCaseExam(Id, examId));
    }

    public void UnlinkExam(Guid examId)
    {
        if (Status != CaseStatus.Draft)
            throw new DomainException("Edição não é permitida após o status de rascunho.");
        if (examId == Guid.Empty)
            throw new DomainException("O id do exame não pode ser vazio.");

        var caseExam = _caseExams.FirstOrDefault(ce => ce.ExamId == examId);

        if (caseExam == null)
            throw new DomainException("Este exame não está vinculado ao caso.");

        _caseExams.Remove(caseExam);
    }
    #endregion

    private void RegisterStatusChange(CaseStatus? from, CaseStatus to, Guid changedByUserId)
    {
        _statusHistory.Add(CaseStatusHistory.CreateCaseStatusHistory(Id, from, to, changedByUserId));
    }
}
