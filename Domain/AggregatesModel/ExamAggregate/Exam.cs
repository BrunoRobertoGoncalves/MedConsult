using Domain.Core.Models;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.AggregatesModel.ExamAggregate
{
    public class Exam : Entity<Guid>, IAggregateRoot
    {
        public Guid UploadedByUserId { get; private set; }
        public string FileName { get; private set; }
        public long FileSize { get; private set; }
        public string StoragePath { get; private set; }
        public ExamType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? ExtractedData { get; private set; }
        public string? AlteredValues { get; private set; }
        public ExamProcessingStatus ProcessingStatus { get; private set; }

        private Exam() { }

        private Exam(Guid uploadedByUserId, string fileName, long fileSize, string storagePath, ExamType type) 
        { 
            Id = Guid.NewGuid();
            UploadedByUserId = uploadedByUserId;
            FileName = fileName;
            FileSize = fileSize;
            StoragePath = storagePath;
            Type = type;
            CreatedAt = DateTime.UtcNow;
            ProcessingStatus = ExamProcessingStatus.Pending;
        }

        public static Exam CreateExam(Guid uploadedByUserId, string fileName, long fileSize, string storagePath, ExamType type)
        {
            if(uploadedByUserId == Guid.Empty)
                throw new DomainException("O id do usuário que enviou o exame não pode ser vazio.");

            if(string.IsNullOrWhiteSpace(fileName))
                throw new DomainException("O nome do arquivo não pode ser vazio.");

            if(fileSize <= 0)
                throw new DomainException("O tamanho do arquivo deve ser maior que zero.");

            if(fileSize > 52428800)
                throw new DomainException("O tamanho do arquivo excede o limite máximo de 50 MB.");

            if(string.IsNullOrWhiteSpace(storagePath))
                throw new DomainException("O caminho de armazenamento não pode ser vazio.");

            if(Enum.IsDefined(typeof(ExamType), type) == false)
                throw new DomainException("Tipo de exame inválido.");


            return new Exam(uploadedByUserId, fileName, fileSize, storagePath, type);
        }
            
        public void UpdateFileName(string newFileName)
        {
            if(string.IsNullOrWhiteSpace(newFileName))
                throw new DomainException("O nome do arquivo não pode ser vazio.");

            FileName = newFileName;
        }

        public void StartProcessing()
        {
            if (ProcessingStatus != ExamProcessingStatus.Pending)
                throw new DomainException("Somente exames com status Pendente podem ser processados.");

            ProcessingStatus = ExamProcessingStatus.Processing;
        }

        public void CompleteProcessing(string extractedData, string alteredValues)
        {
            if (ProcessingStatus != ExamProcessingStatus.Processing)
                throw new DomainException("Somente exames com status Processando podem ser concluídos.");

            if(string.IsNullOrWhiteSpace(extractedData))
                throw new DomainException("Os dados extraídos não podem ser vazios.");

            if(string.IsNullOrWhiteSpace(alteredValues))
                throw new DomainException("Os valores alterados não podem ser vazios.");

            ExtractedData = extractedData;
            AlteredValues = alteredValues;

            ProcessingStatus = ExamProcessingStatus.Completed;
        }

        public void FailProcessing()
        {
            if (ProcessingStatus != ExamProcessingStatus.Processing)
                throw new DomainException("Somente exames com status Processando podem ser marcados como falhos.");

            ProcessingStatus = ExamProcessingStatus.Failed;
        }

    }
}
