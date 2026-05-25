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
                throw new DomainException("UploadedByUserId cannot be empty.");

            if(string.IsNullOrWhiteSpace(fileName))
                throw new DomainException("FileName cannot be empty.");

            if(fileSize <= 0)
                throw new DomainException("FileSize must be greater than zero.");

            if(fileSize > 52428800)
                throw new DomainException("FileSize exceeds the maximum allowed size of 50 MB.");

            if(string.IsNullOrWhiteSpace(storagePath))
                throw new DomainException("StoragePath cannot be empty.");

            if(Enum.IsDefined(typeof(ExamType), type) == false)
                throw new DomainException("Invalid ExamType.");


            return new Exam(uploadedByUserId, fileName, fileSize, storagePath, type);
        }
            
        public void UpdateFileName(string newFileName)
        {
            if(string.IsNullOrWhiteSpace(newFileName))
                throw new DomainException("FileName cannot be empty.");

            FileName = newFileName;
        }

        public void StartProcessing()
        {
            if (ProcessingStatus != ExamProcessingStatus.Pending)
                throw new DomainException("Only exams with Pending status can be processed.");

            ProcessingStatus = ExamProcessingStatus.Processing;
        }

        public void CompleteProcessing(string extractedData, string alteredValues)
        {
            if (ProcessingStatus != ExamProcessingStatus.Processing)
                throw new DomainException("Only exams with Processing status can be completed.");

            if(string.IsNullOrWhiteSpace(extractedData))
                throw new DomainException("ExtractedData cannot be empty.");

            if(string.IsNullOrWhiteSpace(alteredValues))
                throw new DomainException("AlteredValues cannot be empty.");

            ExtractedData = extractedData;
            AlteredValues = alteredValues;

            ProcessingStatus = ExamProcessingStatus.Completed;
        }

        public void FailProcessing()
        {
            if (ProcessingStatus != ExamProcessingStatus.Processing)
                throw new DomainException("Only exams with Processing status can be marked as failed.");

            ProcessingStatus = ExamProcessingStatus.Failed;
        }

    }
}
