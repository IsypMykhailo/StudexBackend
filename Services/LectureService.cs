using System.Linq.Expressions;
using Mapster;
using Studex.Domain.Dto;
using Studex.Domain.Models;
using Studex.Repositories;

namespace Studex.Services;

public class LectureService(ICrudRepository<Lecture> lectureRepository, IAIContentGenerator aiContentGenerator) : ILectureService
{
    public async Task<Lecture> CreateAsync(string userid, LectureDto dto, CancellationToken ct = default)
    {
        var lecture = dto.Adapt<LectureDto, Lecture>();
        if (lecture.Course.UserId != userid)
        {
            throw new UnauthorizedAccessException("You are not authorized to create a lecture for this course");
        }
        if (!string.IsNullOrEmpty(dto.PdfFilePath))
        {
            var pdfText = PdfExtractor.ExtractText(dto.PdfFilePath);
            var prompt = $"Summarize the following text in simple words for students:\n\n{pdfText}";
            lecture.Content = await aiContentGenerator.GenerateContentAsync(prompt);
        }
        else if (!string.IsNullOrEmpty(dto.Topic))
        {
            var prompt = $"Write a detailed lecture in LaTeX format on the topic: {dto.Topic}. Use simple words and cover all necessary concepts.";
            lecture.Content = await aiContentGenerator.GenerateContentAsync(prompt);
        }
        else
        {
            throw new ArgumentException("Either a topic or a PDF file must be provided for content generation");
        }
        await lectureRepository.CreateAsync(lecture, ct);
        await lectureRepository.SaveAsync(ct);
        return lecture;
    }

    public async Task<Lecture?> GetByIdAsync(Guid id, string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await lectureRepository.GetByIdAsync(id, l => l.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<IEnumerable<Lecture>> GetAllAsync(string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await lectureRepository.GetAsync(l => l.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<IEnumerable<Lecture>> GetAllByCourseIdAsync(Guid courseId, string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await lectureRepository.GetAsync(l => l.CourseId == courseId && l.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<bool> UpdateAsync(Guid id, string userId, LectureDto dto, CancellationToken ct = default)
    {
        var lecture = await lectureRepository.GetByIdAsync(id, l => l.Course.UserId == userId, ct: ct);
        if (lecture is null)
        {
            return false;
        }

        var updatedLecture = dto.Adapt<LectureDto, Lecture>();
        if (!string.IsNullOrEmpty(dto.PdfFilePath))
        {
            var pdfText = PdfExtractor.ExtractText(dto.PdfFilePath);
            var prompt = $"Summarize the following text in simple words for students:\n\n{pdfText}";
            updatedLecture.Content = await aiContentGenerator.GenerateContentAsync(prompt);
        }
        else if (!string.IsNullOrEmpty(dto.Topic))
        {
            var prompt = $"Write a detailed lecture in LaTeX format on the topic: {dto.Topic}. Use simple words and cover all necessary concepts.";
            updatedLecture.Content = await aiContentGenerator.GenerateContentAsync(prompt);
        }
        else
        {
            throw new ArgumentException("Either a topic or a PDF file must be provided for content generation");
        }
        updatedLecture.UpdatedAt = DateTime.UtcNow;
        await lectureRepository.UpdateAsync(updatedLecture.Id, updatedLecture, ct);
        await lectureRepository.SaveAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var lecture = await lectureRepository.GetByIdAsync(id, l => l.Course.UserId == userId, ct: ct);
        if (lecture is null)
        {
            return false;
        }

        await lectureRepository.DeleteAsync(lecture, ct);
        await lectureRepository.SaveAsync(ct);
        return true;
    }
}